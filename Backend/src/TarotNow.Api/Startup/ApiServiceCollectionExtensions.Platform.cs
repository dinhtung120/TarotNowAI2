using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;
using TarotNow.Api.Constants;
using TarotNow.Api.Middlewares;
using TarotNow.Api.Realtime;
using TarotNow.Api.Services;
using TarotNow.Application;
using TarotNow.Application.Common.Interfaces;
using TarotNow.Infrastructure;
using TarotNow.Infrastructure.Services;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;

namespace TarotNow.Api.Startup;

public static partial class ApiServiceCollectionExtensions
{
    /// <summary>
    /// Đăng ký các service nền tảng cần cho API runtime.
    /// Luồng xử lý: gắn exception handling, application/infrastructure, realtime services, authorization policies và SignalR.
    /// </summary>
    private static void AddPlatformServices(IServiceCollection services, IConfiguration configuration)
    {
        // Gắn global exception handler để mọi lỗi chưa bắt được chuẩn hóa về ProblemDetails.
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddApplicationServices(Assembly.GetExecutingAssembly());
        services.AddInfrastructureServices(configuration);
        services.AddApiObservability(configuration);
        ConfigureForwardedHeaders(services, configuration);
        services.AddScoped<IAuthCookieService, AuthCookieService>();
        services.AddSingleton<IUserPresenceTracker>(serviceProvider =>
        {
            var cacheBackendState = serviceProvider.GetService<CacheBackendState>();
            if (cacheBackendState?.UsesRedis == true)
            {
                var multiplexer = serviceProvider.GetService<StackExchange.Redis.IConnectionMultiplexer>();
                if (multiplexer is not null)
                {
                    var logger = serviceProvider
                        .GetRequiredService<ILogger<RedisUserPresenceTracker>>();
                    return new RedisUserPresenceTracker(multiplexer, logger);
                }
            }

            return new InMemoryUserPresenceTracker();
        });
        services.AddHostedService<PresenceTimeoutBackgroundService>();
        services.AddSingleton<IRealtimeBridgeSource>(serviceProvider =>
        {
            var multiplexer = serviceProvider.GetService<StackExchange.Redis.IConnectionMultiplexer>();
            if (multiplexer is null)
            {
                return new NoOpRealtimeBridgeSource();
            }

            var logger = serviceProvider.GetRequiredService<ILogger<RedisRealtimeBridgeSource>>();
            return new RedisRealtimeBridgeSource(multiplexer, logger);
        });
        services.AddHostedService<RedisRealtimeSignalRBridgeService>();
        services.AddAuthorization(options =>
        {
            // Định nghĩa policy tập trung để controller tái sử dụng và tránh lệch rule quyền.
            options.AddPolicy(ApiAuthorizationPolicies.AuthenticatedUser, ApiAuthorizationPolicies.RequireAuthenticatedUser);
            options.AddPolicy(ApiAuthorizationPolicies.AdminOnly, ApiAuthorizationPolicies.RequireAdminOnly);
        });
        ConfigureSignalR(services, configuration.GetConnectionString("Redis"));
    }

    /// <summary>
    /// Cấu hình trusted proxy cho forwarded headers.
    /// Luồng xử lý: chỉ bật khi có cờ enable, parse known proxies/networks và fail-fast nếu không có danh sách tin cậy.
    /// </summary>
    private static void ConfigureForwardedHeaders(IServiceCollection services, IConfiguration configuration)
    {
        var enabled = configuration.GetValue<bool>("ForwardedHeaders:Enabled");
        if (!enabled)
        {
            return;
        }

        var knownProxies = ReadTrustedForwardedHeaderEntries(configuration, "ForwardedHeaders:KnownProxies");
        var knownNetworks = ReadTrustedForwardedHeaderEntries(configuration, "ForwardedHeaders:KnownNetworks");

        if (knownProxies.Length == 0 && knownNetworks.Length == 0)
        {
            throw new InvalidOperationException(
                "ForwardedHeaders is enabled but no trusted proxy/network is configured. Set ForwardedHeaders:KnownProxies or ForwardedHeaders:KnownNetworks.");
        }

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.ForwardLimit = 2;
            options.RequireHeaderSymmetry = false;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
            AddKnownProxies(options, knownProxies);
            AddKnownNetworks(options, knownNetworks);

            if (options.KnownNetworks.Count == 0 && options.KnownProxies.Count == 0)
            {
                throw new InvalidOperationException(
                    "ForwardedHeaders enabled but no valid trusted proxies/networks were parsed.");
            }
        });
    }

    private static string[] ReadTrustedForwardedHeaderEntries(IConfiguration configuration, string sectionName)
    {
        return configuration.GetSection(sectionName)
            .Get<string[]>()?
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => value.Trim())
            .ToArray() ?? Array.Empty<string>();
    }

    private static void AddKnownProxies(ForwardedHeadersOptions options, IEnumerable<string> knownProxies)
    {
        foreach (var proxy in knownProxies)
        {
            if (IPAddress.TryParse(proxy, out var parsedProxy))
            {
                options.KnownProxies.Add(parsedProxy);
            }
        }
    }

    private static void AddKnownNetworks(ForwardedHeadersOptions options, IEnumerable<string> knownNetworks)
    {
        foreach (var network in knownNetworks)
        {
            var parsedNetwork = TryParseNetwork(network);
            if (parsedNetwork is null)
            {
                continue;
            }

            options.KnownNetworks.Add(parsedNetwork);
        }
    }

    private static Microsoft.AspNetCore.HttpOverrides.IPNetwork? TryParseNetwork(string network)
    {
        var parts = network.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            return null;
        }

        if (!IPAddress.TryParse(parts[0], out var prefix))
        {
            return null;
        }

        if (!int.TryParse(parts[1], out var prefixLength))
        {
            return null;
        }

        return new Microsoft.AspNetCore.HttpOverrides.IPNetwork(prefix, prefixLength);
    }

    /// <summary>
    /// Cấu hình SignalR và tùy chọn backplane Redis khi có connection string.
    /// Luồng xử lý: tạo SignalR builder, cấu hình payload JSON enum, rồi bật Redis scale-out nếu được cấu hình.
    /// </summary>
    private static void ConfigureSignalR(IServiceCollection services, string? redisConnectionString)
    {
        var signalRBuilder = services.AddSignalR(options =>
        {
            // Giới hạn cao hơn mặc định để hỗ trợ payload media metadata lớn trong chat.
            options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
        }).AddJsonProtocol(options =>
        {
            // Đồng bộ chuẩn enum serialization giữa REST và SignalR payload.
            options.PayloadSerializerOptions.Converters.Add(
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

        if (!string.IsNullOrWhiteSpace(redisConnectionString))
        {
            // Chỉ bật Redis backplane khi có cấu hình để mở rộng realtime theo mô hình multi-instance.
            signalRBuilder.AddStackExchangeRedis(redisConnectionString);
        }
    }
}

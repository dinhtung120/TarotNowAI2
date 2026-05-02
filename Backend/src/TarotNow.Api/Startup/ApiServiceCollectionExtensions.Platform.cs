using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;
using TarotNow.Api.Constants;
using TarotNow.Api.Middlewares;
using TarotNow.Api.Options;
using TarotNow.Api.Realtime;
using TarotNow.Api.Services;
using TarotNow.Application;
using TarotNow.Application.Common.Interfaces;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure;
using TarotNow.Infrastructure.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;

namespace TarotNow.Api.Startup;

public static partial class ApiServiceCollectionExtensions
{
    /// <summary>
    /// Đăng ký các service nền tảng cần cho API runtime.
    /// Luồng xử lý: gắn exception handling, application/infrastructure, realtime services, authorization policies và SignalR.
    /// </summary>
    private static void AddPlatformServices(IServiceCollection services, IConfiguration configuration)
    {
        var environmentName = configuration["ASPNETCORE_ENVIRONMENT"]
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? string.Empty;
        var allowDegradedPresence = IsLocalPresenceFallbackAllowed(environmentName);

        // Gắn global exception handler để mọi lỗi chưa bắt được chuẩn hóa về ProblemDetails.
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddApplicationServices(Assembly.GetExecutingAssembly());
        services.AddInfrastructureServices(configuration);
        services.AddApiObservability(configuration);
        ConfigureForwardedHeaders(services, configuration);
        services.AddSingleton<IForwardedHeaderTrustEvaluator, ForwardedHeaderTrustEvaluator>();
        services.AddScoped<IAuthCookieService, AuthCookieService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAiStreamEndpointService, AiStreamEndpointService>();
        services.AddScoped<IAiStreamSseOrchestrator, AiStreamSseOrchestrator>();
        services.AddSingleton<IAiStreamTicketService, AiStreamTicketService>();
        services.AddSingleton<IUserPresenceTracker>(serviceProvider =>
        {
            var systemConfigSettings = serviceProvider.GetRequiredService<ISystemConfigSettings>();
            var cacheBackendState = serviceProvider.GetService<CacheBackendState>();
            if (cacheBackendState?.UsesRedis == true)
            {
                var multiplexer = serviceProvider.GetService<StackExchange.Redis.IConnectionMultiplexer>();
                if (multiplexer is not null)
                {
                    var logger = serviceProvider
                        .GetRequiredService<ILogger<RedisUserPresenceTracker>>();
                    return new RedisUserPresenceTracker(multiplexer, systemConfigSettings, logger);
                }
            }

            if (!allowDegradedPresence)
            {
                throw new InvalidOperationException(
                    $"Redis is required for presence consistency when ASPNETCORE_ENVIRONMENT='{environmentName}'.");
            }

            return new InMemoryUserPresenceTracker(systemConfigSettings);
        });
        services.AddHostedService<PresenceTimeoutBackgroundService>();
        services.AddSingleton<IRealtimeBridgeSource>(serviceProvider =>
        {
            var multiplexer = serviceProvider.GetService<StackExchange.Redis.IConnectionMultiplexer>();
            if (multiplexer is null)
            {
                if (!allowDegradedPresence)
                {
                    throw new InvalidOperationException(
                        $"Redis realtime bridge requires Redis when ASPNETCORE_ENVIRONMENT='{environmentName}'.");
                }

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
        ConfigureSignalR(services, configuration.GetConnectionString("Redis"), ResolveSignalROptions(configuration));
    }

    private static bool IsLocalPresenceFallbackAllowed(string environmentName)
    {
        return environmentName.Equals("Development", StringComparison.OrdinalIgnoreCase)
            || environmentName.Equals("Local", StringComparison.OrdinalIgnoreCase)
            || environmentName.Equals("Test", StringComparison.OrdinalIgnoreCase)
            || environmentName.Equals("Testing", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Cấu hình SignalR và tùy chọn backplane Redis khi có connection string.
    /// Luồng xử lý: tạo SignalR builder, cấu hình payload JSON enum, rồi bật Redis scale-out nếu được cấu hình.
    /// </summary>
    private static void ConfigureSignalR(
        IServiceCollection services,
        string? redisConnectionString,
        SignalRRuntimeOptions signalROptions)
    {
        var signalRBuilder = services.AddSignalR(options =>
        {
            // Giới hạn cao hơn mặc định để hỗ trợ payload media metadata lớn trong chat.
            options.MaximumReceiveMessageSize = signalROptions.MaximumReceiveMessageSizeBytes;
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

    private static SignalRRuntimeOptions ResolveSignalROptions(IConfiguration configuration)
    {
        var configured = configuration.GetSection("SignalR").Get<SignalRRuntimeOptions>() ?? new();
        var maxSize = configured.MaximumReceiveMessageSizeBytes <= 0
            ? 10 * 1024 * 1024
            : configured.MaximumReceiveMessageSizeBytes;
        return new SignalRRuntimeOptions
        {
            MaximumReceiveMessageSizeBytes = Math.Clamp(maxSize, 1024, 100L * 1024 * 1024)
        };
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.BackgroundJobs;
using TarotNow.Infrastructure.Messaging.Redis;
using TarotNow.Infrastructure.Options;
using TarotNow.Infrastructure.Services;

namespace TarotNow.Infrastructure;

public static partial class DependencyInjection
{
    /// <summary>
    /// Đăng ký lớp cache với ưu tiên Redis và fallback in-memory khi Redis không khả dụng.
    /// Luồng xử lý: luôn thêm memory cache, thử tạo multiplexer Redis, đăng ký backend phù hợp và service cache dùng chung.
    /// </summary>
    private static void AddRedisCaching(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        var redisConnectionString = configuration.GetConnectionString("Redis");
        var postgreSqlConnectionString = configuration.GetConnectionString("PostgreSQL");
        var redisInstanceName = configuration["Redis:InstanceName"]?.Trim();
        var redisRequirement = configuration.GetSection(RedisRequirementOptions.SectionName).Get<RedisRequirementOptions>()
            ?? new RedisRequirementOptions();
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;
        var requiresRedis = ShouldRequireRedis(configuration, redisRequirement, environmentName);

        if (!string.IsNullOrWhiteSpace(redisConnectionString) && string.IsNullOrWhiteSpace(redisInstanceName))
        {
            throw new InvalidOperationException("Missing required configuration Redis:InstanceName (env: REDIS__INSTANCENAME).");
        }

        string? bootstrapSettingsFailureType = null;
        string? redisInitializationFailureType = null;
        string? redisEndpointSummary = null;
        var redisBootstrap = !string.IsNullOrWhiteSpace(postgreSqlConnectionString)
            ? TryLoadRedisBootstrapSettings(postgreSqlConnectionString, out bootstrapSettingsFailureType)
            : null;
        var redisMultiplexer = !string.IsNullOrWhiteSpace(redisConnectionString)
            ? TryCreateRedisMultiplexer(redisConnectionString, redisBootstrap, out redisInitializationFailureType, out redisEndpointSummary)
            : null;

        if (requiresRedis && redisMultiplexer is null)
        {
            throw new InvalidOperationException(
                "Redis is required but Redis configuration is missing. Check Redis:RequireRedis and ConnectionStrings:Redis.");
        }

        var usesRedisCache = redisMultiplexer != null;

        if (redisMultiplexer != null)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString!;
                options.InstanceName = redisInstanceName!;
            });

            services.AddSingleton<IConnectionMultiplexer>(redisMultiplexer);
            // Nhánh chuẩn: dùng Redis phân tán để đảm bảo rate limit/quota nhất quán đa instance.
        }
        else
        {
            services.AddDistributedMemoryCache();
            // Fallback: vẫn chạy được nhưng nhất quán kém hơn trong môi trường nhiều node.
        }

        RegisterRealtimePublishers(services);

        services.AddSingleton(new CacheBackendState(
            usesRedisCache,
            bootstrapSettingsFailureType,
            redisInitializationFailureType,
            redisEndpointSummary));
        services.AddHostedService<CacheBackendStartupLogger>();
        services.AddScoped<ICacheService, RedisCacheService>();
    }

    private static bool ShouldRequireRedis(
        IConfiguration configuration,
        RedisRequirementOptions redisRequirement,
        string environmentName)
    {
        if (redisRequirement.RequireRedis.HasValue)
        {
            return redisRequirement.RequireRedis.Value;
        }

        if (configuration.GetValue<bool>("AuthSecurity:RequireRedisForRefreshConsistency"))
        {
            return true;
        }

        return !string.Equals(environmentName, "Development", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(environmentName, "Test", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(environmentName, "Testing", StringComparison.OrdinalIgnoreCase);
    }

    private static void RegisterRealtimePublishers(IServiceCollection services)
    {
        services.AddSingleton<IRedisPublisher>(serviceProvider =>
        {
            var multiplexer = serviceProvider.GetService<IConnectionMultiplexer>();
            if (multiplexer == null)
            {
                return new NoOpRedisPublisher();
            }

            var logger = serviceProvider.GetRequiredService<ILogger<RedisPublisher>>();
            return new RedisPublisher(multiplexer, logger);
        });

        services.AddSingleton<IChatRealtimeFastLanePublisher>(serviceProvider =>
        {
            var multiplexer = serviceProvider.GetService<IConnectionMultiplexer>();
            if (multiplexer == null)
            {
                return new NoOpChatRealtimeFastLanePublisher();
            }

            var redisPublisher = serviceProvider.GetRequiredService<IRedisPublisher>();
            var logger = serviceProvider.GetRequiredService<ILogger<RedisChatRealtimeFastLanePublisher>>();
            return new RedisChatRealtimeFastLanePublisher(redisPublisher, logger);
        });
    }

    /// <summary>
    /// Thử tạo Redis multiplexer với timeout ngắn để không chặn startup lâu.
    /// Luồng xử lý: parse connection options, connect, trả multiplexer khi connected; lỗi thì trả null.
    /// </summary>
    private static IConnectionMultiplexer? TryCreateRedisMultiplexer(
        string connectionString,
        RedisBootstrapSettings? bootstrapSettings,
        out string? failureType,
        out string? endpointSummary)
    {
        failureType = null;
        endpointSummary = ResolveRedisEndpointSummary(connectionString);
        try
        {
            var options = ConfigurationOptions.Parse(connectionString);
            var fallback = new SystemConfigOptions().Operational.Redis;

            options.AbortOnConnectFail = false;
            options.ConnectTimeout = bootstrapSettings?.ConnectTimeoutMs ?? fallback.ConnectTimeoutMs;
            options.SyncTimeout = bootstrapSettings?.SyncTimeoutMs ?? fallback.SyncTimeoutMs;
            options.ConnectRetry = bootstrapSettings?.ConnectRetry ?? fallback.ConnectRetry;

            var multiplexer = ConnectionMultiplexer.Connect(options);
            if (multiplexer.IsConnected)
            {
                return multiplexer;
            }

            multiplexer.Dispose();
            failureType = "RedisNotConnected";
            // Kết nối tạo được nhưng chưa connected thì dispose và fallback an toàn.
            return null;
        }
        catch (Exception ex)
        {
            failureType = ex.GetType().Name;
            return null;
        }
    }

    private static string ResolveRedisEndpointSummary(string connectionString)
    {
        try
        {
            var parsed = ConfigurationOptions.Parse(connectionString);
            return string.Join(",", parsed.EndPoints.Select(endpoint => endpoint.ToString()));
        }
        catch
        {
            return "unknown";
        }
    }

}

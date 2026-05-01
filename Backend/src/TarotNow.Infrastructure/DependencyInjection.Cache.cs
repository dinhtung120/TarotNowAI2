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
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;
        var requiresRedis = string.Equals(environmentName, "Production", StringComparison.OrdinalIgnoreCase);

        if (!string.IsNullOrWhiteSpace(redisConnectionString) && string.IsNullOrWhiteSpace(redisInstanceName))
        {
            throw new InvalidOperationException("Missing required configuration Redis:InstanceName (env: REDIS__INSTANCENAME).");
        }

        var redisBootstrap = !string.IsNullOrWhiteSpace(postgreSqlConnectionString)
            ? TryLoadRedisBootstrapSettings(postgreSqlConnectionString)
            : null;
        var redisMultiplexer = !string.IsNullOrWhiteSpace(redisConnectionString)
            ? TryCreateRedisMultiplexer(redisConnectionString, redisBootstrap)
            : null;

        if (requiresRedis && redisMultiplexer is null)
        {
            throw new InvalidOperationException(
                "Redis is required in Production for realtime consistency. Check ConnectionStrings:Redis and Redis availability.");
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

        services.AddSingleton(new CacheBackendState(usesRedisCache));
        services.AddHostedService<CacheBackendStartupLogger>();
        services.AddScoped<ICacheService, RedisCacheService>();
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
        RedisBootstrapSettings? bootstrapSettings)
    {
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
            // Kết nối tạo được nhưng chưa connected thì dispose và fallback an toàn.
            return null;
        }
        catch (Exception ex)
        {
            var endpointSummary = "unknown";
            try
            {
                var parsed = ConfigurationOptions.Parse(connectionString);
                endpointSummary = string.Join(",", parsed.EndPoints.Select(endpoint => endpoint.ToString()));
            }
            catch
            {
                // Ignore parse failures in fallback logger path.
            }

            Console.Error.WriteLine(
                $"[RedisBootstrap] Failed to initialize Redis multiplexer for endpoint(s) {endpointSummary}. " +
                $"Falling back to distributed memory cache. Reason={ex.GetType().Name}: {ex.Message}");
            return null;
        }
    }

}

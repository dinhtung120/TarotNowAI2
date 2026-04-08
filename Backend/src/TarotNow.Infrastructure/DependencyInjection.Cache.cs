using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.BackgroundJobs;
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
        var redisConnectionString = configuration.GetConnectionString("Redis") ?? "localhost:6379";

        var redisMultiplexer = TryCreateRedisMultiplexer(redisConnectionString);
        var usesRedisCache = redisMultiplexer != null;

        if (redisMultiplexer != null)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "TarotNow:";
            });

            services.AddSingleton<IConnectionMultiplexer>(redisMultiplexer);
            // Nhánh chuẩn: dùng Redis phân tán để đảm bảo rate limit/quota nhất quán đa instance.
        }
        else
        {
            services.AddDistributedMemoryCache();
            // Fallback: vẫn chạy được nhưng nhất quán kém hơn trong môi trường nhiều node.
        }

        services.AddSingleton(new CacheBackendState(usesRedisCache));
        services.AddHostedService<CacheBackendStartupLogger>();
        services.AddScoped<ICacheService, RedisCacheService>();
    }

    /// <summary>
    /// Thử tạo Redis multiplexer với timeout ngắn để không chặn startup lâu.
    /// Luồng xử lý: parse connection options, connect, trả multiplexer khi connected; lỗi thì trả null.
    /// </summary>
    private static IConnectionMultiplexer? TryCreateRedisMultiplexer(string connectionString)
    {
        try
        {
            var options = ConfigurationOptions.Parse(connectionString);
            options.AbortOnConnectFail = false;
            options.ConnectTimeout = 2000;
            options.SyncTimeout = 2000;
            options.ConnectRetry = 1;

            var multiplexer = ConnectionMultiplexer.Connect(options);
            if (multiplexer.IsConnected)
            {
                return multiplexer;
            }

            multiplexer.Dispose();
            // Kết nối tạo được nhưng chưa connected thì dispose và fallback an toàn.
            return null;
        }
        catch
        {
            // Mọi lỗi kết nối Redis đều fallback để tránh fail startup toàn hệ thống.
            return null;
        }
    }
}

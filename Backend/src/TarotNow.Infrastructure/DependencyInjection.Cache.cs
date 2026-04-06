using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.BackgroundJobs;
using TarotNow.Infrastructure.Services;

namespace TarotNow.Infrastructure;

public static partial class DependencyInjection
{
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
        }
        else
        {
            services.AddDistributedMemoryCache();
        }

        services.AddSingleton(new CacheBackendState(usesRedisCache));
        services.AddHostedService<CacheBackendStartupLogger>();
        services.AddScoped<ICacheService, RedisCacheService>();
    }

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
            return null;
        }
        catch
        {
            return null;
        }
    }
}

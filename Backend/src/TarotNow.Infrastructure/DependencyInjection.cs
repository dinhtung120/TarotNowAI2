using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TarotNow.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Đăng ký tất cả Infrastructure Services (DB, Redis, External APIs) vào DI container.
    /// Tách biệt phần cấu hình hạ tầng khỏi Application logic.
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // TODO: Đăng ký Entity Framework Core (PostgreSQL)
        // services.AddDbContext<ApplicationDbContext>(options => 
        //     options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));

        // TODO: Đăng ký MongoDB MongoClient
        // var mongoConnectionString = configuration.GetConnectionString("MongoDB");

        // TODO: Đăng ký Redis Cache
        // services.AddStackExchangeRedisCache(options => ...);

        // TODO: Đăng ký Authentication/JWT Services
        // services.AddAuthentication(...);
        
        return services;
    }
}

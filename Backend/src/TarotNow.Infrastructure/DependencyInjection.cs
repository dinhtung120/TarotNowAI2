using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure;

public static partial class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureOptions(services, configuration);
        AddPersistence(services, configuration);
        AddRepositories(services);
        AddExternalServices(services);
        AddRedisCaching(services, configuration);
        AddJwtAuthentication(services, configuration);
        return services;
    }

    private static void ConfigureOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<CorsOptions>(configuration.GetSection("Cors"));
        services.Configure<AiProviderOptions>(configuration.GetSection("AiProvider"));
        services.Configure<SystemConfigOptions>(configuration.GetSection("SystemConfig"));
        services.Configure<PaymentGatewayOptions>(configuration.GetSection("PaymentGateway"));
    }
}

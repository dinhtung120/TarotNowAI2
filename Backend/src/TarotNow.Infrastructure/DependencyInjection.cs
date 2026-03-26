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
        services.Configure<Argon2Options>(configuration.GetSection("Argon2"));
        services.Configure<SecurityOptions>(configuration.GetSection("Security"));
        services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));
        services.Configure<LegalSettingsOptions>(configuration.GetSection("LegalSettings"));
        services.Configure<DiagnosticsOptions>(configuration.GetSection("Diagnostics"));
    }
}

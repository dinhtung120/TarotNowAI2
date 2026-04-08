using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarotNow.Infrastructure.Options;
using TarotNow.Infrastructure.BackgroundJobs;

namespace TarotNow.Infrastructure;

public static partial class DependencyInjection
{
    /// <summary>
    /// Entry point đăng ký toàn bộ hạ tầng Infrastructure cho ứng dụng.
    /// Luồng xử lý: cấu hình options, persistence, repositories, external services, cache, auth và background jobs.
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureOptions(services, configuration);
        AddPersistence(services, configuration);
        AddRepositories(services);
        AddExternalServices(services);
        AddRedisCaching(services, configuration);
        AddJwtAuthentication(services, configuration);
        AddBackgroundJobs(services);
        return services;
    }

    /// <summary>
    /// Bind toàn bộ options hạ tầng từ cấu hình ứng dụng.
    /// Luồng xử lý: ánh xạ từng section cấu hình vào options class tương ứng để DI sử dụng typed options.
    /// </summary>
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
        services.Configure<ChatModerationOptions>(configuration.GetSection("ChatModeration"));
        services.Configure<LiveKitOptions>(configuration.GetSection("LiveKit"));
        services.Configure<CallV2Options>(configuration.GetSection("CallV2"));
    }

    /// <summary>
    /// Đăng ký các background jobs khởi tạo/snapshot chạy ở tầng infrastructure.
    /// Luồng xử lý: thêm hosted services cho leaderboard snapshot và các seed service khởi động.
    /// </summary>
    private static void AddBackgroundJobs(IServiceCollection services)
    {
        services.AddHostedService<LeaderboardSnapshotJob>();
        services.AddHostedService<GamificationSeedService>();
        services.AddHostedService<GachaSeedService>();
    }
}

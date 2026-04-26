using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarotNow.Application.Options;
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
        services.Configure<AuthSecurityOptions>(configuration.GetSection("AuthSecurity"));
        services.Configure<CorsOptions>(configuration.GetSection("Cors"));
        services.Configure<AiProviderOptions>(configuration.GetSection("AiProvider"));
        services.Configure<SystemConfigOptions>(configuration.GetSection("SystemConfig"));
        services.Configure<OutboxOptions>(configuration.GetSection("Outbox"));
        services.Configure<LeaderboardSnapshotOptions>(configuration.GetSection("LeaderboardSnapshot"));
        services.Configure<DepositOptions>(configuration.GetSection("Deposit"));
        services.AddOptions<PayOsOptions>()
            .Bind(configuration.GetSection("PayOS"))
            .Validate(options =>
            {
                return HasValidPayOsCredential(options.ClientId)
                       && HasValidPayOsCredential(options.ApiKey)
                       && HasValidPayOsCredential(options.ChecksumKey);
            }, "PayOS credentials are missing or still use placeholder values.")
            .ValidateOnStart();
        services.Configure<Argon2Options>(configuration.GetSection("Argon2"));
        services.Configure<SecurityOptions>(configuration.GetSection("Security"));
        services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));
        services.Configure<LegalSettingsOptions>(configuration.GetSection("LegalSettings"));
        services.Configure<DiagnosticsOptions>(configuration.GetSection("Diagnostics"));
        services.Configure<ChatModerationOptions>(configuration.GetSection("ChatModeration"));
        services.Configure<FileStorageOptions>(configuration.GetSection("FileStorage"));
        services.AddOptions<ObjectStorageOptions>()
            .Bind(configuration.GetSection("ObjectStorage"))
            .Validate(o =>
            {
                return string.Equals(o.Provider, "R2", StringComparison.OrdinalIgnoreCase)
                       && !string.IsNullOrWhiteSpace(o.R2.AccountId)
                       && !string.IsNullOrWhiteSpace(o.R2.AccessKeyId)
                       && !string.IsNullOrWhiteSpace(o.R2.SecretAccessKey)
                       && !string.IsNullOrWhiteSpace(o.R2.BucketName)
                       && !string.IsNullOrWhiteSpace(o.R2.PublicBaseUrl);
            }, "ObjectStorage R2 chưa đủ cấu hình bắt buộc (AccountId, AccessKeyId, SecretAccessKey, BucketName, PublicBaseUrl).")
            .ValidateOnStart();
        services.Configure<MediaCdnOptions>(configuration.GetSection("MediaCdn"));
    }

    private static bool HasValidPayOsCredential(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var normalized = value.Trim();
        return !normalized.Contains("REPLACE", StringComparison.OrdinalIgnoreCase)
               && !normalized.Contains("PLACEHOLDER", StringComparison.OrdinalIgnoreCase)
               && !normalized.Contains("CHANGEME", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Đăng ký các background jobs khởi tạo/snapshot chạy ở tầng infrastructure.
    /// Luồng xử lý: thêm hosted services cho leaderboard snapshot và các seed service khởi động.
    /// </summary>
    private static void AddBackgroundJobs(IServiceCollection services)
    {
        services.AddHostedService<LeaderboardSnapshotJob>();
        services.AddHostedService<GamificationSeedService>();
    }
}

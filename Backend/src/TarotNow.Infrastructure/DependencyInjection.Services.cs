using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using MailKit.Net.Smtp;
using TarotNow.Infrastructure.Options;
using TarotNow.Infrastructure.Security;
using TarotNow.Infrastructure.Services;
using TarotNow.Infrastructure.Services.Ai;
using TarotNow.Infrastructure.Services.Configuration;

namespace TarotNow.Infrastructure;

public static partial class DependencyInjection
{
    /// <summary>
    /// Đăng ký các service hạ tầng ngoài repository (security, mail, domain publisher, media, ai provider...).
    /// Luồng xử lý: bind interface -> implementation theo vòng đời phù hợp singleton/scoped/http client.
    /// </summary>
    private static void AddUserImageStorage(IServiceCollection services)
    {
        // ImageSharpAvifInitializer.EnsureConfigured(); // Tạm tắt do thiếu thư viện native trong Docker gây chậm/lỗi.
        services.AddSingleton<IVirusScanService, NoOpVirusScanService>();
        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ObjectStorageOptions>>().Value;
            return new ImageUploadConcurrencyGate(options.MaxConcurrentUploads);
        });
        services.AddScoped<LocalObjectStorageService>();
        services.AddScoped<R2ObjectStorageService>();
        services.AddScoped<IUserImagePipeline, UserImagePipeline>();
        services.AddScoped<IObjectStorageService>(sp =>
        {
            var opt = sp.GetRequiredService<IOptions<ObjectStorageOptions>>().Value;
            return string.Equals(opt.Provider, "R2", StringComparison.OrdinalIgnoreCase)
                ? sp.GetRequiredService<R2ObjectStorageService>()
                : sp.GetRequiredService<LocalObjectStorageService>();
        });
    }

    private static void AddExternalServices(IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, Argon2idPasswordHasher>();
        services.AddSingleton<ITokenService, JwtTokenService>();
        services.AddSingleton<IJwtTokenSettings, JwtTokenSettings>();
        services.AddSingleton<ILegalVersionSettings, LegalVersionSettings>();
        services.AddSingleton<ISystemConfigSettings, SystemConfigSettings>();

        services.AddScoped<ISmtpClient, SmtpClient>();
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<IDomainEventPublisher, MediatRDomainEventPublisher>();
        services.AddSingleton<IRngService, RngService>();
        services.AddSingleton<IPaymentGatewayService, HmacPaymentGatewayService>();
        services.AddScoped<IDiagnosticsService, DiagnosticsService>();
        services.AddScoped<IReadinessService, ReadinessService>();
        services.AddScoped<IMediaProcessorService, MediaProcessorService>();
        services.AddScoped<IEntitlementService, EntitlementService>();

        services.AddScoped<IFileStorageService, LocalFileStorageService>();
        services.AddScoped<IImageProcessingService, ImageSharpProcessingService>();
        AddUserImageStorage(services);

        services.AddHttpClient<IAiProvider, OpenAiProvider>();
        // Đăng ký typed HttpClient cho AI provider để quản lý retry/timeouts theo chuẩn HttpClientFactory.
    }
}

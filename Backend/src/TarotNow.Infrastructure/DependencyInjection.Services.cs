using Microsoft.Extensions.DependencyInjection;
using TarotNow.Application.Interfaces;
using MailKit.Net.Smtp;
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
        services.AddScoped<IMediaProcessorService, MediaProcessorService>();
        services.AddScoped<IEntitlementService, EntitlementService>();

        services.AddScoped<IFileStorageService, LocalFileStorageService>();
        services.AddScoped<IImageProcessingService, ImageSharpProcessingService>();
        // Dùng local storage + xử lý ảnh để phục vụ upload/media trong môi trường hiện tại.

        services.AddHttpClient<IAiProvider, OpenAiProvider>();
        // Đăng ký typed HttpClient cho AI provider để quản lý retry/timeouts theo chuẩn HttpClientFactory.
    }
}

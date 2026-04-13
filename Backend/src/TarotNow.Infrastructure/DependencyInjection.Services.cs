using MailKit.Net.Smtp;
using Microsoft.Extensions.DependencyInjection;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Security;
using TarotNow.Infrastructure.Services;
using TarotNow.Infrastructure.Services.Ai;
using TarotNow.Infrastructure.Services.Configuration;

namespace TarotNow.Infrastructure;

public static partial class DependencyInjection
{
    /// <summary>
    /// Đăng ký các service hạ tầng ngoài repository (security, mail, domain publisher, ai, media upload...).
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
        services.AddScoped<IReadinessService, ReadinessService>();
        services.AddScoped<IEntitlementService, EntitlementService>();

        // R2-only strict upload adapter (presign + delete + URL mapping).
        services.AddScoped<IR2UploadService, R2UploadService>();

        services.AddHttpClient<IAiProvider, OpenAiProvider>();
    }
}

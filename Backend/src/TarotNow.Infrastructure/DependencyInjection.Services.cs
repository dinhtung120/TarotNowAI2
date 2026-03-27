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

        services.AddHttpClient<IAiProvider, OpenAiProvider>();
    }
}

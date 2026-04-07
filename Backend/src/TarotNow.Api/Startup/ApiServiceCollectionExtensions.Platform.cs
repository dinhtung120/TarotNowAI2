using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;
using TarotNow.Api.Constants;
using TarotNow.Api.Middlewares;
using TarotNow.Api.Realtime;
using TarotNow.Api.Services;
using TarotNow.Application;
using TarotNow.Application.Common.Interfaces;
using TarotNow.Infrastructure;

namespace TarotNow.Api.Startup;

public static partial class ApiServiceCollectionExtensions
{
    private static void AddPlatformServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddApplicationServices(Assembly.GetExecutingAssembly());
        services.AddInfrastructureServices(configuration);
        services.AddApiObservability(configuration);
        services.AddScoped<IRefreshTokenCookieService, RefreshTokenCookieService>();
        services.AddSingleton<IUserPresenceTracker, InMemoryUserPresenceTracker>();
        services.AddHostedService<PresenceTimeoutBackgroundService>();
        services.AddScoped<Application.Interfaces.INotificationPushService, SignalRNotificationPushService>();
        services.AddScoped<Application.Interfaces.IWalletPushService, SignalRWalletPushService>();
        services.AddScoped<Application.Interfaces.IChatPushService, SignalRChatPushService>();
        services.AddAuthorization(options =>
        {
            options.AddPolicy(ApiAuthorizationPolicies.AuthenticatedUser, ApiAuthorizationPolicies.RequireAuthenticatedUser);
            options.AddPolicy(ApiAuthorizationPolicies.AdminOnly, ApiAuthorizationPolicies.RequireAdminOnly);
        });
        ConfigureSignalR(services, configuration.GetConnectionString("Redis"));
    }

    private static void ConfigureSignalR(IServiceCollection services, string? redisConnectionString)
    {
        var signalRBuilder = services.AddSignalR(options =>
        {
            options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
        }).AddJsonProtocol(options =>
        {
            options.PayloadSerializerOptions.Converters.Add(
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

        if (!string.IsNullOrWhiteSpace(redisConnectionString))
        {
            signalRBuilder.AddStackExchangeRedis(redisConnectionString);
        }
    }
}

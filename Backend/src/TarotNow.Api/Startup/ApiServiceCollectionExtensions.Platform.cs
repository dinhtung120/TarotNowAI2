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
    /// <summary>
    /// Đăng ký các service nền tảng cần cho API runtime.
    /// Luồng xử lý: gắn exception handling, application/infrastructure, realtime services, authorization policies và SignalR.
    /// </summary>
    private static void AddPlatformServices(IServiceCollection services, IConfiguration configuration)
    {
        // Gắn global exception handler để mọi lỗi chưa bắt được chuẩn hóa về ProblemDetails.
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
            // Định nghĩa policy tập trung để controller tái sử dụng và tránh lệch rule quyền.
            options.AddPolicy(ApiAuthorizationPolicies.AuthenticatedUser, ApiAuthorizationPolicies.RequireAuthenticatedUser);
            options.AddPolicy(ApiAuthorizationPolicies.AdminOnly, ApiAuthorizationPolicies.RequireAdminOnly);
        });
        ConfigureSignalR(services, configuration.GetConnectionString("Redis"));
    }

    /// <summary>
    /// Cấu hình SignalR và tùy chọn backplane Redis khi có connection string.
    /// Luồng xử lý: tạo SignalR builder, cấu hình payload JSON enum, rồi bật Redis scale-out nếu được cấu hình.
    /// </summary>
    private static void ConfigureSignalR(IServiceCollection services, string? redisConnectionString)
    {
        var signalRBuilder = services.AddSignalR(options =>
        {
            // Giới hạn cao hơn mặc định để hỗ trợ payload media metadata lớn trong chat.
            options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
        }).AddJsonProtocol(options =>
        {
            // Đồng bộ chuẩn enum serialization giữa REST và SignalR payload.
            options.PayloadSerializerOptions.Converters.Add(
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

        if (!string.IsNullOrWhiteSpace(redisConnectionString))
        {
            // Chỉ bật Redis backplane khi có cấu hình để mở rộng realtime theo mô hình multi-instance.
            signalRBuilder.AddStackExchangeRedis(redisConnectionString);
        }
    }
}

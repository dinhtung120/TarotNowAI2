

using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Realtime;

public class SignalRNotificationPushService : INotificationPushService
{
    private readonly IHubContext<PresenceHub> _hubContext;
    private readonly ILogger<SignalRNotificationPushService> _logger;

    public SignalRNotificationPushService(
        IHubContext<PresenceHub> hubContext,
        ILogger<SignalRNotificationPushService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

        public async Task PushNewNotificationAsync(
        NotificationCreateDto notification,
        CancellationToken cancellationToken = default)
    {
        var groupName = $"user:{notification.UserId}";

        try
        {
            await _hubContext.Clients.Group(groupName).SendAsync(
                "notification.new",
                new
                {
                    notification.TitleVi,
                    notification.TitleEn,
                    notification.BodyVi,
                    notification.BodyEn,
                    notification.Type,
                    CreatedAt = DateTime.UtcNow
                },
                cancellationToken);

            _logger.LogInformation(
                "[NotificationPush] Pushed '{Type}' notification to user {UserId}",
                notification.Type,
                notification.UserId);
        }
        catch (Exception ex)
        {
            
            _logger.LogError(ex,
                "[NotificationPush] Failed to push '{Type}' to user {UserId}",
                notification.Type,
                notification.UserId);
        }
    }

    public async Task SendEventAsync(string userId, string eventName, object payload, CancellationToken cancellationToken = default)
    {
        var groupName = $"user:{userId}";

        try
        {
            await _hubContext.Clients.Group(groupName).SendAsync(eventName, payload, cancellationToken);
            _logger.LogInformation(
                "[NotificationPush] Pushed custom event '{EventName}' to user {UserId}",
                eventName,
                userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "[NotificationPush] Failed to push custom event '{EventName}' to user {UserId}",
                eventName,
                userId);
        }
    }
}

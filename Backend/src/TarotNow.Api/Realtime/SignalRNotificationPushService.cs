using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Realtime;

// Push thông báo realtime tới nhóm kết nối của từng người dùng.
public class SignalRNotificationPushService : INotificationPushService
{
    private readonly IHubContext<PresenceHub> _hubContext;
    private readonly ILogger<SignalRNotificationPushService> _logger;

    /// <summary>
    /// Khởi tạo dịch vụ push notification qua SignalR.
    /// Luồng xử lý: dùng PresenceHub để phát sự kiện theo user group và logger để theo dõi delivery.
    /// </summary>
    public SignalRNotificationPushService(
        IHubContext<PresenceHub> hubContext,
        ILogger<SignalRNotificationPushService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    /// <summary>
    /// Gửi thông báo mới cho một người dùng theo group `user:{id}`.
    /// Luồng xử lý: dựng group name, gửi payload notification, log thành công hoặc log lỗi nếu thất bại.
    /// </summary>
    public async Task PushNewNotificationAsync(
        NotificationCreateDto notification,
        CancellationToken cancellationToken = default)
    {
        var groupName = $"user:{notification.UserId}";

        try
        {
            // Push payload tối thiểu client cần để render toast/in-app notification ngay lập tức.
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
            // Không ném lại exception để tránh làm đứt flow nghiệp vụ chính khi realtime tạm lỗi.
            _logger.LogError(ex,
                "[NotificationPush] Failed to push '{Type}' to user {UserId}",
                notification.Type,
                notification.UserId);
        }
    }

    /// <summary>
    /// Gửi sự kiện tùy biến tới một người dùng.
    /// Luồng xử lý: phát trực tiếp eventName/payload vào group user, sau đó ghi log theo kết quả.
    /// </summary>
    public async Task SendEventAsync(string userId, string eventName, object payload, CancellationToken cancellationToken = default)
    {
        var groupName = $"user:{userId}";

        try
        {
            // Dùng event tùy biến để tái sử dụng cho các luồng realtime không thuộc notification.new.
            await _hubContext.Clients.Group(groupName).SendAsync(eventName, payload, cancellationToken);
            _logger.LogInformation(
                "[NotificationPush] Pushed custom event '{EventName}' to user {UserId}",
                eventName,
                userId);
        }
        catch (Exception ex)
        {
            // Nhánh lỗi delivery: chỉ log để vận hành theo dõi, không phá vỡ transaction gọi đến service này.
            _logger.LogError(ex,
                "[NotificationPush] Failed to push custom event '{EventName}' to user {UserId}",
                eventName,
                userId);
        }
    }
}

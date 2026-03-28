/*
 * ===================================================================
 * FILE: SignalRNotificationPushService.cs
 * NAMESPACE: TarotNow.Api.Realtime
 * ===================================================================
 * MỤC ĐÍCH:
 *   Implement INotificationPushService bằng SignalR IHubContext.
 *   Khi backend tạo thông báo mới (lưu vào MongoDB), service này sẽ
 *   push event "notification.new" xuống đúng user qua PresenceHub.
 *
 * TẠI SAO ĐẶT Ở API LAYER THAY VÌ INFRASTRUCTURE?
 *   - Clean Architecture: Infrastructure KHÔNG ĐƯỢC phụ thuộc vào Api layer.
 *   - PresenceHub nằm ở Api layer → service cần truy cập IHubContext<PresenceHub>
 *     cũng phải ở Api layer.
 *   - Interface INotificationPushService vẫn ở Application layer
 *     → các handler trong Application vẫn gọi qua interface, không vi phạm.
 *
 * TẠI SAO DÙNG IHubContext THAY VÌ GỌI TRỰC TIẾP HUB?
 *   - IHubContext cho phép gửi message từ BẤT KỲ ĐÂU trong ứng dụng
 *     (không chỉ từ bên trong Hub class).
 *   - Thread-safe: IHubContext là singleton, an toàn khi gọi từ nhiều threads.
 * ===================================================================
 */

using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Realtime;

/// <summary>
/// Gửi thông báo real-time xuống client qua PresenceHub (SignalR WebSocket).
/// Sử dụng group "user:{userId}" để đảm bảo chỉ đúng user nhận được event.
/// </summary>
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

    /// <summary>
    /// Push event "notification.new" xuống group "user:{userId}".
    /// Client (FE) lắng nghe event này → invalidate React Query cache → UI cập nhật.
    ///
    /// Payload chứa thông tin cơ bản để FE có thể:
    ///   1. Hiển thị toast/preview ngay lập tức (nếu muốn sau này).
    ///   2. Invalidate cache và refetch danh sách thông báo từ API.
    /// </summary>
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
            /*
             * Lỗi push KHÔNG BAO GIỜ được block luồng chính (tạo notification).
             * Log error rồi tiếp tục — user vẫn thấy notification khi mở dropdown/refetch.
             * Đây là thiết kế "best effort" push: nếu WS bị đứt thì FE sẽ fallback
             * khi user mở dropdown (refetch lúc dropdown open).
             */
            _logger.LogError(ex,
                "[NotificationPush] Failed to push '{Type}' to user {UserId}",
                notification.Type,
                notification.UserId);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Extensions;
using TarotNow.Application.Common.Interfaces;

namespace TarotNow.Api.Hubs;

[Authorize]
// SignalR hub theo dõi trạng thái presence người dùng.
// Luồng chính: đánh dấu connected/disconnected, nhận heartbeat và broadcast thay đổi trạng thái.
public class PresenceHub : Hub
{
    private readonly IUserPresenceTracker _presenceTracker;
    private readonly ILogger<PresenceHub> _logger;

    /// <summary>
    /// Khởi tạo presence hub.
    /// </summary>
    /// <param name="presenceTracker">Tracker lưu trạng thái online/offline và heartbeat.</param>
    /// <param name="logger">Logger phục vụ giám sát kết nối presence.</param>
    public PresenceHub(IUserPresenceTracker presenceTracker, ILogger<PresenceHub> logger)
    {
        _presenceTracker = presenceTracker;
        _logger = logger;
    }

    /// <summary>
    /// Lấy user id từ claim của kết nối hiện tại.
    /// </summary>
    /// <returns>User id dạng chuỗi hoặc <c>null</c> nếu thiếu claim.</returns>
    private string? GetUserId() => Context.User.GetUserIdOrNull()?.ToString();

    /// <summary>
    /// Xử lý khi kết nối presence được thiết lập.
    /// Luồng xử lý: mark connected, join user-group, broadcast trạng thái online.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            // Ghi nhận kết nối mới để trạng thái online phản ánh đúng theo từng connection.
            _presenceTracker.MarkConnected(userId, Context.ConnectionId);

            // Join user-group để hỗ trợ push theo user khi cần.
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");

            // Broadcast trạng thái online cho client theo dõi presence realtime.
            await Clients.All.SendAsync("UserStatusChanged", userId, "online");
        }

        _logger.LogDebug(
            "[PresenceHub] User {UserId} connected. ConnectionId: {ConnectionId}",
            userId,
            Context.ConnectionId);

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Xử lý khi kết nối presence bị ngắt.
    /// Luồng xử lý: mark disconnected và rời user-group tương ứng.
    /// </summary>
    /// <param name="exception">Exception ngắt kết nối nếu có.</param>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            // Ghi nhận disconnect để tracker cập nhật trạng thái hiện diện chính xác.
            _presenceTracker.MarkDisconnected(userId, Context.ConnectionId);

            // Gỡ connection khỏi user-group sau khi disconnect.
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user:{userId}");
        }

        _logger.LogDebug(
            "[PresenceHub] User {UserId} disconnected. Reason: {Reason}",
            userId,
            exception?.Message ?? "normal");

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Nhận heartbeat định kỳ từ client để duy trì trạng thái online.
    /// </summary>
    /// <returns>Tác vụ hoàn tất ngay sau khi ghi nhận heartbeat.</returns>
    public Task Heartbeat()
    {
        var userId = GetUserId();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            // Cập nhật dấu mốc heartbeat để timeout service không đánh dấu offline sớm.
            _presenceTracker.RecordHeartbeat(userId);
            _logger.LogDebug("[PresenceHub] User {UserId} heartbeat received.", userId);
        }

        return Task.CompletedTask;
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Extensions;
using TarotNow.Application.Common.Interfaces;

namespace TarotNow.Api.Hubs;

/// <summary>
/// Hub riêng biệt chuyên quản lý trạng thái Online/Offline/Heartbeat của người dùng.
/// Việc tách rời khỏi ChatHub giúp theo dõi presence ngay cả khi user ở trang khác không phải Chat.
/// </summary>
[Authorize]
public class PresenceHub : Hub
{
    private readonly IUserPresenceTracker _presenceTracker;
    private readonly ILogger<PresenceHub> _logger;

    public PresenceHub(IUserPresenceTracker presenceTracker, ILogger<PresenceHub> logger)
    {
        _presenceTracker = presenceTracker;
        _logger = logger;
    }

    private string? GetUserId() => Context.User.GetUserIdOrNull()?.ToString();

    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            _presenceTracker.MarkConnected(userId, Context.ConnectionId);

            /*
             * Thêm connection vào group "user:{userId}" để cho phép
             * INotificationPushService gửi event tới đúng user.
             * Một user có thể có nhiều connections (nhiều tabs/devices)
             * → tất cả đều nhận được event khi push vào group.
             */
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
            
            // Broadcast event cho client biết user vừa online
            await Clients.All.SendAsync("UserStatusChanged", userId, "online");
        }

        _logger.LogDebug(
            "[PresenceHub] User {UserId} connected. ConnectionId: {ConnectionId}",
            userId,
            Context.ConnectionId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            _presenceTracker.MarkDisconnected(userId, Context.ConnectionId);

            /* Rời group khi disconnect để tránh push vào connection đã chết */
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user:{userId}");
        }

        _logger.LogDebug(
            "[PresenceHub] User {UserId} disconnected. Reason: {Reason}",
            userId,
            exception?.Message ?? "normal");

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Client gọi hàm này định kỳ (VD: mỗi 5 phút) để đánh dấu là mình vẫn đang mờ app/web
    /// </summary>
    public Task Heartbeat()
    {
        var userId = GetUserId();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            _presenceTracker.RecordHeartbeat(userId);
            _logger.LogDebug("[PresenceHub] User {UserId} heartbeat received.", userId);
        }

        return Task.CompletedTask;
    }
}

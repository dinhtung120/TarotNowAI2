using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Extensions;
using TarotNow.Application.Common.Interfaces;
using TarotNow.Application.Features.Presence.Commands.PublishUserStatusChanged;

namespace TarotNow.Api.Hubs;

[Authorize]
// SignalR hub theo dõi trạng thái presence người dùng.
// Luồng chính: đánh dấu connected/disconnected, nhận heartbeat và phát domain event trạng thái hiện diện.
public class PresenceHub : Hub
{
    private readonly IMediator _mediator;
    private readonly IUserPresenceTracker _presenceTracker;
    private readonly ILogger<PresenceHub> _logger;

    /// <summary>
    /// Khởi tạo presence hub.
    /// </summary>
    /// <param name="mediator">MediatR điều phối publish presence domain events.</param>
    /// <param name="presenceTracker">Tracker lưu trạng thái online/offline và heartbeat.</param>
    /// <param name="logger">Logger phục vụ giám sát kết nối presence.</param>
    public PresenceHub(
        IMediator mediator,
        IUserPresenceTracker presenceTracker,
        ILogger<PresenceHub> logger)
    {
        _mediator = mediator;
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
    /// Luồng xử lý: mark connected, join user-group, publish trạng thái online.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            _presenceTracker.MarkConnected(userId, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
            await PublishUserStatusChangedAsync(userId, "online");
        }

        _logger.LogDebug(
            "[PresenceHub] User {UserId} connected. ConnectionId: {ConnectionId}",
            userId,
            Context.ConnectionId);

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Xử lý khi kết nối presence bị ngắt.
    /// Luồng xử lý: mark disconnected, rời user-group và publish trạng thái offline.
    /// </summary>
    /// <param name="exception">Exception ngắt kết nối nếu có.</param>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            _presenceTracker.MarkDisconnected(userId, Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user:{userId}");
            await PublishUserStatusChangedAsync(userId, "offline");
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
            _presenceTracker.RecordHeartbeat(userId);
            _logger.LogDebug("[PresenceHub] User {UserId} heartbeat received.", userId);
        }

        return Task.CompletedTask;
    }

    private async Task PublishUserStatusChangedAsync(string userId, string status)
    {
        try
        {
            await _mediator.Send(new PublishUserStatusChangedCommand
            {
                UserId = userId,
                Status = status
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "[PresenceHub] Failed to publish user status changed. UserId={UserId}, Status={Status}",
                userId,
                status);
        }
    }
}

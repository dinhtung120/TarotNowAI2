using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Extensions;
using TarotNow.Api.Realtime;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Hubs;

[Authorize]
// SignalR hub cho realtime chat.
// Luồng chính: quản lý group theo user/conversation và xử lý gửi/đọc tin nhắn theo partial methods.
public partial class ChatHub : Hub
{
    private readonly IMediator _mediator;
    private readonly ILogger<ChatHub> _logger;
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo chat hub.
    /// </summary>
    /// <param name="mediator">MediatR điều phối nghiệp vụ chat.</param>
    /// <param name="logger">Logger phục vụ quan sát lỗi realtime.</param>
    /// <param name="redisPublisher">Publisher Redis để phát sự kiện realtime qua bridge.</param>
    public ChatHub(
        IMediator mediator,
        ILogger<ChatHub> logger,
        IRedisPublisher redisPublisher)
    {
        _mediator = mediator;
        _logger = logger;
        _redisPublisher = redisPublisher;
    }

    /// <summary>
    /// Lấy user id dạng chuỗi từ claim hiện tại của kết nối.
    /// </summary>
    /// <returns>User id dạng chuỗi hoặc <c>null</c> khi thiếu claim.</returns>
    private string? GetUserId() => Context.User.GetUserIdOrNull()?.ToString();

    /// <summary>
    /// Thử parse user id hiện tại thành Guid.
    /// </summary>
    /// <param name="userGuid">User id đầu ra nếu parse thành công.</param>
    /// <returns><c>true</c> nếu parse được Guid hợp lệ.</returns>
    private bool TryGetUserGuid(out Guid userGuid)
    {
        return Guid.TryParse(GetUserId(), out userGuid);
    }

    /// <summary>
    /// Tạo tên group SignalR theo conversation id.
    /// </summary>
    /// <param name="conversationId">Id hội thoại.</param>
    /// <returns>Tên group conversation.</returns>
    private static string ConversationGroup(string conversationId) => $"conversation:{conversationId}";

    /// <summary>
    /// Tạo tên group SignalR theo user id.
    /// </summary>
    /// <param name="userId">Id người dùng.</param>
    /// <returns>Tên group user.</returns>
    private static string UserGroup(string userId) => $"user:{userId}";

    /// <summary>
    /// Gửi thông báo lỗi realtime về caller hiện tại.
    /// </summary>
    /// <param name="message">Thông điệp lỗi cần hiển thị cho client.</param>
    /// <returns>Tác vụ gửi sự kiện lỗi.</returns>
    private async Task SendClientErrorAsync(string message)
    {
        try
        {
            await Clients.Caller.SendAsync("Error", message);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(
                ex,
                "[ChatHub] Failed to send error payload to caller. Message={Message}",
                message);
        }
    }
}

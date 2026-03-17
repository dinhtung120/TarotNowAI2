using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TarotNow.Application.Features.Chat.Commands.MarkMessagesRead;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Common;

namespace TarotNow.Api.Hubs;

/// <summary>
/// SignalR Hub cho Chat 1-1 (realtime messaging).
///
/// Transport: WebSocket (fallback: Server-Sent Events → Long Polling).
/// Auth: JWT query string — đã cấu hình trong DI (OnMessageReceived).
///
/// Flow:
/// 1. Client connect với ?access_token=xxx
/// 2. Client gọi JoinConversation(conversationId) để join SignalR group
/// 3. Client gọi SendMessage(...) → Hub persist + broadcast cho group
/// 4. Client gọi MarkRead(conversationId) → reset unread count
/// 5. Khi disconnect, tự leave tất cả groups
///
/// Mỗi conversation = 1 SignalR group (group name = conversationId).
/// Chỉ members trong group nhận tin nhắn broadcast.
/// </summary>
[Authorize]
public class ChatHub : Hub
{
    private readonly IMediator _mediator;

    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy UserId từ JWT claims — không tin client.
    /// </summary>
    private string? GetUserId() =>
        Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    // ======================================================================
    // CONNECTION LIFECYCLE
    // ======================================================================

    /// <summary>
    /// Khi client kết nối — log connection.
    /// Có thể mở rộng: track online status, notify readers.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        Console.WriteLine($"[ChatHub] User {userId} connected. ConnectionId: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Khi client ngắt kết nối — tự động leave tất cả groups.
    /// SignalR quản lý group membership theo connection — khi disconnect, tự remove.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();
        Console.WriteLine($"[ChatHub] User {userId} disconnected. Reason: {exception?.Message ?? "normal"}");
        await base.OnDisconnectedAsync(exception);
    }

    // ======================================================================
    // HUB METHODS — Client gọi qua invoke()
    // ======================================================================

    /// <summary>
    /// Join vào conversation group — bắt đầu nhận tin nhắn realtime.
    ///
    /// Client gọi: connection.invoke("JoinConversation", conversationId)
    /// </summary>
    public async Task JoinConversation(string conversationId)
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            await Clients.Caller.SendAsync("Error", "Unauthorized");
            return;
        }

        // Thêm connection vào group — group name = conversationId
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);

        // Thông báo cho group rằng user đã join
        await Clients.Group(conversationId).SendAsync("UserJoined", new
        {
            userId,
            conversationId,
            joinedAt = DateTime.UtcNow
        });

        Console.WriteLine($"[ChatHub] User {userId} joined conversation {conversationId}");
    }

    /// <summary>
    /// Rời conversation group — ngừng nhận tin nhắn.
    /// Client gọi trước khi navigate khỏi chat screen.
    /// </summary>
    public async Task LeaveConversation(string conversationId)
    {
        var userId = GetUserId();
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        Console.WriteLine($"[ChatHub] User {userId} left conversation {conversationId}");
    }

    /// <summary>
    /// Gửi tin nhắn — persist vào MongoDB + broadcast cho group.
    ///
    /// Client gọi: connection.invoke("SendMessage", conversationId, content, type)
    /// Group nhận: "ReceiveMessage" event với ChatMessageDto
    /// </summary>
    public async Task SendMessage(string conversationId, string content, string type = "text")
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            await Clients.Caller.SendAsync("Error", "Unauthorized");
            return;
        }

        try
        {
            // Persist qua MediatR handler — handler validate quyền + business rules
            var command = new SendMessageCommand
            {
                ConversationId = conversationId,
                SenderId = userGuid,
                Type = type,
                Content = content
            };

            var message = await _mediator.Send(command);

            // Broadcast cho tất cả members trong conversation group
            await Clients.Group(conversationId).SendAsync("ReceiveMessage", message);
        }
        catch (Exception ex)
        {
            // Gửi lỗi riêng cho caller — không broadcast
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
    }

    /// <summary>
    /// Đánh dấu tin nhắn đã đọc — reset unread count.
    ///
    /// Client gọi khi: mở conversation, hoặc khi đang ở conversation và nhận tin mới.
    /// </summary>
    public async Task MarkRead(string conversationId)
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            await Clients.Caller.SendAsync("Error", "Unauthorized");
            return;
        }

        try
        {
            var command = new MarkMessagesReadCommand
            {
                ConversationId = conversationId,
                ReaderId = userGuid
            };

            await _mediator.Send(command);

            // Thông báo cho group rằng user đã đọc (typing indicator / read receipt)
            await Clients.Group(conversationId).SendAsync("MessagesRead", new
            {
                userId,
                conversationId,
                readAt = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
    }
}

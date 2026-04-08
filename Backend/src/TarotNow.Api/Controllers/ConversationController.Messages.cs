using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Commands.QueueChatModeration;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Features.Chat.Queries.ListMessages;

namespace TarotNow.Api.Controllers;

public partial class ConversationController
{
    /// <summary>
    /// Lấy danh sách tin nhắn của một hội thoại theo cursor pagination.
    /// Luồng xử lý: xác thực user, gửi query list messages, trả kết quả cho client.
    /// </summary>
    /// <param name="id">Id hội thoại.</param>
    /// <param name="cursor">Con trỏ phân trang tùy chọn.</param>
    /// <param name="limit">Giới hạn số tin nhắn trả về.</param>
    /// <returns>Danh sách tin nhắn theo cursor.</returns>
    [HttpGet("{id}/messages")]
    [EnableRateLimiting("chat-standard")]
    public async Task<IActionResult> Messages(
        string id,
        [FromQuery] string? cursor = null,
        [FromQuery] int limit = 50)
    {
        if (TryGetUserId(out var userId) == false)
        {
            // Chặn truy vấn message khi không xác định được người yêu cầu.
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new ListMessagesQuery
        {
            ConversationId = id,
            RequesterId = userId,
            Cursor = cursor,
            Limit = limit
        });

        return Ok(result);
    }

    /// <summary>
    /// Gửi tin nhắn mới vào hội thoại.
    /// Luồng xử lý: xác thực sender, gửi command tạo message, broadcast realtime và xếp hàng moderation.
    /// </summary>
    /// <param name="id">Id hội thoại đích.</param>
    /// <param name="body">Payload tin nhắn gửi lên.</param>
    /// <returns>Tin nhắn vừa tạo.</returns>
    [HttpPost("{id}/messages")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> SendMessage(string id, [FromBody] ConversationSendMessageBody body)
    {
        if (TryGetUserId(out var userId) == false)
        {
            // Chặn gửi tin nhắn khi sender không hợp lệ để bảo toàn tính toàn vẹn dữ liệu.
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new SendMessageCommand
        {
            ConversationId = id,
            SenderId = userId,
            // Chuẩn hóa type về text khi client gửi rỗng để tránh nhánh nghiệp vụ không xác định.
            Type = string.IsNullOrWhiteSpace(body.Type) ? "text" : body.Type.Trim(),
            Content = body.Content,
            MediaPayload = body.MediaPayload
        });

        // Broadcast message mới để cập nhật UI realtime cho các participant.
        await TryBroadcastMessageCreatedAsync(id, result);
        // Broadcast conversation.updated để danh sách inbox đồng bộ preview/message state.
        await TryBroadcastConversationUpdatedAsync(id, "message_created");
        // Đưa moderation vào hàng đợi không chặn đường đi chính của gửi tin nhắn.
        await TryQueueModerationAsync(result);
        return Ok(result);
    }

    /// <summary>
    /// Thử broadcast sự kiện tạo message mới qua SignalR.
    /// </summary>
    /// <param name="conversationId">Id hội thoại.</param>
    /// <param name="message">Tin nhắn vừa tạo.</param>
    private async Task TryBroadcastMessageCreatedAsync(string conversationId, ChatMessageDto message)
    {
        if (string.IsNullOrWhiteSpace(conversationId))
        {
            // Edge case id rỗng: bỏ qua broadcast để tránh gửi event không định danh.
            return;
        }

        try
        {
            await ChatHubContext.Clients.Group(ConversationGroup(conversationId)).SendAsync("message.created", message);
        }
        catch
        {
            // Broadcast lỗi không được làm hỏng API gửi tin nhắn nên được nuốt có chủ đích.
        }
    }

    /// <summary>
    /// Thử đẩy message vào hàng đợi moderation bất đồng bộ.
    /// </summary>
    /// <param name="message">Tin nhắn cần moderation.</param>
    private async Task TryQueueModerationAsync(ChatMessageDto message)
    {
        try
        {
            // Đẩy payload moderation đầy đủ để worker có đủ ngữ cảnh kiểm duyệt.
            await Mediator.Send(new QueueChatModerationCommand
            {
                Payload = new ChatModerationPayload
                {
                    MessageId = message.Id,
                    ConversationId = message.ConversationId,
                    SenderId = message.SenderId,
                    Type = message.Type,
                    Content = message.Content,
                    CreatedAt = message.CreatedAt
                }
            });
        }
        catch
        {
            // Lỗi queue moderation không được chặn trải nghiệm gửi tin nhắn của người dùng.
        }
    }
}

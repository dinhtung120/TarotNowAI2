using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

/// <summary>
/// Command gửi tin nhắn trong conversation.
///
/// Handler sẽ:
/// 1. Validate quyền truy cập conversation (phải là member).
/// 2. Persist tin nhắn vào MongoDB chat_messages.
/// 3. Cập nhật conversations.last_message_at + unread_count.
/// 4. Trả về ChatMessageDto cho SignalR hub broadcast.
/// </summary>
public class SendMessageCommand : IRequest<ChatMessageDto>
{
    /// <summary>ObjectId conversation target.</summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>UUID người gửi.</summary>
    public Guid SenderId { get; set; }

    /// <summary>Loại tin nhắn — default: text.</summary>
    public string Type { get; set; } = "text";

    /// <summary>Nội dung tin nhắn.</summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>Payload thanh toán (nếu type = payment_offer).</summary>
    public PaymentPayloadDto? PaymentPayload { get; set; }
}

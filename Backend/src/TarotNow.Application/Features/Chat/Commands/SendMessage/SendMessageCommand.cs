

using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

// Command gửi tin nhắn trong conversation với nhiều loại payload.
public class SendMessageCommand : IRequest<ChatMessageDto>
{
    // Định danh conversation đích.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh người gửi.
    public Guid SenderId { get; set; }

    // Loại tin nhắn (text/image/voice/system/call...).
    public string Type { get; set; } = "text";

    // Nội dung chính của tin nhắn.
    public string Content { get; set; } = string.Empty;

    // Payload thanh toán khi dùng message loại payment.
    public PaymentPayloadDto? PaymentPayload { get; set; }

    // Payload media khi message là image/voice.
    public MediaPayloadDto? MediaPayload { get; set; }

    // Payload cuộc gọi cho message log/call signaling.
    public CallSessionDto? CallPayload { get; set; }
}

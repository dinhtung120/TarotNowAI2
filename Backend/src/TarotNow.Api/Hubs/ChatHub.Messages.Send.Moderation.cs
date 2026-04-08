using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Commands.QueueChatModeration;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    /// <summary>
    /// Thử đẩy payload message vào hàng đợi moderation.
    /// Luồng xử lý: gửi command queue moderation, ghi warning khi enqueue thất bại.
    /// </summary>
    /// <param name="message">Tin nhắn cần kiểm duyệt.</param>
    private async Task TryQueueModerationAsync(ChatMessageDto message)
    {
        try
        {
            // Đẩy đủ metadata để worker moderation đánh giá chính xác ngữ cảnh.
            await _mediator.Send(new QueueChatModerationCommand
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
        catch (Exception ex)
        {
            // Lỗi moderation queue không được chặn luồng gửi message thành công cho user.
            _logger.LogWarning(
                ex,
                "[ChatHub] Unable to queue moderation payload. MessageId={MessageId}, ConversationId={ConversationId}",
                message.Id,
                message.ConversationId);
        }
    }
}

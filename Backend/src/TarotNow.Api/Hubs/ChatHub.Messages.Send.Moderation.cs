using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Commands.QueueChatModeration;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    private async Task TryQueueModerationAsync(ChatMessageDto message)
    {
        try
        {
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
            _logger.LogWarning(
                ex,
                "[ChatHub] Unable to queue moderation payload. MessageId={MessageId}, ConversationId={ConversationId}",
                message.Id,
                message.ConversationId);
        }
    }
}

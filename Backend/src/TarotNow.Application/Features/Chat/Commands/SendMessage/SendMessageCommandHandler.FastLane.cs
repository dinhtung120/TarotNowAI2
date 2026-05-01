using TarotNow.Application.Common;
using TarotNow.Application.Common.Realtime;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandlerRequestedDomainEventHandler
{
    private async Task PublishFastLaneRealtimeAsync(
        ConversationDto conversation,
        ChatMessageDto message,
        CancellationToken cancellationToken)
    {
        await _chatRealtimeFastLanePublisher.PublishAsync(
            BuildFastLaneMessageEnvelope(conversation, message),
            cancellationToken);
        await _chatRealtimeFastLanePublisher.PublishAsync(
            BuildFastLaneConversationDeltaEnvelope(conversation, message),
            cancellationToken);
        await _chatRealtimeFastLanePublisher.PublishAsync(
            BuildFastLaneUnreadDeltaEnvelope(conversation, message),
            cancellationToken);
    }

    private static ChatRealtimeEnvelopeV2 BuildFastLaneMessageEnvelope(
        ConversationDto conversation,
        ChatMessageDto message)
    {
        return new ChatRealtimeEnvelopeV2
        {
            EventType = RealtimeEventNames.ChatMessageCreatedFast,
            ConversationId = conversation.Id,
            MessageId = message.Id,
            ClientMessageId = message.ClientMessageId,
            SenderId = message.SenderId,
            OccurredAtUtc = message.CreatedAt,
            Payload = new
            {
                message,
                userId = conversation.UserId,
                readerId = conversation.ReaderId
            }
        };
    }

    private static ChatRealtimeEnvelopeV2 BuildFastLaneConversationDeltaEnvelope(
        ConversationDto conversation,
        ChatMessageDto message)
    {
        return new ChatRealtimeEnvelopeV2
        {
            EventType = RealtimeEventNames.ConversationUpdatedDelta,
            ConversationId = conversation.Id,
            MessageId = message.Id,
            ClientMessageId = message.ClientMessageId,
            SenderId = message.SenderId,
            OccurredAtUtc = message.CreatedAt,
            Payload = new
            {
                conversationId = conversation.Id,
                userId = conversation.UserId,
                readerId = conversation.ReaderId,
                type = "message_created",
                status = conversation.Status,
                updatedAt = message.CreatedAt
            }
        };
    }

    private static ChatRealtimeEnvelopeV2 BuildFastLaneUnreadDeltaEnvelope(
        ConversationDto conversation,
        ChatMessageDto message)
    {
        var recipientUserId = string.Equals(message.SenderId, conversation.UserId, StringComparison.Ordinal)
            ? conversation.ReaderId
            : conversation.UserId;

        return new ChatRealtimeEnvelopeV2
        {
            EventType = RealtimeEventNames.ChatUnreadDelta,
            ConversationId = conversation.Id,
            MessageId = message.Id,
            ClientMessageId = message.ClientMessageId,
            SenderId = message.SenderId,
            OccurredAtUtc = message.CreatedAt,
            Payload = new
            {
                conversationId = conversation.Id,
                userId = conversation.UserId,
                readerId = conversation.ReaderId,
                recipientUserId,
                unreadDelta = 1,
                type = "message_created",
                status = conversation.Status,
                updatedAt = message.CreatedAt,
                lastMessagePreview = message.Content,
                lastMessageType = message.Type
            }
        };
    }
}

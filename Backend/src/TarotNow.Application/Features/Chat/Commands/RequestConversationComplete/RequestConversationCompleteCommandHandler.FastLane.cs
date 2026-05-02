using TarotNow.Application.Common;
using TarotNow.Application.Common.Realtime;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;

public partial class RequestConversationCompleteCommandHandlerRequestedDomainEventHandler
{
    private readonly record struct ConversationRealtimeEventContext(
        string ActorId,
        string EventType,
        DateTime OccurredAtUtc);

    private async Task PublishFastLaneConversationEventAsync(
        ConversationDto conversation,
        ConversationRealtimeEventContext realtimeContext,
        IReadOnlyCollection<ChatMessageDto> createdMessages,
        CancellationToken cancellationToken)
    {
        foreach (var message in createdMessages.OrderBy(m => m.CreatedAt))
        {
            await _chatRealtimeFastLanePublisher.PublishAsync(
                BuildMessageCreatedFastEnvelope(conversation, message),
                cancellationToken);
        }

        await _chatRealtimeFastLanePublisher.PublishAsync(
            BuildConversationDeltaEnvelope(
                conversation,
                realtimeContext.ActorId,
                realtimeContext.EventType,
                realtimeContext.OccurredAtUtc),
            cancellationToken);

        if (createdMessages.Count <= 0)
        {
            return;
        }

        var latestMessage = createdMessages
            .OrderByDescending(m => m.CreatedAt)
            .First();

        await _chatRealtimeFastLanePublisher.PublishAsync(
            BuildUnreadDeltaEnvelope(
                conversation,
                realtimeContext.ActorId,
                realtimeContext.OccurredAtUtc,
                createdMessages.Count,
                latestMessage),
            cancellationToken);
    }

    private static ChatRealtimeEnvelopeV2 BuildMessageCreatedFastEnvelope(
        ConversationDto conversation,
        ChatMessageDto message)
    {
        return new ChatRealtimeEnvelopeV2
        {
            EventType = RealtimeEventNames.ChatMessageCreatedFast,
            ConversationId = conversation.Id,
            MessageId = message.Id,
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

    private static ChatRealtimeEnvelopeV2 BuildConversationDeltaEnvelope(
        ConversationDto conversation,
        string actorId,
        string eventType,
        DateTime occurredAtUtc)
    {
        return new ChatRealtimeEnvelopeV2
        {
            EventType = RealtimeEventNames.ConversationUpdatedDelta,
            ConversationId = conversation.Id,
            SenderId = actorId,
            OccurredAtUtc = occurredAtUtc,
            Payload = new
            {
                conversationId = conversation.Id,
                userId = conversation.UserId,
                readerId = conversation.ReaderId,
                type = eventType,
                status = conversation.Status,
                updatedAt = occurredAtUtc,
                confirm = conversation.Confirm
            }
        };
    }

    private static ChatRealtimeEnvelopeV2 BuildUnreadDeltaEnvelope(
        ConversationDto conversation,
        string actorId,
        DateTime occurredAtUtc,
        int messageCount,
        ChatMessageDto latestMessage)
    {
        var recipientUserId = string.Equals(actorId, conversation.UserId, StringComparison.Ordinal)
            ? conversation.ReaderId
            : conversation.UserId;

        return new ChatRealtimeEnvelopeV2
        {
            EventType = RealtimeEventNames.ChatUnreadDelta,
            ConversationId = conversation.Id,
            MessageId = latestMessage.Id,
            SenderId = actorId,
            OccurredAtUtc = occurredAtUtc,
            Payload = new
            {
                conversationId = conversation.Id,
                userId = conversation.UserId,
                readerId = conversation.ReaderId,
                recipientUserId,
                unreadDelta = messageCount,
                type = "message_created",
                status = conversation.Status,
                updatedAt = occurredAtUtc,
                lastMessagePreview = latestMessage.Content,
                lastMessageType = latestMessage.Type
            }
        };
    }
}

using TarotNow.Application.Common;
using TarotNow.Application.Common.Realtime;

namespace TarotNow.Application.Features.Chat.Commands.MarkMessagesRead;

public partial class MarkMessagesReadCommandHandlerRequestedDomainEventHandler
{
    private async Task PublishFastLaneRealtimeAsync(
        ConversationDto conversation,
        string readerId,
        DateTime occurredAtUtc,
        CancellationToken cancellationToken)
    {
        await _chatRealtimeFastLanePublisher.PublishAsync(
            BuildChatMessageReadDeltaEnvelope(conversation, readerId, occurredAtUtc),
            cancellationToken);

        await _chatRealtimeFastLanePublisher.PublishAsync(
            BuildConversationUpdatedDeltaEnvelope(conversation, readerId, occurredAtUtc),
            cancellationToken);

        await _chatRealtimeFastLanePublisher.PublishAsync(
            BuildUnreadDeltaEnvelope(conversation, readerId, occurredAtUtc),
            cancellationToken);
    }

    private static ChatRealtimeEnvelopeV2 BuildChatMessageReadDeltaEnvelope(
        ConversationDto conversation,
        string readerId,
        DateTime occurredAtUtc)
    {
        return new ChatRealtimeEnvelopeV2
        {
            EventType = RealtimeEventNames.ChatMessageReadDelta,
            ConversationId = conversation.Id,
            SenderId = readerId,
            OccurredAtUtc = occurredAtUtc,
            Payload = new
            {
                conversationId = conversation.Id,
                userId = readerId,
                readAt = occurredAtUtc
            }
        };
    }

    private static ChatRealtimeEnvelopeV2 BuildConversationUpdatedDeltaEnvelope(
        ConversationDto conversation,
        string readerId,
        DateTime occurredAtUtc)
    {
        return new ChatRealtimeEnvelopeV2
        {
            EventType = RealtimeEventNames.ConversationUpdatedDelta,
            ConversationId = conversation.Id,
            SenderId = readerId,
            OccurredAtUtc = occurredAtUtc,
            Payload = new
            {
                conversationId = conversation.Id,
                userId = conversation.UserId,
                readerId = conversation.ReaderId,
                type = "message_read",
                updatedAt = occurredAtUtc
            }
        };
    }

    private static ChatRealtimeEnvelopeV2 BuildUnreadDeltaEnvelope(
        ConversationDto conversation,
        string readerId,
        DateTime occurredAtUtc)
    {
        return new ChatRealtimeEnvelopeV2
        {
            EventType = RealtimeEventNames.ChatUnreadDelta,
            ConversationId = conversation.Id,
            SenderId = readerId,
            OccurredAtUtc = occurredAtUtc,
            Payload = new
            {
                conversationId = conversation.Id,
                userId = conversation.UserId,
                readerId = conversation.ReaderId,
                clearForUserId = readerId,
                unreadDelta = 0,
                type = "message_read",
                updatedAt = occurredAtUtc
            }
        };
    }
}

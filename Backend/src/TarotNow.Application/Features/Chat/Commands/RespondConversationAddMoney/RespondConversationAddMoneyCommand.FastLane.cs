using TarotNow.Application.Common;
using TarotNow.Application.Common.Realtime;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;

public partial class RespondConversationAddMoneyCommandHandlerRequestedDomainEventHandler
{
    private async Task PublishFastLanePaymentAcceptAsync(
        ConversationDto conversation,
        string senderId,
        ChatMessageDto offer,
        string responseMessageId,
        CancellationToken cancellationToken)
    {
        var occurredAtUtc = DateTime.UtcNow;
        var message = BuildFastLanePaymentAcceptMessage(conversation, senderId, offer, responseMessageId, occurredAtUtc);

        await _chatRealtimeFastLanePublisher.PublishAsync(
            BuildFastLaneMessageCreatedEnvelope(conversation, senderId, responseMessageId, occurredAtUtc, message),
            cancellationToken);
        await _chatRealtimeFastLanePublisher.PublishAsync(
            BuildFastLaneConversationUpdatedEnvelope(conversation, senderId, responseMessageId, occurredAtUtc),
            cancellationToken);
        await _chatRealtimeFastLanePublisher.PublishAsync(
            BuildFastLaneUnreadDeltaEnvelope(conversation, senderId, responseMessageId, occurredAtUtc),
            cancellationToken);
    }

    private static ChatMessageDto BuildFastLanePaymentAcceptMessage(
        ConversationDto conversation,
        string senderId,
        ChatMessageDto offer,
        string responseMessageId,
        DateTime occurredAtUtc)
    {
        return new ChatMessageDto
        {
            Id = responseMessageId,
            ConversationId = conversation.Id,
            SenderId = senderId,
            Type = ChatMessageType.PaymentAccept,
            Content = BuildOfferResponseContent(offer.Id, offer.PaymentPayload?.ProposalId, note: null),
            IsRead = false,
            CreatedAt = occurredAtUtc
        };
    }

    private static ChatRealtimeEnvelopeV2 BuildFastLaneMessageCreatedEnvelope(
        ConversationDto conversation,
        string senderId,
        string responseMessageId,
        DateTime occurredAtUtc,
        ChatMessageDto message)
    {
        return new ChatRealtimeEnvelopeV2
        {
            EventType = RealtimeEventNames.ChatMessageCreatedFast,
            ConversationId = conversation.Id,
            MessageId = responseMessageId,
            SenderId = senderId,
            OccurredAtUtc = occurredAtUtc,
            Payload = new
            {
                message,
                userId = conversation.UserId,
                readerId = conversation.ReaderId
            }
        };
    }

    private static ChatRealtimeEnvelopeV2 BuildFastLaneConversationUpdatedEnvelope(
        ConversationDto conversation,
        string senderId,
        string responseMessageId,
        DateTime occurredAtUtc)
    {
        return new ChatRealtimeEnvelopeV2
        {
            EventType = RealtimeEventNames.ConversationUpdatedDelta,
            ConversationId = conversation.Id,
            MessageId = responseMessageId,
            SenderId = senderId,
            OccurredAtUtc = occurredAtUtc,
            Payload = new
            {
                conversationId = conversation.Id,
                userId = conversation.UserId,
                readerId = conversation.ReaderId,
                type = "add_money_responded",
                updatedAt = occurredAtUtc
            }
        };
    }

    private static ChatRealtimeEnvelopeV2 BuildFastLaneUnreadDeltaEnvelope(
        ConversationDto conversation,
        string senderId,
        string responseMessageId,
        DateTime occurredAtUtc)
    {
        return new ChatRealtimeEnvelopeV2
        {
            EventType = RealtimeEventNames.ChatUnreadDelta,
            ConversationId = conversation.Id,
            MessageId = responseMessageId,
            SenderId = senderId,
            OccurredAtUtc = occurredAtUtc,
            Payload = new
            {
                conversationId = conversation.Id,
                userId = conversation.UserId,
                readerId = conversation.ReaderId,
                recipientUserId = conversation.ReaderId,
                unreadDelta = 1,
                type = "add_money_responded",
                updatedAt = occurredAtUtc,
                lastMessagePreview = "Đã chấp nhận đề xuất cộng tiền",
                lastMessageType = ChatMessageType.PaymentAccept
            }
        };
    }
}

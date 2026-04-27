using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler đồng bộ conversation/message sau khi escrow settlement đã commit.
/// </summary>
public sealed class EscrowConversationSyncRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<EscrowConversationSyncRequestedDomainEvent>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatMessageRepository _chatMessageRepository;

    public EscrowConversationSyncRequestedDomainEventHandler(
        IConversationRepository conversationRepository,
        IChatMessageRepository chatMessageRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _conversationRepository = conversationRepository;
        _chatMessageRepository = chatMessageRepository;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        EscrowConversationSyncRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(domainEvent.ConversationId, cancellationToken);
        if (conversation == null || !CanTransition(conversation.Status, domainEvent.TargetStatus))
        {
            return;
        }

        conversation.Status = domainEvent.TargetStatus;
        conversation.OfferExpiresAt = null;
        if (conversation.Confirm != null && domainEvent.TargetStatus == ConversationStatus.Completed)
        {
            conversation.Confirm.AutoResolveAt = null;
        }

        conversation.LastMessageAt = domainEvent.ResolvedAtUtc;
        conversation.UpdatedAt = domainEvent.ResolvedAtUtc;
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);

        var message = new ChatMessageDto
        {
            ConversationId = domainEvent.ConversationId,
            SenderId = domainEvent.ActorId,
            Type = domainEvent.MessageType,
            Content = domainEvent.MessageContent,
            SystemEventKey = domainEvent.EventIdempotencyKey,
            IsRead = false,
            CreatedAt = domainEvent.ResolvedAtUtc
        };
        await _chatMessageRepository.AddAsync(message, cancellationToken);
    }

    private static bool CanTransition(string currentStatus, string targetStatus)
    {
        if (targetStatus == ConversationStatus.Expired)
        {
            return !ConversationStatus.IsTerminal(currentStatus)
                   && currentStatus != ConversationStatus.Disputed;
        }

        if (targetStatus == ConversationStatus.Completed)
        {
            return currentStatus is ConversationStatus.Ongoing
                or ConversationStatus.AwaitingAcceptance
                or ConversationStatus.Disputed
                or ConversationStatus.Completed;
        }

        return false;
    }
}

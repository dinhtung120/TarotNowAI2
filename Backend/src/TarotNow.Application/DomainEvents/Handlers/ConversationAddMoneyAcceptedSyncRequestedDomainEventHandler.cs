using Microsoft.Extensions.Logging;
using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

// Đồng bộ projection khi add-money offer được accept để tránh freeze orphan khi write message thất bại transient.
public sealed class ConversationAddMoneyAcceptedSyncRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ConversationAddMoneyAcceptedSyncRequestedDomainEvent>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly ILogger<ConversationAddMoneyAcceptedSyncRequestedDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo handler sync payment-accept message.
    /// Luồng xử lý: nhận repository conversation/message và publisher để materialize message + emit realtime events qua outbox.
    /// </summary>
    public ConversationAddMoneyAcceptedSyncRequestedDomainEventHandler(
        IConversationRepository conversationRepository,
        IChatMessageRepository chatMessageRepository,
        IDomainEventPublisher domainEventPublisher,
        ILogger<ConversationAddMoneyAcceptedSyncRequestedDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _conversationRepository = conversationRepository;
        _chatMessageRepository = chatMessageRepository;
        _domainEventPublisher = domainEventPublisher;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ConversationAddMoneyAcceptedSyncRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(domainEvent.ConversationId, cancellationToken);
        if (conversation == null)
        {
            _logger.LogWarning(
                "Add-money accept sync skipped because conversation not found. ConversationId={ConversationId}",
                domainEvent.ConversationId);
            return;
        }

        var acceptMessage = await EnsureAcceptMessageProjectionAsync(domainEvent, cancellationToken);
        await TryUpdateConversationProjectionAsync(conversation, acceptMessage, cancellationToken);
        await PublishRealtimeDomainEventsAsync(conversation, acceptMessage, cancellationToken);
    }

    private async Task<ChatMessageDto> EnsureAcceptMessageProjectionAsync(
        ConversationAddMoneyAcceptedSyncRequestedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var existing = await _chatMessageRepository.GetByIdAsync(domainEvent.ResponseMessageId, cancellationToken);
        if (existing != null)
        {
            return existing;
        }

        var acceptMessage = new ChatMessageDto
        {
            Id = domainEvent.ResponseMessageId,
            ConversationId = domainEvent.ConversationId,
            SenderId = domainEvent.SenderUserId,
            Type = ChatMessageType.PaymentAccept,
            Content = RespondConversationAddMoneyCommandHandler.BuildOfferResponseContent(
                domainEvent.OfferMessageId,
                domainEvent.ProposalId,
                note: null),
            SystemEventKey = domainEvent.EventIdempotencyKey,
            IsRead = false,
            CreatedAt = domainEvent.OccurredAtUtc
        };
        await _chatMessageRepository.AddAsync(acceptMessage, cancellationToken);
        return acceptMessage;
    }

    private async Task TryUpdateConversationProjectionAsync(
        ConversationDto conversation,
        ChatMessageDto acceptMessage,
        CancellationToken cancellationToken)
    {
        var shouldUpdateConversation = false;
        if (!conversation.LastMessageAt.HasValue || conversation.LastMessageAt.Value < acceptMessage.CreatedAt)
        {
            conversation.LastMessageAt = acceptMessage.CreatedAt;
            if (string.Equals(acceptMessage.SenderId, conversation.UserId, StringComparison.Ordinal))
            {
                conversation.UnreadCountReader += 1;
            }
            else if (string.Equals(acceptMessage.SenderId, conversation.ReaderId, StringComparison.Ordinal))
            {
                conversation.UnreadCountUser += 1;
            }

            shouldUpdateConversation = true;
        }

        if (!conversation.UpdatedAt.HasValue || conversation.UpdatedAt.Value < acceptMessage.CreatedAt)
        {
            conversation.UpdatedAt = acceptMessage.CreatedAt;
            shouldUpdateConversation = true;
        }

        if (shouldUpdateConversation)
        {
            await _conversationRepository.UpdateAsync(conversation, cancellationToken);
        }
    }

    private async Task PublishRealtimeDomainEventsAsync(
        ConversationDto conversation,
        ChatMessageDto acceptMessage,
        CancellationToken cancellationToken)
    {
        await _domainEventPublisher.PublishAsync(
            new ChatMessageCreatedDomainEvent
            {
                ConversationId = conversation.Id,
                MessageId = acceptMessage.Id,
                SenderId = acceptMessage.SenderId,
                MessageType = acceptMessage.Type,
                OccurredAtUtc = acceptMessage.CreatedAt
            },
            cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new UnreadCountChangedDomainEvent
            {
                ConversationId = conversation.Id,
                UserId = conversation.UserId,
                ReaderId = conversation.ReaderId,
                OccurredAtUtc = acceptMessage.CreatedAt
            },
            cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new ConversationUpdatedDomainEvent(conversation.Id, "add_money_responded", acceptMessage.CreatedAt),
            cancellationToken);
    }
}

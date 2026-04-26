using Microsoft.Extensions.Logging;
using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

// Đồng bộ projection conversation/message sau khi completion-timeout settlement đã ghi bền vững ở write model.
public sealed class CompletionTimeoutConversationSyncRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<CompletionTimeoutConversationSyncRequestedDomainEvent>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly ILogger<CompletionTimeoutConversationSyncRequestedDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo handler sync projection completion-timeout.
    /// Luồng xử lý: nhận repository conversation/message và idempotency service để xử lý retry an toàn.
    /// </summary>
    public CompletionTimeoutConversationSyncRequestedDomainEventHandler(
        IConversationRepository conversationRepository,
        IChatMessageRepository chatMessageRepository,
        ILogger<CompletionTimeoutConversationSyncRequestedDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _conversationRepository = conversationRepository;
        _chatMessageRepository = chatMessageRepository;
        _logger = logger;
    }

    /// <summary>
    /// Xử lý event sync completion-timeout.
    /// Luồng xử lý: kiểm tra conversation hợp lệ, cập nhật trạng thái completed và append system message idempotent.
    /// </summary>
    protected override async Task HandleDomainEventAsync(
        CompletionTimeoutConversationSyncRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(domainEvent.ConversationId, cancellationToken);
        if (conversation == null)
        {
            _logger.LogWarning(
                "Completion-timeout sync skipped because conversation not found. ConversationId={ConversationId}",
                domainEvent.ConversationId);
            return;
        }

        if (ShouldSkipBecauseAlreadyCompletedManually(conversation))
        {
            _logger.LogInformation(
                "Completion-timeout sync skipped because conversation already completed manually. ConversationId={ConversationId}",
                domainEvent.ConversationId);
            return;
        }

        if (CanTransitionToCompleted(conversation.Status) == false)
        {
            _logger.LogDebug(
                "Completion-timeout sync ignored due to terminal status. ConversationId={ConversationId}, Status={Status}",
                domainEvent.ConversationId,
                conversation.Status);
            return;
        }

        await UpdateConversationProjectionAsync(conversation, domainEvent, cancellationToken);
        await AddIdempotentSystemMessageAsync(domainEvent, cancellationToken);
    }

    private async Task UpdateConversationProjectionAsync(
        ConversationDto conversation,
        CompletionTimeoutConversationSyncRequestedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        conversation.Status = ConversationStatus.Completed;
        conversation.OfferExpiresAt = null;
        if (conversation.Confirm != null)
        {
            conversation.Confirm.AutoResolveAt = null;
        }

        conversation.LastMessageAt = domainEvent.ResolvedAtUtc;
        conversation.UpdatedAt = domainEvent.ResolvedAtUtc;
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);
    }

    private async Task AddIdempotentSystemMessageAsync(
        CompletionTimeoutConversationSyncRequestedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var systemMessage = new ChatMessageDto
        {
            ConversationId = domainEvent.ConversationId,
            SenderId = domainEvent.ActorId,
            Type = ChatMessageType.SystemRelease,
            Content = domainEvent.MessageContent,
            SystemEventKey = domainEvent.EventIdempotencyKey,
            IsRead = false,
            CreatedAt = domainEvent.ResolvedAtUtc
        };
        await _chatMessageRepository.AddAsync(systemMessage, cancellationToken);
    }

    private static bool CanTransitionToCompleted(string status)
    {
        return status is ConversationStatus.Ongoing
            or ConversationStatus.AwaitingAcceptance
            or ConversationStatus.Disputed
            or ConversationStatus.Completed;
    }

    private static bool ShouldSkipBecauseAlreadyCompletedManually(ConversationDto conversation)
    {
        return conversation.Status == ConversationStatus.Completed
               && conversation.Confirm?.UserAt != null
               && conversation.Confirm.ReaderAt != null;
    }
}

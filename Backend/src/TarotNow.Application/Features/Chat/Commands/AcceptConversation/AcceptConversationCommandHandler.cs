using MediatR;
using System;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Chat.Commands.AcceptConversation;

// Handler điều phối luồng accept conversation cho reader.
public partial class AcceptConversationCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<AcceptConversationCommandHandlerRequestedDomainEvent>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatFinanceRepository _financeRepository;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly ISystemConfigSettings _systemConfigSettings;
    private readonly IChatRealtimeFastLanePublisher _chatRealtimeFastLanePublisher;

    /// <summary>
    /// Khởi tạo handler accept conversation.
    /// Luồng xử lý: nhận repository conversation/finance/message và transaction coordinator để xử lý atomically.
    /// </summary>
    public AcceptConversationCommandHandlerRequestedDomainEventHandler(
        IConversationRepository conversationRepository,
        IChatFinanceRepository financeRepository,
        ITransactionCoordinator transactionCoordinator,
        IChatMessageRepository chatMessageRepository,
        IDomainEventPublisher domainEventPublisher,
        ISystemConfigSettings systemConfigSettings,
        IChatRealtimeFastLanePublisher chatRealtimeFastLanePublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _conversationRepository = conversationRepository;
        _financeRepository = financeRepository;
        _transactionCoordinator = transactionCoordinator;
        _chatMessageRepository = chatMessageRepository;
        _domainEventPublisher = domainEventPublisher;
        _systemConfigSettings = systemConfigSettings;
        _chatRealtimeFastLanePublisher = chatRealtimeFastLanePublisher;
    }

    /// <summary>
    /// Xử lý command accept conversation.
    /// Luồng xử lý: tải conversation hợp lệ, accept câu hỏi chính, cập nhật trạng thái conversation, thêm system message, publish domain event.
    /// </summary>
    public async Task<ConversationActionResult> Handle(
        AcceptConversationCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await LoadConversationForAcceptAsync(request, cancellationToken);
        var now = DateTime.UtcNow;

        await AcceptMainQuestionAsync(conversation, now, cancellationToken);

        // Đổi trạng thái conversation sang ongoing sau khi escrow item đã được accept thành công.
        conversation.Status = Domain.Enums.ConversationStatus.Ongoing;
        conversation.OfferExpiresAt = null;
        conversation.UpdatedAt = now;

        // Thêm system message để đồng bộ timeline chat.
        var systemMessageAt = await AddAcceptedSystemMessageAsync(conversation, now, cancellationToken);
        conversation.LastMessageAt = systemMessageAt;

        await _conversationRepository.UpdateAsync(conversation, cancellationToken);

        // Phát domain event để các luồng realtime/integration đồng bộ trạng thái mới.
        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(conversation.Id, "accepted", now),
            cancellationToken);
        await PublishFastLaneConversationAcceptedAsync(conversation, now, cancellationToken);

        return new ConversationActionResult { Status = conversation.Status };
    }

    protected override async Task HandleDomainEventAsync(
        AcceptConversationCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }

    private Task PublishFastLaneConversationAcceptedAsync(
        ConversationDto conversation,
        DateTime occurredAtUtc,
        CancellationToken cancellationToken)
    {
        return _chatRealtimeFastLanePublisher.PublishAsync(
            new ChatRealtimeEnvelopeV2
            {
                EventType = RealtimeEventNames.ConversationUpdatedDelta,
                ConversationId = conversation.Id,
                OccurredAtUtc = occurredAtUtc,
                Payload = new
                {
                    conversationId = conversation.Id,
                    userId = conversation.UserId,
                    readerId = conversation.ReaderId,
                    type = "accepted",
                    status = conversation.Status.ToString(),
                    updatedAt = occurredAtUtc
                }
            },
            cancellationToken);
    }
}

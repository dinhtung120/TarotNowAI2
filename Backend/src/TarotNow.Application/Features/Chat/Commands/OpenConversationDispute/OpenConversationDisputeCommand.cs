using MediatR;
using System;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Escrow.Commands.OpenDispute;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.OpenConversationDispute;

// Command mở tranh chấp cho conversation đang diễn ra.
public class OpenConversationDisputeCommand : IRequest<ConversationActionResult>
{
    // Định danh conversation cần mở tranh chấp.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh user thực hiện thao tác.
    public Guid UserId { get; set; }

    // Item tài chính mục tiêu (tùy chọn, null thì hệ thống tự chọn).
    public Guid? ItemId { get; set; }

    // Lý do mở tranh chấp.
    public string Reason { get; set; } = string.Empty;
}

// Handler mở tranh chấp ở tầng conversation và ủy quyền settle item cho OpenDisputeCommand.
public class OpenConversationDisputeCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<OpenConversationDisputeCommandHandlerRequestedDomainEvent>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatFinanceRepository _financeRepository;
    private readonly IMediator _mediator;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler open conversation dispute.
    /// Luồng xử lý: nhận repository conversation/finance, mediator và domain event publisher.
    /// </summary>
    public OpenConversationDisputeCommandHandlerRequestedDomainEventHandler(
        IConversationRepository conversationRepository,
        IChatFinanceRepository financeRepository,
        IMediator mediator,
        IDomainEventPublisher domainEventPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _conversationRepository = conversationRepository;
        _financeRepository = financeRepository;
        _mediator = mediator;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command mở tranh chấp conversation.
    /// Luồng xử lý: kiểm tra conversation + quyền + trạng thái, resolve item id, gửi OpenDisputeCommand, cập nhật conversation disputed.
    /// </summary>
    public async Task<ConversationActionResult> Handle(
        OpenConversationDisputeCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        var userId = request.UserId.ToString();
        if (conversation.UserId != userId && conversation.ReaderId != userId)
        {
            // Chặn user ngoài conversation mở tranh chấp.
            throw new BadRequestException("Bạn không thể mở tranh chấp cho cuộc trò chuyện này.");
        }

        if (conversation.Status != ConversationStatus.Ongoing && conversation.Status != ConversationStatus.Disputed)
        {
            // Chỉ cho phép mở dispute từ trạng thái ongoing/disputed.
            throw new BadRequestException($"Không thể mở tranh chấp ở trạng thái '{conversation.Status}'.");
        }

        // Nếu caller không chỉ định ItemId thì tự chọn item phù hợp gần nhất.
        var itemId = request.ItemId ?? await ResolveDefaultItemIdAsync(conversation, cancellationToken);

        await _mediator.Send(new OpenDisputeCommand
        {
            ItemId = itemId,
            UserId = request.UserId,
            Reason = request.Reason
        }, cancellationToken);

        // Cập nhật conversation sang trạng thái disputed sau khi mở dispute thành công.
        conversation.Status = ConversationStatus.Disputed;
        conversation.UpdatedAt = DateTime.UtcNow;
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);

        // Phát domain event để các luồng realtime/UI cập nhật trạng thái tranh chấp.
        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(conversation.Id, "disputed", conversation.UpdatedAt.Value),
            cancellationToken);

        return new ConversationActionResult { Status = conversation.Status };
    }

    /// <summary>
    /// Resolve item mặc định để mở tranh chấp khi request không truyền ItemId.
    /// Luồng xử lý: lấy finance session theo conversation, chọn item mới nhất đang Accepted/Disputed.
    /// </summary>
    private async Task<Guid> ResolveDefaultItemIdAsync(ConversationDto conversation, CancellationToken cancellationToken)
    {
        var session = await _financeRepository.GetSessionByConversationRefAsync(conversation.Id, cancellationToken)
            ?? throw new BadRequestException("Không tìm thấy phiên tài chính của cuộc trò chuyện.");

        var items = await _financeRepository.GetItemsBySessionIdAsync(session.Id, cancellationToken);
        var candidate = items
            .OrderByDescending(item => item.UpdatedAt ?? item.CreatedAt)
            .FirstOrDefault(item => item.Status is QuestionItemStatus.Accepted or QuestionItemStatus.Disputed);

        if (candidate == null)
        {
            // Edge case không còn item đủ điều kiện dispute trong phiên hiện tại.
            throw new BadRequestException("Không tìm thấy giao dịch đủ điều kiện để mở tranh chấp.");
        }

        return candidate.Id;
    }

    protected override async Task HandleDomainEventAsync(
        OpenConversationDisputeCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}

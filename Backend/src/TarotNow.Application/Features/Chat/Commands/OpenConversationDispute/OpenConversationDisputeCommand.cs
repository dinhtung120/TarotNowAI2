using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Escrow.Commands.OpenDispute;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.OpenConversationDispute;

public class OpenConversationDisputeCommand : IRequest<ConversationActionResult>
{
    public string ConversationId { get; set; } = string.Empty;

    public Guid UserId { get; set; }

    public Guid? ItemId { get; set; }

    public string Reason { get; set; } = string.Empty;
}

public class OpenConversationDisputeCommandHandler
    : IRequestHandler<OpenConversationDisputeCommand, ConversationActionResult>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatFinanceRepository _financeRepository;
    private readonly IMediator _mediator;
    private readonly IDomainEventPublisher _domainEventPublisher;

    public OpenConversationDisputeCommandHandler(
        IConversationRepository conversationRepository,
        IChatFinanceRepository financeRepository,
        IMediator mediator,
        IDomainEventPublisher domainEventPublisher)
    {
        _conversationRepository = conversationRepository;
        _financeRepository = financeRepository;
        _mediator = mediator;
        _domainEventPublisher = domainEventPublisher;
    }

    public async Task<ConversationActionResult> Handle(
        OpenConversationDisputeCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        var userId = request.UserId.ToString();
        if (conversation.UserId != userId && conversation.ReaderId != userId)
        {
            throw new BadRequestException("Bạn không thể mở tranh chấp cho cuộc trò chuyện này.");
        }

        if (conversation.Status != ConversationStatus.Ongoing && conversation.Status != ConversationStatus.Disputed)
        {
            throw new BadRequestException($"Không thể mở tranh chấp ở trạng thái '{conversation.Status}'.");
        }

        var itemId = request.ItemId ?? await ResolveDefaultItemIdAsync(conversation, cancellationToken);

        await _mediator.Send(new OpenDisputeCommand
        {
            ItemId = itemId,
            UserId = request.UserId,
            Reason = request.Reason
        }, cancellationToken);

        conversation.Status = ConversationStatus.Disputed;
        conversation.UpdatedAt = DateTime.UtcNow;
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(conversation.Id, "disputed", conversation.UpdatedAt.Value),
            cancellationToken);

        return new ConversationActionResult { Status = conversation.Status };
    }

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
            throw new BadRequestException("Không tìm thấy giao dịch đủ điều kiện để mở tranh chấp.");
        }

        return candidate.Id;
    }
}

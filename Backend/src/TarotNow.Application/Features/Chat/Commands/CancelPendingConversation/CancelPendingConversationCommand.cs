using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.CancelPendingConversation;

public class CancelPendingConversationCommand : IRequest<ConversationActionResult>
{
    public string ConversationId { get; set; } = string.Empty;

    public Guid RequesterId { get; set; }
}

public class CancelPendingConversationCommandHandler : IRequestHandler<CancelPendingConversationCommand, ConversationActionResult>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    public CancelPendingConversationCommandHandler(
        IConversationRepository conversationRepository,
        IDomainEventPublisher domainEventPublisher)
    {
        _conversationRepository = conversationRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    public async Task<ConversationActionResult> Handle(
        CancelPendingConversationCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        var requesterId = request.RequesterId.ToString();
        if (conversation.UserId != requesterId)
        {
            throw new BadRequestException("Bạn không có quyền hủy cuộc trò chuyện này.");
        }

        if (conversation.Status != ConversationStatus.Pending)
        {
            throw new BadRequestException("Chỉ có thể xóa cuộc trò chuyện ở trạng thái pending.");
        }

        conversation.Status = ConversationStatus.Cancelled;
        conversation.OfferExpiresAt = null;
        conversation.UpdatedAt = DateTime.UtcNow;
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(conversation.Id, "cancelled", conversation.UpdatedAt.Value),
            cancellationToken);

        return new ConversationActionResult
        {
            Status = conversation.Status
        };
    }
}

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.CancelPendingConversation;

// Command hủy conversation khi còn ở trạng thái pending.
public class CancelPendingConversationCommand : IRequest<ConversationActionResult>
{
    // Định danh conversation cần hủy.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh người gửi yêu cầu hủy.
    public Guid RequesterId { get; set; }
}

// Handler xử lý hủy conversation pending.
public class CancelPendingConversationCommandHandler : IRequestHandler<CancelPendingConversationCommand, ConversationActionResult>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler cancel pending conversation.
    /// Luồng xử lý: nhận conversation repository và domain event publisher để cập nhật trạng thái + phát event.
    /// </summary>
    public CancelPendingConversationCommandHandler(
        IConversationRepository conversationRepository,
        IDomainEventPublisher domainEventPublisher)
    {
        _conversationRepository = conversationRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command hủy conversation pending.
    /// Luồng xử lý: tải conversation, kiểm tra quyền requester và trạng thái pending, cập nhật cancelled, publish event.
    /// </summary>
    public async Task<ConversationActionResult> Handle(
        CancelPendingConversationCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        var requesterId = request.RequesterId.ToString();
        if (conversation.UserId != requesterId)
        {
            // Chỉ user chủ conversation mới được phép hủy.
            throw new BadRequestException("Bạn không có quyền hủy cuộc trò chuyện này.");
        }

        if (conversation.Status != ConversationStatus.Pending)
        {
            // Rule nghiệp vụ: chỉ hủy được khi conversation chưa được accept.
            throw new BadRequestException("Chỉ có thể xóa cuộc trò chuyện ở trạng thái pending.");
        }

        // Đổi trạng thái conversation và xóa offer expiry khi user hủy.
        conversation.Status = ConversationStatus.Cancelled;
        conversation.OfferExpiresAt = null;
        conversation.UpdatedAt = DateTime.UtcNow;
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);

        // Phát domain event để realtime/UI đồng bộ trạng thái cancelled.
        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(conversation.Id, "cancelled", conversation.UpdatedAt.Value),
            cancellationToken);

        return new ConversationActionResult
        {
            Status = conversation.Status
        };
    }
}

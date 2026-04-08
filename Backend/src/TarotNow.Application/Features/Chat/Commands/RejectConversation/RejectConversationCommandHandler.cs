using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.RejectConversation;

// Handler xử lý luồng reader từ chối conversation.
public partial class RejectConversationCommandHandler
    : IRequestHandler<RejectConversationCommand, ConversationActionResult>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatFinanceRepository _financeRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler reject conversation.
    /// Luồng xử lý: nhận repository tài chính/conversation/message và transaction coordinator để xử lý hoàn tiền nhất quán.
    /// </summary>
    public RejectConversationCommandHandler(
        IConversationRepository conversationRepository,
        IChatFinanceRepository financeRepository,
        IWalletRepository walletRepository,
        ITransactionCoordinator transactionCoordinator,
        IChatMessageRepository chatMessageRepository,
        IDomainEventPublisher domainEventPublisher)
    {
        _conversationRepository = conversationRepository;
        _financeRepository = financeRepository;
        _walletRepository = walletRepository;
        _transactionCoordinator = transactionCoordinator;
        _chatMessageRepository = chatMessageRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command reject conversation.
    /// Luồng xử lý: tải conversation hợp lệ, hoàn tiền nếu cần, cập nhật trạng thái cancelled, thêm system message và publish event.
    /// </summary>
    public async Task<ConversationActionResult> Handle(
        RejectConversationCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await LoadConversationForRejectAsync(request, cancellationToken);
        var now = DateTime.UtcNow;
        var refundedAmount = await RefundIfNeededAsync(conversation, now, cancellationToken);

        // Đổi trạng thái conversation về cancelled sau khi reject.
        conversation.Status = Domain.Enums.ConversationStatus.Cancelled;
        conversation.OfferExpiresAt = null;
        conversation.UpdatedAt = now;
        // Chỉ thêm system message khi thực sự có hoàn tiền.
        await TryAppendRejectSystemMessageAsync(conversation, request.Reason, refundedAmount, now, cancellationToken);
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);

        // Phát event để realtime và các integration biết conversation bị reject.
        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(conversation.Id, "rejected", now),
            cancellationToken);

        return new ConversationActionResult { Status = conversation.Status, Reason = request.Reason };
    }
}

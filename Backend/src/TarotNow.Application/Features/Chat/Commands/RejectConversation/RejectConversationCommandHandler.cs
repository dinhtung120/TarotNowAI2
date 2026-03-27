using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.RejectConversation;

public partial class RejectConversationCommandHandler
    : IRequestHandler<RejectConversationCommand, ConversationActionResult>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatFinanceRepository _financeRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

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

    public async Task<ConversationActionResult> Handle(
        RejectConversationCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await LoadConversationForRejectAsync(request, cancellationToken);
        var now = DateTime.UtcNow;
        var refundedAmount = await RefundIfNeededAsync(conversation, now, cancellationToken);

        conversation.Status = Domain.Enums.ConversationStatus.Cancelled;
        conversation.OfferExpiresAt = null;
        conversation.UpdatedAt = now;
        await TryAppendRejectSystemMessageAsync(conversation, request.Reason, refundedAmount, now, cancellationToken);
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(conversation.Id, "rejected", now),
            cancellationToken);

        return new ConversationActionResult { Status = conversation.Status, Reason = request.Reason };
    }
}

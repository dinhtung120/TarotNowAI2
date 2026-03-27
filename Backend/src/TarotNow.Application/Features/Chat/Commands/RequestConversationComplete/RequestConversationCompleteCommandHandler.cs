using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;

public partial class RequestConversationCompleteCommandHandler
    : IRequestHandler<RequestConversationCompleteCommand, ConversationActionResult>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatFinanceRepository _financeRepository;
    private readonly IEscrowSettlementService _escrowSettlementService;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    public RequestConversationCompleteCommandHandler(
        IConversationRepository conversationRepository,
        IChatFinanceRepository financeRepository,
        IEscrowSettlementService escrowSettlementService,
        ITransactionCoordinator transactionCoordinator,
        IChatMessageRepository chatMessageRepository,
        IDomainEventPublisher domainEventPublisher)
    {
        _conversationRepository = conversationRepository;
        _financeRepository = financeRepository;
        _escrowSettlementService = escrowSettlementService;
        _transactionCoordinator = transactionCoordinator;
        _chatMessageRepository = chatMessageRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    public async Task<ConversationActionResult> Handle(
        RequestConversationCompleteCommand request,
        CancellationToken cancellationToken)
    {
        var context = await BuildContextAsync(request, cancellationToken);
        if (IsAlreadyConfirmedByRequester(context))
        {
            return new ConversationActionResult { Status = context.Conversation.Status };
        }

        var lastMessageAt = await HandleFirstRequestIfNeededAsync(context, cancellationToken);
        ApplyRequesterConfirmation(context);

        if (HasBothSidesConfirmed(context.Conversation))
        {
            return await CompleteConversationAsync(context, cancellationToken);
        }

        await PersistPendingConversationAsync(context.Conversation, context.Now, lastMessageAt, cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(context.Conversation.Id, "complete_requested", context.Now),
            cancellationToken);

        return new ConversationActionResult { Status = context.Conversation.Status };
    }
}

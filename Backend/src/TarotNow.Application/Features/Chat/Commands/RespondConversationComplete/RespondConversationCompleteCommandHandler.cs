using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

public partial class RespondConversationCompleteCommandHandler
    : IRequestHandler<RespondConversationCompleteCommand, ConversationCompleteRespondResult>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatFinanceRepository _financeRepository;
    private readonly IEscrowSettlementService _escrowSettlementService;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    public RespondConversationCompleteCommandHandler(
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

    public async Task<ConversationCompleteRespondResult> Handle(
        RespondConversationCompleteCommand request,
        CancellationToken cancellationToken)
    {
        var context = await BuildContextAsync(request, cancellationToken);
        if (request.Accept == false)
        {
            return await RejectCompletionRequestAsync(context, cancellationToken);
        }

        ApplyResponderConfirmation(context);
        if (HasBothSidesConfirmed(context.Conversation))
        {
            return await CompleteConversationAsync(context, cancellationToken);
        }

        context.Conversation.UpdatedAt = context.Now;
        await _conversationRepository.UpdateAsync(context.Conversation, cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(context.Conversation.Id, "complete_responded", context.Now),
            cancellationToken);

        return new ConversationCompleteRespondResult { Status = context.Conversation.Status, Accepted = true };
    }
}

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.AcceptConversation;

public partial class AcceptConversationCommandHandler
    : IRequestHandler<AcceptConversationCommand, ConversationActionResult>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatFinanceRepository _financeRepository;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    public AcceptConversationCommandHandler(
        IConversationRepository conversationRepository,
        IChatFinanceRepository financeRepository,
        ITransactionCoordinator transactionCoordinator,
        IChatMessageRepository chatMessageRepository,
        IDomainEventPublisher domainEventPublisher)
    {
        _conversationRepository = conversationRepository;
        _financeRepository = financeRepository;
        _transactionCoordinator = transactionCoordinator;
        _chatMessageRepository = chatMessageRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    public async Task<ConversationActionResult> Handle(
        AcceptConversationCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await LoadConversationForAcceptAsync(request, cancellationToken);
        var now = DateTime.UtcNow;

        await AcceptMainQuestionAsync(conversation, now, cancellationToken);

        conversation.Status = Domain.Enums.ConversationStatus.Ongoing;
        conversation.OfferExpiresAt = null;
        conversation.UpdatedAt = now;

        var systemMessageAt = await AddAcceptedSystemMessageAsync(conversation, now, cancellationToken);
        conversation.LastMessageAt = systemMessageAt;

        await _conversationRepository.UpdateAsync(conversation, cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(conversation.Id, "accepted", now),
            cancellationToken);

        return new ConversationActionResult { Status = conversation.Status };
    }
}

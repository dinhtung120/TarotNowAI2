using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationAddMoney;

public partial class RequestConversationAddMoneyCommandHandler
    : IRequestHandler<RequestConversationAddMoneyCommand, ConversationAddMoneyRequestResult>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IMediator _mediator;
    private readonly IDomainEventPublisher _domainEventPublisher;

    public RequestConversationAddMoneyCommandHandler(
        IConversationRepository conversationRepository,
        IChatMessageRepository chatMessageRepository,
        IMediator mediator,
        IDomainEventPublisher domainEventPublisher)
    {
        _conversationRepository = conversationRepository;
        _chatMessageRepository = chatMessageRepository;
        _mediator = mediator;
        _domainEventPublisher = domainEventPublisher;
    }

    public async Task<ConversationAddMoneyRequestResult> Handle(
        RequestConversationAddMoneyCommand request,
        CancellationToken cancellationToken)
    {
        ValidateRequest(request);
        var conversation = await LoadConversationAsync(request, cancellationToken);

        if (await TrySendCancelledByCompleteMessageAsync(request, conversation, cancellationToken) is { } cancelledResult)
        {
            return cancelledResult;
        }

        await EnsureNoPendingOfferAsync(request.ConversationId, cancellationToken);
        var message = await SendOfferMessageAsync(request, cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(request.ConversationId, "add_money_requested", DateTime.UtcNow),
            cancellationToken);

        return new ConversationAddMoneyRequestResult { MessageId = message.Id };
    }
}

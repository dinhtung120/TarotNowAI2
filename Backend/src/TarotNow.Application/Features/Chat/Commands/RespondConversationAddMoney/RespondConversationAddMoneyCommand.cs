using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;

public class RespondConversationAddMoneyCommand : IRequest<ConversationAddMoneyRespondResult>
{
    public string ConversationId { get; set; } = string.Empty;

    public Guid UserId { get; set; }

    public bool Accept { get; set; }

    public string OfferMessageId { get; set; } = string.Empty;

    public string? RejectReason { get; set; }
}

public partial class RespondConversationAddMoneyCommandHandler
    : IRequestHandler<RespondConversationAddMoneyCommand, ConversationAddMoneyRespondResult>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IMediator _mediator;
    private readonly IDomainEventPublisher _domainEventPublisher;

    public RespondConversationAddMoneyCommandHandler(
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

    public async Task<ConversationAddMoneyRespondResult> Handle(
        RespondConversationAddMoneyCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await GetConversationAsync(request, cancellationToken);
        ValidateRespondPermission(conversation, request.UserId);
        var offer = await GetOfferMessageAsync(request, conversation, cancellationToken);
        await EnsureOfferNotHandledAsync(request.ConversationId, offer.Id, cancellationToken);

        if (request.Accept == false)
        {
            return await RejectOfferAsync(request, offer, cancellationToken);
        }

        var readerId = ValidateAndParseReaderId(conversation, request);
        var itemId = await FreezeOfferAsync(request, offer, readerId, cancellationToken);
        var acceptMessage = await SendAcceptMessageAsync(request, offer, cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(request.ConversationId, "add_money_responded", DateTime.UtcNow),
            cancellationToken);

        return new ConversationAddMoneyRespondResult
        {
            Accepted = true,
            ItemId = itemId,
            MessageId = acceptMessage.Id
        };
    }
}

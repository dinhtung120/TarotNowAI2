using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationAddMoney;

public sealed class RequestConversationAddMoneyCommandHandler : IRequestHandler<RequestConversationAddMoneyCommand, ConversationAddMoneyRequestResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public RequestConversationAddMoneyCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<ConversationAddMoneyRequestResult> Handle(RequestConversationAddMoneyCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new RequestConversationAddMoneyCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (ConversationAddMoneyRequestResult)domainEvent.Result;
    }
}

public sealed class RequestConversationAddMoneyCommandHandlerRequestedDomainEvent : IIdempotentDomainEvent
{
    public RequestConversationAddMoneyCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public string EventIdempotencyKey => CommandEventIdempotencyKey.Build(
        nameof(RequestConversationAddMoneyCommandHandlerRequestedDomainEvent),
        Command.IdempotencyKey);

    public RequestConversationAddMoneyCommandHandlerRequestedDomainEvent(RequestConversationAddMoneyCommand command)
    {
        Command = command;
    }
}

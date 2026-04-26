using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;

public sealed class RespondConversationAddMoneyCommandHandler : IRequestHandler<RespondConversationAddMoneyCommand, ConversationAddMoneyRespondResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public RespondConversationAddMoneyCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<ConversationAddMoneyRespondResult> Handle(RespondConversationAddMoneyCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new RespondConversationAddMoneyCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (ConversationAddMoneyRespondResult)domainEvent.Result;
    }
}

public sealed class RespondConversationAddMoneyCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public RespondConversationAddMoneyCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public RespondConversationAddMoneyCommandHandlerRequestedDomainEvent(RespondConversationAddMoneyCommand command)
    {
        Command = command;
    }
}

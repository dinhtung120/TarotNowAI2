using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.AcceptConversation;

public sealed class AcceptConversationCommandHandler : IRequestHandler<AcceptConversationCommand, ConversationActionResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public AcceptConversationCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<ConversationActionResult> Handle(AcceptConversationCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new AcceptConversationCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (ConversationActionResult)domainEvent.Result;
    }
}

public sealed class AcceptConversationCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public AcceptConversationCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public AcceptConversationCommandHandlerRequestedDomainEvent(AcceptConversationCommand command)
    {
        Command = command;
    }
}

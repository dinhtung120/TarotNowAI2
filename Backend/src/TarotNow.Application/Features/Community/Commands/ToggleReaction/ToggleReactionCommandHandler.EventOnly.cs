using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Community.Commands.ToggleReaction;

public sealed class ToggleReactionCommandHandler : IRequestHandler<ToggleReactionCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public ToggleReactionCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(ToggleReactionCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new ToggleReactionCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class ToggleReactionCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public ToggleReactionCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public ToggleReactionCommandHandlerRequestedDomainEvent(ToggleReactionCommand command)
    {
        Command = command;
    }
}

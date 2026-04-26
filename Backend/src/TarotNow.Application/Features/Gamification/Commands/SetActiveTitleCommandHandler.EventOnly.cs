using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Gamification.Commands;

public sealed class SetActiveTitleCommandHandler : IRequestHandler<SetActiveTitleCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public SetActiveTitleCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(SetActiveTitleCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new SetActiveTitleCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class SetActiveTitleCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public SetActiveTitleCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public SetActiveTitleCommandHandlerRequestedDomainEvent(SetActiveTitleCommand command)
    {
        Command = command;
    }
}

using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Gamification.Commands;

public sealed class GrantAllTitlesCommandHandler : IRequestHandler<GrantAllTitlesCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public GrantAllTitlesCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(GrantAllTitlesCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new GrantAllTitlesCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class GrantAllTitlesCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public GrantAllTitlesCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public GrantAllTitlesCommandHandlerRequestedDomainEvent(GrantAllTitlesCommand command)
    {
        Command = command;
    }
}

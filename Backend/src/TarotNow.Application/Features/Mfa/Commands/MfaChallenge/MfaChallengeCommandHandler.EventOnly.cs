using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Mfa.Commands.MfaChallenge;

public sealed class MfaChallengeCommandHandler : IRequestHandler<MfaChallengeCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public MfaChallengeCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(MfaChallengeCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new MfaChallengeCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class MfaChallengeCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public MfaChallengeCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public MfaChallengeCommandHandlerRequestedDomainEvent(MfaChallengeCommand command)
    {
        Command = command;
    }
}

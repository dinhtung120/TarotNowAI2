using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Mfa.Commands.MfaSetup;

public sealed class MfaSetupCommandHandler : IRequestHandler<MfaSetupCommand, MfaSetupResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public MfaSetupCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<MfaSetupResult> Handle(MfaSetupCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new MfaSetupCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (MfaSetupResult)domainEvent.Result;
    }
}

public sealed class MfaSetupCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public MfaSetupCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public MfaSetupCommandHandlerRequestedDomainEvent(MfaSetupCommand command)
    {
        Command = command;
    }
}

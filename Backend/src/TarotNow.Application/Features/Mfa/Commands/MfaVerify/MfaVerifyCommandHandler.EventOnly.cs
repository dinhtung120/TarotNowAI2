using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Mfa.Commands.MfaVerify;

public sealed class MfaVerifyCommandHandler : IRequestHandler<MfaVerifyCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public MfaVerifyCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(MfaVerifyCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new MfaVerifyCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class MfaVerifyCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public MfaVerifyCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public MfaVerifyCommandHandlerRequestedDomainEvent(MfaVerifyCommand command)
    {
        Command = command;
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Mfa.Commands.MfaChallenge;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class MfaChallengeCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<MfaChallengeCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<MfaChallengeCommand, bool> _executor;

    public MfaChallengeCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<MfaChallengeCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        MfaChallengeCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

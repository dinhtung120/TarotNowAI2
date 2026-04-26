using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Mfa.Commands.MfaSetup;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class MfaSetupCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<MfaSetupCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<MfaSetupCommand, MfaSetupResult> _executor;

    public MfaSetupCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<MfaSetupCommand, MfaSetupResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        MfaSetupCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Admin.Commands.ToggleUserLock;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class ToggleUserLockCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ToggleUserLockCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<ToggleUserLockCommand, bool> _executor;

    public ToggleUserLockCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<ToggleUserLockCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        ToggleUserLockCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

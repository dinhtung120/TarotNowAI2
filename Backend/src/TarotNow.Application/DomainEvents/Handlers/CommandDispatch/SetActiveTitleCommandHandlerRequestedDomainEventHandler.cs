using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Gamification.Commands;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class SetActiveTitleCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<SetActiveTitleCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<SetActiveTitleCommand, bool> _executor;

    public SetActiveTitleCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<SetActiveTitleCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        SetActiveTitleCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

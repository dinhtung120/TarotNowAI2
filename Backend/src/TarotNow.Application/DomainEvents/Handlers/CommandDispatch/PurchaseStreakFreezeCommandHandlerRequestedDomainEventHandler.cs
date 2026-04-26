using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.CheckIn.Commands.PurchaseFreeze;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class PurchaseStreakFreezeCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<PurchaseStreakFreezeCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<PurchaseStreakFreezeCommand, PurchaseStreakFreezeResult> _executor;

    public PurchaseStreakFreezeCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<PurchaseStreakFreezeCommand, PurchaseStreakFreezeResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        PurchaseStreakFreezeCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

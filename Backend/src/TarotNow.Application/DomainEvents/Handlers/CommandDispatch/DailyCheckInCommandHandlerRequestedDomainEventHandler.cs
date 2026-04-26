using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.CheckIn.Commands.DailyCheckIn;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class DailyCheckInCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<DailyCheckInCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<DailyCheckInCommand, DailyCheckInResult> _executor;

    public DailyCheckInCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<DailyCheckInCommand, DailyCheckInResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        DailyCheckInCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

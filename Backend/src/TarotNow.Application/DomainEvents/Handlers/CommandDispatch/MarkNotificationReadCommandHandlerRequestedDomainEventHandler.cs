using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Notification.Commands.MarkAsRead;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class MarkNotificationReadCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<MarkNotificationReadCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<MarkNotificationReadCommand, bool> _executor;

    public MarkNotificationReadCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<MarkNotificationReadCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        MarkNotificationReadCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

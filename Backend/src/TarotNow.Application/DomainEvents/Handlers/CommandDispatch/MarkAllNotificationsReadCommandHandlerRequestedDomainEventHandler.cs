using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Notification.Commands.MarkAllAsRead;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class MarkAllNotificationsReadCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<MarkAllNotificationsReadCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<MarkAllNotificationsReadCommand, bool> _executor;

    public MarkAllNotificationsReadCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<MarkAllNotificationsReadCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        MarkAllNotificationsReadCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

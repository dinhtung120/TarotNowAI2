using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Admin.Commands.UpdateUser;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class UpdateUserCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<UpdateUserCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<UpdateUserCommand, bool> _executor;

    public UpdateUserCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<UpdateUserCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        UpdateUserCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

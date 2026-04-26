using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Admin.Commands.CreateUser;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class CreateUserCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<CreateUserCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<CreateUserCommand, Guid> _executor;

    public CreateUserCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<CreateUserCommand, Guid> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        CreateUserCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

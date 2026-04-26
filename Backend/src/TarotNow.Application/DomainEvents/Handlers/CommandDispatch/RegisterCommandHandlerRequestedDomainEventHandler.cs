using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Auth.Commands.Register;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class RegisterCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<RegisterCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<RegisterCommand, Guid> _executor;

    public RegisterCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<RegisterCommand, Guid> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        RegisterCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

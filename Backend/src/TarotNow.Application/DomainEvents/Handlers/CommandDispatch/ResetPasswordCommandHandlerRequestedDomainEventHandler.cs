using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Auth.Commands.ResetPassword;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class ResetPasswordCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ResetPasswordCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<ResetPasswordCommand, bool> _executor;

    public ResetPasswordCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<ResetPasswordCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        ResetPasswordCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

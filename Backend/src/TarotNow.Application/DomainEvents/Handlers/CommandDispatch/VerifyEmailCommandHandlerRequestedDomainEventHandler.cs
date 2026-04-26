using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Auth.Commands.VerifyEmail;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class VerifyEmailCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<VerifyEmailCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<VerifyEmailCommand, bool> _executor;

    public VerifyEmailCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<VerifyEmailCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        VerifyEmailCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

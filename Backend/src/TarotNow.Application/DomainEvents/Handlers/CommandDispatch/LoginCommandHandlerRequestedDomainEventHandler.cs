using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Auth.Commands.Login;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class LoginCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<LoginCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<LoginCommand, LoginResult> _executor;

    public LoginCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<LoginCommand, LoginResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        LoginCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

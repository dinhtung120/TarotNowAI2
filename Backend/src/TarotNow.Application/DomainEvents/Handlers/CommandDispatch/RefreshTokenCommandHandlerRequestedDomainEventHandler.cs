using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Auth.Commands.RefreshToken;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class RefreshTokenCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<RefreshTokenCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<RefreshTokenCommand, RefreshTokenResult> _executor;

    public RefreshTokenCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<RefreshTokenCommand, RefreshTokenResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        RefreshTokenCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

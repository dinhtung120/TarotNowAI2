using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Mfa.Commands.MfaVerify;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class MfaVerifyCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<MfaVerifyCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<MfaVerifyCommand, bool> _executor;

    public MfaVerifyCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<MfaVerifyCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        MfaVerifyCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

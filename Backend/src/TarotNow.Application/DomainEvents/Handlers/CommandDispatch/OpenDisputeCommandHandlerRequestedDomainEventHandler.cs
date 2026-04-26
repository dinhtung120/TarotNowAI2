using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Escrow.Commands.OpenDispute;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class OpenDisputeCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<OpenDisputeCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<OpenDisputeCommand, bool> _executor;

    public OpenDisputeCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<OpenDisputeCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        OpenDisputeCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

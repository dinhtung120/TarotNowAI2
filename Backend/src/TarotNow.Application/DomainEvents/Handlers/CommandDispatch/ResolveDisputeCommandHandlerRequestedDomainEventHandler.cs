using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Admin.Commands.ResolveDispute;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class ResolveDisputeCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ResolveDisputeCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<ResolveDisputeCommand, bool> _executor;

    public ResolveDisputeCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<ResolveDisputeCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        ResolveDisputeCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

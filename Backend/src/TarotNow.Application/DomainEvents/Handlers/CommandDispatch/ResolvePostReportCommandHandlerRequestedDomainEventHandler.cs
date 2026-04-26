using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Community.Commands.ResolvePostReport;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class ResolvePostReportCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ResolvePostReportCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<ResolvePostReportCommand, bool> _executor;

    public ResolvePostReportCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<ResolvePostReportCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        ResolvePostReportCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

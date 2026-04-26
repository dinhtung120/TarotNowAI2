using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Community.Commands.ReportPost;
using TarotNow.Application.Common;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class ReportPostCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ReportPostCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<ReportPostCommand, ReportDto> _executor;

    public ReportPostCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<ReportPostCommand, ReportDto> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        ReportPostCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

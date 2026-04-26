using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Chat.Commands.CreateReport;
using TarotNow.Application.Common;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class CreateReportCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<CreateReportCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<CreateReportCommand, ReportDto> _executor;

    public CreateReportCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<CreateReportCommand, ReportDto> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        CreateReportCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

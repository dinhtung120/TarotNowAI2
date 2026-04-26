using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class CompleteAiStreamCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<CompleteAiStreamCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<CompleteAiStreamCommand, bool> _executor;

    public CompleteAiStreamCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<CompleteAiStreamCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        CompleteAiStreamCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

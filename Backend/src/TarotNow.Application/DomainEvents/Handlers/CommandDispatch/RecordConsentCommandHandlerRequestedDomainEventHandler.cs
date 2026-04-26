using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Legal.Commands.RecordConsent;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class RecordConsentCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<RecordConsentCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<RecordConsentCommand, bool> _executor;

    public RecordConsentCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<RecordConsentCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        RecordConsentCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

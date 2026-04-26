using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Gamification.Commands;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class AdminTitleCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<AdminTitleCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<UpsertTitleDefinitionCommand, bool> _executor;

    public AdminTitleCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<UpsertTitleDefinitionCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        AdminTitleCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

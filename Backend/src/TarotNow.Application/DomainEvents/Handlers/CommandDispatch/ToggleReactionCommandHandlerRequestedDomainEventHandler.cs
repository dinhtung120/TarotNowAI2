using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Community.Commands.ToggleReaction;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class ToggleReactionCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ToggleReactionCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<ToggleReactionCommand, bool> _executor;

    public ToggleReactionCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<ToggleReactionCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        ToggleReactionCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

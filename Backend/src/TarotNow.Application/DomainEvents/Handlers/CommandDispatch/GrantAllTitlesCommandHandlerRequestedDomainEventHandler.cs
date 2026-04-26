using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Gamification.Commands;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class GrantAllTitlesCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<GrantAllTitlesCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<GrantAllTitlesCommand, bool> _executor;

    public GrantAllTitlesCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<GrantAllTitlesCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        GrantAllTitlesCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

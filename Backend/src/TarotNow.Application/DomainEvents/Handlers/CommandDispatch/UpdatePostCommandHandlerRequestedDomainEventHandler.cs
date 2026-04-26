using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Community.Commands.UpdatePost;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class UpdatePostCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<UpdatePostCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<UpdatePostCommand, bool> _executor;

    public UpdatePostCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<UpdatePostCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        UpdatePostCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Chat.Commands.PublishTypingState;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class PublishTypingStateCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<PublishTypingStateCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<PublishTypingStateCommand, bool> _executor;

    public PublishTypingStateCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<PublishTypingStateCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        PublishTypingStateCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

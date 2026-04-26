using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Chat.Commands.MarkMessagesRead;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class MarkMessagesReadCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<MarkMessagesReadCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<MarkMessagesReadCommand, bool> _executor;

    public MarkMessagesReadCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<MarkMessagesReadCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        MarkMessagesReadCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

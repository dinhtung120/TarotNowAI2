using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Chat.Commands.CancelPendingConversation;
using TarotNow.Application.Common;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class CancelPendingConversationCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<CancelPendingConversationCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<CancelPendingConversationCommand, ConversationActionResult> _executor;

    public CancelPendingConversationCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<CancelPendingConversationCommand, ConversationActionResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        CancelPendingConversationCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;
using TarotNow.Application.Common;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class RequestConversationCompleteCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<RequestConversationCompleteCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<RequestConversationCompleteCommand, ConversationActionResult> _executor;

    public RequestConversationCompleteCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<RequestConversationCompleteCommand, ConversationActionResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        RequestConversationCompleteCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

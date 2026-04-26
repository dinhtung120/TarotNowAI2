using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Chat.Commands.AcceptConversation;
using TarotNow.Application.Common;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class AcceptConversationCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<AcceptConversationCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<AcceptConversationCommand, ConversationActionResult> _executor;

    public AcceptConversationCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<AcceptConversationCommand, ConversationActionResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        AcceptConversationCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

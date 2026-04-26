using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Chat.Commands.RejectConversation;
using TarotNow.Application.Common;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class RejectConversationCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<RejectConversationCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<RejectConversationCommand, ConversationActionResult> _executor;

    public RejectConversationCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<RejectConversationCommand, ConversationActionResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        RejectConversationCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

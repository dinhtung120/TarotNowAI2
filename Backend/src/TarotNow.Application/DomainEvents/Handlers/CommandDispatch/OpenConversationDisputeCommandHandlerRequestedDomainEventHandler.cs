using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Chat.Commands.OpenConversationDispute;
using TarotNow.Application.Common;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class OpenConversationDisputeCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<OpenConversationDisputeCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<OpenConversationDisputeCommand, ConversationActionResult> _executor;

    public OpenConversationDisputeCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<OpenConversationDisputeCommand, ConversationActionResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        OpenConversationDisputeCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

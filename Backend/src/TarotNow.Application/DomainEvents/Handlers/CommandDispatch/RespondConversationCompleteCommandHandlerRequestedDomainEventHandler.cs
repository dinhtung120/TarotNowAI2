using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;
using TarotNow.Application.Common;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class RespondConversationCompleteCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<RespondConversationCompleteCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<RespondConversationCompleteCommand, ConversationCompleteRespondResult> _executor;

    public RespondConversationCompleteCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<RespondConversationCompleteCommand, ConversationCompleteRespondResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        RespondConversationCompleteCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

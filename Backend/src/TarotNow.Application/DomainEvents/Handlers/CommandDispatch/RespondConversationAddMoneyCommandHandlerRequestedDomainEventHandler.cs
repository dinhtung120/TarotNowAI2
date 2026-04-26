using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;
using TarotNow.Application.Common;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class RespondConversationAddMoneyCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<RespondConversationAddMoneyCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<RespondConversationAddMoneyCommand, ConversationAddMoneyRespondResult> _executor;

    public RespondConversationAddMoneyCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<RespondConversationAddMoneyCommand, ConversationAddMoneyRespondResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        RespondConversationAddMoneyCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Common;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class SendMessageCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<SendMessageCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<SendMessageCommand, ChatMessageDto> _executor;

    public SendMessageCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<SendMessageCommand, ChatMessageDto> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        SendMessageCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

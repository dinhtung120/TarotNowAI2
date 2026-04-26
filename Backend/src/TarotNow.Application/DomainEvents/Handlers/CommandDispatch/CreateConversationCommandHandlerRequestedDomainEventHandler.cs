using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Chat.Commands.CreateConversation;
using TarotNow.Application.Common;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class CreateConversationCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<CreateConversationCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<CreateConversationCommand, ConversationDto> _executor;

    public CreateConversationCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<CreateConversationCommand, ConversationDto> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        CreateConversationCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

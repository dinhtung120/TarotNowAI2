using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Chat.Commands.PresignConversationMedia;
using TarotNow.Application.Common.MediaUpload;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class PresignConversationMediaCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<PresignConversationMediaCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<PresignConversationMediaCommand, PresignedUploadResult> _executor;

    public PresignConversationMediaCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<PresignConversationMediaCommand, PresignedUploadResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        PresignConversationMediaCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Community.Commands.PresignCommunityImage;
using TarotNow.Application.Common.MediaUpload;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class PresignCommunityImageCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<PresignCommunityImageCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<PresignCommunityImageCommand, PresignedUploadResult> _executor;

    public PresignCommunityImageCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<PresignCommunityImageCommand, PresignedUploadResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        PresignCommunityImageCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

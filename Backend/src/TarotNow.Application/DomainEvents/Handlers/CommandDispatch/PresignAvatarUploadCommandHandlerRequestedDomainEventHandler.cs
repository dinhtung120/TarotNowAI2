using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Profile.Commands.PresignAvatarUpload;
using TarotNow.Application.Common.MediaUpload;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class PresignAvatarUploadCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<PresignAvatarUploadCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<PresignAvatarUploadCommand, PresignedUploadResult> _executor;

    public PresignAvatarUploadCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<PresignAvatarUploadCommand, PresignedUploadResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        PresignAvatarUploadCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

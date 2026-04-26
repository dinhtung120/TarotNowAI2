using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Profile.Commands.ConfirmAvatarUpload;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class ConfirmAvatarUploadCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ConfirmAvatarUploadCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<ConfirmAvatarUploadCommand, ConfirmAvatarUploadResult> _executor;

    public ConfirmAvatarUploadCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<ConfirmAvatarUploadCommand, ConfirmAvatarUploadResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        ConfirmAvatarUploadCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Community.Commands.ConfirmCommunityImage;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class ConfirmCommunityImageCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ConfirmCommunityImageCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<ConfirmCommunityImageCommand, ConfirmCommunityImageResult> _executor;

    public ConfirmCommunityImageCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<ConfirmCommunityImageCommand, ConfirmCommunityImageResult> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        ConfirmCommunityImageCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

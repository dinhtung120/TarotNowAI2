using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Promotions.Commands.DeletePromotion;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class DeletePromotionCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<DeletePromotionCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<DeletePromotionCommand, bool> _executor;

    public DeletePromotionCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<DeletePromotionCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        DeletePromotionCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class UpdatePromotionCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<UpdatePromotionCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<UpdatePromotionCommand, bool> _executor;

    public UpdatePromotionCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<UpdatePromotionCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        UpdatePromotionCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

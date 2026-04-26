using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Promotions.Commands.CreatePromotion;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class CreatePromotionCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<CreatePromotionCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<CreatePromotionCommand, bool> _executor;

    public CreatePromotionCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<CreatePromotionCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        CreatePromotionCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}

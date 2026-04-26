using MediatR;
using System;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Promotions.Commands.CreatePromotion;

// Handler tạo mới promotion nạp tiền.
public class CreatePromotionCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<CreatePromotionCommandHandlerRequestedDomainEvent>
{
    private readonly IDepositPromotionRepository _promotionRepository;

    /// <summary>
    /// Khởi tạo handler tạo promotion.
    /// </summary>
    public CreatePromotionCommandHandlerRequestedDomainEventHandler(
        IDepositPromotionRepository promotionRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _promotionRepository = promotionRepository;
    }

    /// <summary>
    /// Xử lý command tạo promotion.
    /// </summary>
    public async Task<bool> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
    {
        var promotion = new DepositPromotion(
            request.MinAmountVnd,
            request.BonusGold,
            request.IsActive);

        await _promotionRepository.AddAsync(promotion, cancellationToken);
        return true;
    }

    protected override async Task HandleDomainEventAsync(
        CreatePromotionCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}

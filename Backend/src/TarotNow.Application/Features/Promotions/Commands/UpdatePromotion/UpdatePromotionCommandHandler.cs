using MediatR;
using System;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;

// Handler cập nhật promotion nạp tiền.
public class UpdatePromotionCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<UpdatePromotionCommandHandlerRequestedDomainEvent>
{
    private readonly IDepositPromotionRepository _promotionRepository;

    /// <summary>
    /// Khởi tạo handler cập nhật promotion.
    /// </summary>
    public UpdatePromotionCommandHandlerRequestedDomainEventHandler(
        IDepositPromotionRepository promotionRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _promotionRepository = promotionRepository;
    }

    /// <summary>
    /// Xử lý command cập nhật promotion.
    /// </summary>
    public async Task<bool> Handle(UpdatePromotionCommand request, CancellationToken cancellationToken)
    {
        var promotion = await _promotionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (promotion == null)
        {
            return false;
        }

        promotion.Update(request.MinAmountVnd, request.BonusGold, request.IsActive);
        await _promotionRepository.UpdateAsync(promotion, cancellationToken);
        return true;
    }

    protected override async Task HandleDomainEventAsync(
        UpdatePromotionCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}

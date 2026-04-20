using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Promotions.Commands.CreatePromotion;

// Handler tạo mới promotion nạp tiền.
public class CreatePromotionCommandHandler : IRequestHandler<CreatePromotionCommand, bool>
{
    private readonly IDepositPromotionRepository _promotionRepository;

    /// <summary>
    /// Khởi tạo handler tạo promotion.
    /// </summary>
    public CreatePromotionCommandHandler(IDepositPromotionRepository promotionRepository)
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
}

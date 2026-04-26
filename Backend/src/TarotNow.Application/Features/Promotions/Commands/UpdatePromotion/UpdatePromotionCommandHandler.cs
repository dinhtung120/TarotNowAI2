using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;

// Handler cập nhật promotion nạp tiền.
public class UpdatePromotionCommandExecutor : ICommandExecutionExecutor<UpdatePromotionCommand, bool>
{
    private readonly IDepositPromotionRepository _promotionRepository;

    /// <summary>
    /// Khởi tạo handler cập nhật promotion.
    /// </summary>
    public UpdatePromotionCommandExecutor(IDepositPromotionRepository promotionRepository)
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
}

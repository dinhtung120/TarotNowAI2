/*
 * ===================================================================
 * FILE: UpdatePromotionCommandValidator.cs
 * NAMESPACE: TarotNow.Application.Features.Promotions.Commands.UpdatePromotion
 * ===================================================================
 * MỤC ĐÍCH:
 *   Chặn đường những bản Cập Nhật mang tính Tự Hủy (ví dụ đổi Kim Cương thưởng về số Âm).
 * ===================================================================
 */

using FluentValidation;

namespace TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;

public class UpdatePromotionCommandValidator : AbstractValidator<UpdatePromotionCommand>
{
    public UpdatePromotionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        // Không Gì Tệ Hơn Một Khuyến Mãi Đội Giá Âm VNĐ.
        RuleFor(x => x.MinAmountVnd).GreaterThan(0);
        RuleFor(x => x.BonusDiamond).GreaterThan(0);
    }
}

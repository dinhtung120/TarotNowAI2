using FluentValidation;

namespace TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;

// Validator cho command cập nhật promotion.
public class UpdatePromotionCommandValidator : AbstractValidator<UpdatePromotionCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu cập nhật khuyến mãi.
    /// </summary>
    public UpdatePromotionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.MinAmountVnd)
            .GreaterThan(0);

        RuleFor(x => x.BonusGold)
            .GreaterThanOrEqualTo(0);
    }
}

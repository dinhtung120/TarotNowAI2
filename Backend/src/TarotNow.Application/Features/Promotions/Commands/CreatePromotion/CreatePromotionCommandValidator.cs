using FluentValidation;

namespace TarotNow.Application.Features.Promotions.Commands.CreatePromotion;

// Validator cho command tạo promotion.
public class CreatePromotionCommandValidator : AbstractValidator<CreatePromotionCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu tạo khuyến mãi.
    /// </summary>
    public CreatePromotionCommandValidator()
    {
        RuleFor(x => x.MinAmountVnd)
            .GreaterThan(0);

        RuleFor(x => x.BonusGold)
            .GreaterThanOrEqualTo(0);
    }
}

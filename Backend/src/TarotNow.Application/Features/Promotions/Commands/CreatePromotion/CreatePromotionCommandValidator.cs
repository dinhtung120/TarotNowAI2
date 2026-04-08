using FluentValidation;

namespace TarotNow.Application.Features.Promotions.Commands.CreatePromotion;

// Validator cho command tạo promotion.
public class CreatePromotionCommandValidator : AbstractValidator<CreatePromotionCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu tạo khuyến mãi.
    /// Luồng xử lý: ép MinAmountVnd và BonusDiamond phải lớn hơn 0 để tránh cấu hình khuyến mãi vô nghĩa.
    /// </summary>
    public CreatePromotionCommandValidator()
    {
        RuleFor(x => x.MinAmountVnd)
            .GreaterThan(0);
        // Business rule: ngưỡng nạp phải dương mới có ý nghĩa áp dụng.

        RuleFor(x => x.BonusDiamond)
            .GreaterThan(0);
        // Business rule: mức thưởng phải dương để tránh promotion không tạo giá trị.
    }
}

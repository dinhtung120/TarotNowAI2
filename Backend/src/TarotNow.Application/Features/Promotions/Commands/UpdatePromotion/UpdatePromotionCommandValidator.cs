using FluentValidation;

namespace TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;

// Validator cho command cập nhật promotion.
public class UpdatePromotionCommandValidator : AbstractValidator<UpdatePromotionCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu cập nhật promotion.
    /// Luồng xử lý: bắt buộc Id hợp lệ và ép các ngưỡng tiền/thưởng phải dương.
    /// </summary>
    public UpdatePromotionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        // Id bắt buộc để xác định đúng promotion mục tiêu.

        RuleFor(x => x.MinAmountVnd)
            .GreaterThan(0);
        // Ngưỡng nạp phải dương để tránh cấu hình không khả thi.

        RuleFor(x => x.BonusDiamond)
            .GreaterThan(0);
        // Số kim cương thưởng phải dương để đảm bảo promotion có ý nghĩa.
    }
}

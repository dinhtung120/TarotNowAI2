using FluentValidation;

namespace TarotNow.Application.Features.Promotions.Commands.DeletePromotion;

// Validator cho command xóa promotion.
public class DeletePromotionCommandValidator : AbstractValidator<DeletePromotionCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho yêu cầu xóa promotion.
    /// Luồng xử lý: bắt buộc Id không rỗng để định vị đúng bản ghi cần xóa.
    /// </summary>
    public DeletePromotionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        // Id rỗng sẽ dẫn đến truy vấn sai hoặc xóa nhầm dữ liệu.
    }
}

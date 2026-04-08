using FluentValidation;

namespace TarotNow.Application.Features.Subscription.Commands.CreateSubscriptionPlan;

// Validator cho command tạo subscription plan.
public class CreateSubscriptionPlanCommandValidator : AbstractValidator<CreateSubscriptionPlanCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu tạo gói subscription.
    /// Luồng xử lý: kiểm tra tên gói, giá, thời hạn, entitlements và thứ tự hiển thị theo ràng buộc nghiệp vụ.
    /// </summary>
    public CreateSubscriptionPlanCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Plan name is required.")
            .MaximumLength(200)
            .WithMessage("Plan name must not exceed 200 characters.");
        // Tên gói bắt buộc và giới hạn để đồng bộ hiển thị UI.

        RuleFor(x => x.PriceDiamond)
            .GreaterThan(0)
            .WithMessage("PriceDiamond must be greater than 0.");
        // Giá gói phải dương để tránh cấu hình gói bán với giá không hợp lệ.

        RuleFor(x => x.DurationDays)
            .InclusiveBetween(1, 365)
            .WithMessage("DurationDays must be between 1 and 365.");
        // Thời hạn gói giới hạn trong khoảng nghiệp vụ hỗ trợ.

        RuleFor(x => x.EntitlementsJson)
            .NotEmpty()
            .WithMessage("EntitlementsJson is required.");
        // Entitlements bắt buộc để hệ thống biết quyền lợi cần cấp khi mua gói.

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0)
            .WithMessage("DisplayOrder must be 0 or positive.");
        // Thứ tự hiển thị không âm để đảm bảo sắp xếp ổn định.
    }
}

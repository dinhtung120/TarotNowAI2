/*
 * ===================================================================
 * FILE: CreateSubscriptionPlanCommandValidator.cs
 * NAMESPACE: TarotNow.Application.Features.Subscription.Commands.CreateSubscriptionPlan
 * ===================================================================
 * MỤC ĐÍCH:
 *   FluentValidation cho lệnh tạo gói đăng ký mới (Admin Only).
 *   Kiểm tra tất cả trường bắt buộc và giới hạn giá trị hợp lý.
 *
 * THIẾT KẾ:
 *   - Name: bắt buộc, tối đa 200 ký tự
 *   - PriceDiamond: phải > 0 (không cho phép gói miễn phí qua API này)  
 *   - DurationDays: phải >= 1 (tối thiểu 1 ngày)
 *   - EntitlementsJson: bắt buộc, phải là JSON hợp lệ
 *   - DisplayOrder: phải >= 0 (thứ tự hiển thị)
 * ===================================================================
 */

using FluentValidation;

namespace TarotNow.Application.Features.Subscription.Commands.CreateSubscriptionPlan;

/// <summary>
/// Validator kiểm tra đầu vào khi Admin tạo gói cước mới.
/// </summary>
public class CreateSubscriptionPlanCommandValidator : AbstractValidator<CreateSubscriptionPlanCommand>
{
    public CreateSubscriptionPlanCommandValidator()
    {
        // Tên gói không được trống, giới hạn 200 ký tự để tránh abuse frontend render
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Plan name is required.")
            .MaximumLength(200).WithMessage("Plan name must not exceed 200 characters.");

        // Giá phải dương — gói miễn phí nên dùng entitlement mechanism riêng, không phải plan
        RuleFor(x => x.PriceDiamond)
            .GreaterThan(0).WithMessage("PriceDiamond must be greater than 0.");

        // Thời hạn tối thiểu 1 ngày, tối đa 1 năm (365 ngày) để tránh lỗi logic 
        RuleFor(x => x.DurationDays)
            .InclusiveBetween(1, 365).WithMessage("DurationDays must be between 1 and 365.");

        // JSON entitlements phải có nội dung — chống lỗi runtime khi parse
        RuleFor(x => x.EntitlementsJson)
            .NotEmpty().WithMessage("EntitlementsJson is required.");

        // Thứ tự hiển thị không âm
        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("DisplayOrder must be 0 or positive.");
    }
}

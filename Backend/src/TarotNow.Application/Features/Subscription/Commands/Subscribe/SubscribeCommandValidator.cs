using FluentValidation;

namespace TarotNow.Application.Features.Subscription.Commands.Subscribe;

// Validator cho command mua subscription.
public class SubscribeCommandValidator : AbstractValidator<SubscribeCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu subscribe.
    /// Luồng xử lý: kiểm tra UserId, PlanId và IdempotencyKey bắt buộc để chống giao dịch trùng.
    /// </summary>
    public SubscribeCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");
        // UserId bắt buộc để xác định chủ tài khoản thanh toán.

        RuleFor(x => x.PlanId)
            .NotEmpty()
            .WithMessage("PlanId is required.");
        // PlanId bắt buộc để xác định gói cần mua.

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .WithMessage("IdempotencyKey is required to prevent duplicate transactions.")
            .MaximumLength(200)
            .WithMessage("IdempotencyKey must not exceed 200 characters.");
        // IdempotencyKey bắt buộc để bảo vệ luồng retry không tạo giao dịch lặp.
    }
}

using FluentValidation;

namespace TarotNow.Application.Features.CheckIn.Commands.PurchaseFreeze;

// Validator đầu vào cho command mua phục hồi streak.
public class PurchaseStreakFreezeCommandValidator : AbstractValidator<PurchaseStreakFreezeCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho PurchaseStreakFreezeCommand.
    /// Luồng xử lý: bắt buộc UserId và IdempotencyKey, đồng thời giới hạn độ dài key.
    /// </summary>
    public PurchaseStreakFreezeCommandValidator()
    {
        // UserId bắt buộc để định vị ví và trạng thái streak.
        RuleFor(x => x.UserId)
            .NotEmpty();

        // IdempotencyKey bắt buộc để tránh giao dịch trừ kim cương trùng.
        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(128);
    }
}

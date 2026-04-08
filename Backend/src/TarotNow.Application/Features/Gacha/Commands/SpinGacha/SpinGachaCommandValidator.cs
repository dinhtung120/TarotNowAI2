using FluentValidation;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

// Validator đầu vào cho command quay gacha.
public class SpinGachaCommandValidator : AbstractValidator<SpinGachaCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho SpinGachaCommand.
    /// Luồng xử lý: bắt buộc BannerCode và IdempotencyKey, giới hạn độ dài IdempotencyKey.
    /// </summary>
    public SpinGachaCommandValidator()
    {
        // BannerCode bắt buộc để định vị banner quay.
        RuleFor(v => v.BannerCode)
            .NotEmpty().WithMessage("Banner code is required.");

        // IdempotencyKey bắt buộc để đảm bảo replay an toàn khi retry.
        RuleFor(v => v.IdempotencyKey)
            .NotEmpty().WithMessage("IdempotencyKey is required.")
            .MaximumLength(128).WithMessage("IdempotencyKey must not exceed 128 characters.");
    }
}

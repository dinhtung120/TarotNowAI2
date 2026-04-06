using FluentValidation;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public class SpinGachaCommandValidator : AbstractValidator<SpinGachaCommand>
{
    public SpinGachaCommandValidator()
    {
        RuleFor(v => v.BannerCode)
            .NotEmpty().WithMessage("Banner code is required.");
            
        RuleFor(v => v.IdempotencyKey)
            .NotEmpty().WithMessage("IdempotencyKey is required.")
            .MaximumLength(128).WithMessage("IdempotencyKey must not exceed 128 characters.");
    }
}

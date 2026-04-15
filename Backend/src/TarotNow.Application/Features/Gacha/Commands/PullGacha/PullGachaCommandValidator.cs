using FluentValidation;

namespace TarotNow.Application.Features.Gacha.Commands.PullGacha;

/// <summary>
/// Validator cho PullGachaCommand.
/// </summary>
public sealed class PullGachaCommandValidator : AbstractValidator<PullGachaCommand>
{
    /// <summary>
    /// Khởi tạo rule validate command.
    /// </summary>
    public PullGachaCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.PoolCode)
            .NotEmpty().WithMessage("PoolCode is required.")
            .MaximumLength(64).WithMessage("PoolCode must not exceed 64 characters.");

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty().WithMessage("IdempotencyKey is required.")
            .MaximumLength(128).WithMessage("IdempotencyKey must not exceed 128 characters.");

        RuleFor(x => x.Count)
            .InclusiveBetween(1, 10).WithMessage("Count must be between 1 and 10.");
    }
}

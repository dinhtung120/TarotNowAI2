using FluentValidation;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public class ResolveDisputeCommandValidator : AbstractValidator<ResolveDisputeCommand>
{
    public ResolveDisputeCommandValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty();

        RuleFor(x => x.AdminId)
            .NotEmpty();

        RuleFor(x => x.Action)
            .NotEmpty()
            .Must(action =>
            {
                var normalized = action?.Trim().ToLowerInvariant();
                return normalized is "release" or "refund" or "split";
            })
            .WithMessage("Action phải là 'release', 'refund' hoặc 'split'.");

        RuleFor(x => x.SplitPercentToReader)
            .NotNull()
            .InclusiveBetween(1, 99)
            .When(x => string.Equals(x.Action?.Trim(), "split", StringComparison.OrdinalIgnoreCase));

        RuleFor(x => x.AdminNote)
            .MaximumLength(1000)
            .When(x => string.IsNullOrWhiteSpace(x.AdminNote) == false);
    }
}

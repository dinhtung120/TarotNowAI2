using FluentValidation;

namespace TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal;

public class ProcessWithdrawalCommandValidator : AbstractValidator<ProcessWithdrawalCommand>
{
    public ProcessWithdrawalCommandValidator()
    {
        RuleFor(x => x.RequestId)
            .NotEmpty();

        RuleFor(x => x.AdminId)
            .NotEmpty();

        RuleFor(x => x.Action)
            .NotEmpty()
            .Must(action =>
            {
                var normalized = action?.Trim().ToLowerInvariant();
                return normalized is "approve" or "reject";
            })
            .WithMessage("Action phải là 'approve' hoặc 'reject'.");

        RuleFor(x => x.AdminNote)
            .MaximumLength(1000)
            .When(x => string.IsNullOrWhiteSpace(x.AdminNote) == false);

        RuleFor(x => x.MfaCode)
            .NotEmpty()
            .Length(6, 64);
    }
}

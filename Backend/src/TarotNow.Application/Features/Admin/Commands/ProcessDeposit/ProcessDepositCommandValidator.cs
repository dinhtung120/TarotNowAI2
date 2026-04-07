using FluentValidation;

namespace TarotNow.Application.Features.Admin.Commands.ProcessDeposit;

public class ProcessDepositCommandValidator : AbstractValidator<ProcessDepositCommand>
{
    public ProcessDepositCommandValidator()
    {
        RuleFor(x => x.DepositId)
            .NotEmpty();

        RuleFor(x => x.Action)
            .NotEmpty()
            .Must(action =>
            {
                var normalized = action?.Trim().ToLowerInvariant();
                return normalized is "approve" or "reject";
            })
            .WithMessage("Action phải là 'approve' hoặc 'reject'.");

        RuleFor(x => x.TransactionId)
            .MaximumLength(128)
            .When(x => string.IsNullOrWhiteSpace(x.TransactionId) == false);
    }
}

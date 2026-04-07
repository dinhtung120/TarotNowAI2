using FluentValidation;

namespace TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;

public class CreateWithdrawalCommandValidator : AbstractValidator<CreateWithdrawalCommand>
{
    public CreateWithdrawalCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.AmountDiamond)
            .GreaterThan(0);

        RuleFor(x => x.BankName)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.BankAccountName)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.BankAccountNumber)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.MfaCode)
            .NotEmpty()
            .Length(6, 64);
    }
}

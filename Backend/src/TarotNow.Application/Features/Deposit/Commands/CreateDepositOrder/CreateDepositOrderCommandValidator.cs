using FluentValidation;

namespace TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;

public class CreateDepositOrderCommandValidator : AbstractValidator<CreateDepositOrderCommand>
{
    public CreateDepositOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.AmountVnd)
            .GreaterThan(0).WithMessage("AmountVnd must be greater than 0.")
            .Must(amount => amount % 10000 == 0).WithMessage("AmountVnd must be a multiple of 10,000.");
    }
}

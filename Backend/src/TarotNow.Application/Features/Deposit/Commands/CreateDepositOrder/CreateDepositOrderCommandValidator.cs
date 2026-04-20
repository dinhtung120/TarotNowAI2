using FluentValidation;

namespace TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;

// Validator đầu vào cho command tạo đơn nạp.
public class CreateDepositOrderCommandValidator : AbstractValidator<CreateDepositOrderCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho CreateDepositOrderCommand.
    /// </summary>
    public CreateDepositOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.PackageCode)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(128);
    }
}



using FluentValidation;

namespace TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;

// Validator đầu vào cho command tạo đơn nạp.
public class CreateDepositOrderCommandValidator : AbstractValidator<CreateDepositOrderCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho CreateDepositOrderCommand.
    /// Luồng xử lý: bắt buộc UserId, AmountVnd > 0 và chia hết 1.000, IdempotencyKey bắt buộc và giới hạn độ dài.
    /// </summary>
    public CreateDepositOrderCommandValidator()
    {
        // UserId bắt buộc để tạo order đúng người dùng.
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        // AmountVnd phải hợp lệ theo bước nạp chuẩn của hệ thống.
        RuleFor(x => x.AmountVnd)
            .GreaterThan(0).WithMessage("AmountVnd must be greater than 0.")
            .Must(amount => amount % 1000 == 0).WithMessage("AmountVnd must be a multiple of 1,000.");

        // IdempotencyKey bắt buộc để chống tạo order trùng.
        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(128);
    }
}

using FluentValidation;

namespace TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;

// Validator cho command tạo yêu cầu rút tiền.
public class CreateWithdrawalCommandValidator : AbstractValidator<CreateWithdrawalCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu rút tiền.
    /// Luồng xử lý: kiểm tra định danh user, số diamond, idempotency key, thông tin ngân hàng và mã MFA.
    /// </summary>
    public CreateWithdrawalCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
        // UserId bắt buộc để xác định chủ yêu cầu rút.

        RuleFor(x => x.AmountDiamond)
            .GreaterThan(0);
        // Số lượng rút phải dương.

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(128);
        // Idempotency key bắt buộc và giới hạn độ dài để chống request trùng.

        RuleFor(x => x.BankName)
            .NotEmpty()
            .MaximumLength(255);
        // Tên ngân hàng bắt buộc để tạo lệnh chi trả.

        RuleFor(x => x.BankAccountName)
            .NotEmpty()
            .MaximumLength(255);
        // Tên chủ tài khoản bắt buộc để đối soát.

        RuleFor(x => x.BankAccountNumber)
            .NotEmpty()
            .MaximumLength(50);
        // Số tài khoản bắt buộc để chuyển khoản.

        RuleFor(x => x.MfaCode)
            .NotEmpty()
            .Length(6, 64);
        // Mã MFA bắt buộc để xác thực thao tác rút tiền.
    }
}

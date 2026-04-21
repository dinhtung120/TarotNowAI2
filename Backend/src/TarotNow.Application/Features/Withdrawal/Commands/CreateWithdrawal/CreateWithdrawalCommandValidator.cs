using FluentValidation;
using TarotNow.Application.Common.Constants;

namespace TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;

// Validator cho command tạo yêu cầu rút tiền.
public class CreateWithdrawalCommandValidator : AbstractValidator<CreateWithdrawalCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu rút tiền.
    /// Luồng xử lý: kiểm tra định danh user, số diamond, idempotency key, thông tin ngân hàng và ghi chú.
    /// </summary>
    public CreateWithdrawalCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
        // UserId bắt buộc để xác định chủ yêu cầu rút.

        RuleFor(x => x.AmountDiamond)
            .GreaterThanOrEqualTo(WithdrawalPolicyConstants.MinimumWithdrawDiamond);
        // Số lượng rút phải đạt mức tối thiểu.

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(WithdrawalPolicyConstants.IdempotencyKeyMaxLength);
        // Idempotency key bắt buộc và giới hạn độ dài để chống request trùng.

        RuleFor(x => x.UserNote)
            .MaximumLength(WithdrawalPolicyConstants.NoteMaxLength)
            .When(x => string.IsNullOrWhiteSpace(x.UserNote) == false);
        // Ghi chú user là tùy chọn nhưng bị giới hạn độ dài.
    }
}

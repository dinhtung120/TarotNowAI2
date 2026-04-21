using FluentValidation;
using System;
using TarotNow.Application.Common.Constants;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal;

// Validator cho command xử lý withdrawal.
public class ProcessWithdrawalCommandValidator : AbstractValidator<ProcessWithdrawalCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu process withdrawal.
    /// Luồng xử lý: kiểm tra request/admin/action/idempotency và giới hạn độ dài admin note.
    /// </summary>
    public ProcessWithdrawalCommandValidator()
    {
        RuleFor(x => x.RequestId)
            .NotEmpty();
        // RequestId bắt buộc để định vị yêu cầu rút cần xử lý.

        RuleFor(x => x.AdminId)
            .NotEmpty();
        // AdminId bắt buộc để ghi audit và xác thực quyền thao tác.

        RuleFor(x => x.Action)
            .NotEmpty()
            .Must(action =>
            {
                var normalized = action?.Trim().ToLowerInvariant();
                return normalized is WithdrawalProcessAction.Approve or WithdrawalProcessAction.Reject;
            })
            .WithMessage("Action phải là 'approve' hoặc 'reject'.");
        // Chỉ chấp nhận hai action nghiệp vụ hỗ trợ.

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(WithdrawalPolicyConstants.IdempotencyKeyMaxLength);
        // Idempotency key bắt buộc để chống double process khi admin retry/click lặp.

        RuleFor(x => x.AdminNote)
            .MaximumLength(WithdrawalPolicyConstants.NoteMaxLength)
            .When(x => string.IsNullOrWhiteSpace(x.AdminNote) == false);
        // Ghi chú tùy chọn nhưng giới hạn độ dài để tránh dữ liệu quá lớn.

        RuleFor(x => x.AdminNote)
            .NotEmpty()
            .When(x => string.Equals(x.Action?.Trim(), WithdrawalProcessAction.Reject, StringComparison.OrdinalIgnoreCase))
            .WithMessage("Lý do từ chối là bắt buộc khi reject.");
        // Từ chối yêu cầu phải lưu lại lý do để phục vụ giải trình với user.
    }
}

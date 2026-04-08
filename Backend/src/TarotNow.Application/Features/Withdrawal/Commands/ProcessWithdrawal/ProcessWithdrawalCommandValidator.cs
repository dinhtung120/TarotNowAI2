using FluentValidation;

namespace TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal;

// Validator cho command xử lý withdrawal.
public class ProcessWithdrawalCommandValidator : AbstractValidator<ProcessWithdrawalCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu process withdrawal.
    /// Luồng xử lý: kiểm tra request/admin/action/mfa và giới hạn độ dài admin note.
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
                return normalized is "approve" or "reject";
            })
            .WithMessage("Action phải là 'approve' hoặc 'reject'.");
        // Chỉ chấp nhận hai action nghiệp vụ hỗ trợ.

        RuleFor(x => x.AdminNote)
            .MaximumLength(1000)
            .When(x => string.IsNullOrWhiteSpace(x.AdminNote) == false);
        // Ghi chú tùy chọn nhưng giới hạn độ dài để tránh dữ liệu quá lớn.

        RuleFor(x => x.MfaCode)
            .NotEmpty()
            .Length(6, 64);
        // Mã MFA admin bắt buộc cho thao tác duyệt/từ chối yêu cầu rút.
    }
}

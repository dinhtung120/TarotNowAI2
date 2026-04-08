using FluentValidation;

namespace TarotNow.Application.Features.Admin.Commands.ProcessDeposit;

// Validator đầu vào cho command xử lý lệnh nạp tiền.
public class ProcessDepositCommandValidator : AbstractValidator<ProcessDepositCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho ProcessDepositCommand.
    /// Luồng xử lý: kiểm tra deposit id, action hợp lệ và giới hạn độ dài transaction id.
    /// </summary>
    public ProcessDepositCommandValidator()
    {
        // DepositId bắt buộc để xác định đúng lệnh nạp cần xử lý.
        RuleFor(x => x.DepositId)
            .NotEmpty();

        // Action chỉ chấp nhận approve/reject để tránh thao tác ngoài nghiệp vụ.
        RuleFor(x => x.Action)
            .NotEmpty()
            .Must(action =>
            {
                // Chuẩn hóa input để tránh sai do khác biệt hoa-thường hoặc khoảng trắng.
                var normalized = action?.Trim().ToLowerInvariant();
                return normalized is "approve" or "reject";
            })
            .WithMessage("Action phải là 'approve' hoặc 'reject'.");

        // TransactionId tùy chọn nhưng giới hạn độ dài để bảo vệ dữ liệu lưu trữ.
        RuleFor(x => x.TransactionId)
            .MaximumLength(128)
            .When(x => string.IsNullOrWhiteSpace(x.TransactionId) == false);
    }
}

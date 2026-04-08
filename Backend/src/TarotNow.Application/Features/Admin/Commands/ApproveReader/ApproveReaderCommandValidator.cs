using FluentValidation;

namespace TarotNow.Application.Features.Admin.Commands.ApproveReader;

// Validator đầu vào cho command duyệt reader.
public class ApproveReaderCommandValidator : AbstractValidator<ApproveReaderCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho ApproveReaderCommand.
    /// Luồng xử lý: kiểm tra request id, action, admin id và giới hạn độ dài admin note.
    /// </summary>
    public ApproveReaderCommandValidator()
    {
        // RequestId bắt buộc để truy xuất đúng đơn cần duyệt.
        RuleFor(x => x.RequestId)
            .NotEmpty()
            .MaximumLength(128);

        // Rule nghiệp vụ: action chỉ được phép approve hoặc reject.
        RuleFor(x => x.Action)
            .NotEmpty()
            .Must(action => action is "approve" or "reject")
            .WithMessage("Action phải là 'approve' hoặc 'reject'.");

        // AdminId bắt buộc để phục vụ audit người xử lý.
        RuleFor(x => x.AdminId)
            .NotEmpty();

        // AdminNote là tùy chọn nhưng phải giới hạn chiều dài để tránh dữ liệu quá lớn.
        RuleFor(x => x.AdminNote)
            .MaximumLength(1000)
            .When(x => string.IsNullOrWhiteSpace(x.AdminNote) == false);
    }
}

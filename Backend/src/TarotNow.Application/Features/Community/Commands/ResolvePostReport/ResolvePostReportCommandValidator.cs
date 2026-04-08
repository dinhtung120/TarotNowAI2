using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Commands.ResolvePostReport;

// Validator đầu vào cho command xử lý report bài viết.
public class ResolvePostReportCommandValidator : AbstractValidator<ResolvePostReportCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho ResolvePostReportCommand.
    /// Luồng xử lý: bắt buộc ReportId/AdminId/Result, giới hạn Result theo enum moderation và kiểm tra độ dài AdminNote.
    /// </summary>
    public ResolvePostReportCommandValidator()
    {
        // ReportId bắt buộc để định vị report cần xử lý.
        RuleFor(x => x.ReportId)
            .NotEmpty();

        // AdminId bắt buộc để truy vết người xử lý.
        RuleFor(x => x.AdminId)
            .NotEmpty();

        // Result bắt buộc và phải là một trong các kết quả moderation cho phép.
        RuleFor(x => x.Result)
            .NotEmpty()
            .Must(result => result is ModerationResult.Warn
                or ModerationResult.RemovePost
                or ModerationResult.FreezeAccount
                or ModerationResult.NoAction)
            .WithMessage("Kết quả xử lý không hợp lệ.");

        // AdminNote là tùy chọn nhưng bị giới hạn độ dài.
        RuleFor(x => x.AdminNote)
            .MaximumLength(1000)
            .When(x => string.IsNullOrWhiteSpace(x.AdminNote) == false);
    }
}

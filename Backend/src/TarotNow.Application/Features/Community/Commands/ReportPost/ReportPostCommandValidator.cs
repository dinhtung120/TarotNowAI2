using FluentValidation;
using TarotNow.Application.Features.Community;

namespace TarotNow.Application.Features.Community.Commands.ReportPost;

// Validator đầu vào cho command báo cáo bài viết.
public sealed class ReportPostCommandValidator : AbstractValidator<ReportPostCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho ReportPostCommand.
    /// Luồng xử lý: bắt buộc PostId/ReporterId/ReasonCode/Description và kiểm tra reason code theo danh sách hỗ trợ.
    /// </summary>
    public ReportPostCommandValidator()
    {
        // PostId bắt buộc để định vị bài viết bị báo cáo.
        RuleFor(x => x.PostId)
            .NotEmpty();

        // ReporterId bắt buộc để truy vết người gửi report.
        RuleFor(x => x.ReporterId)
            .NotEmpty();

        // ReasonCode bắt buộc và phải thuộc whitelist của module.
        RuleFor(x => x.ReasonCode)
            .NotEmpty()
            .Must(code => CommunityModuleConstants.SupportedReportReasonCodes.Contains(code))
            .WithMessage("Unsupported reason code.");

        // Description bắt buộc, đủ độ dài tối thiểu và không quá dài.
        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(1000);
    }
}

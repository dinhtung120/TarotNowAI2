using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.CreateReport;

// Validator đầu vào cho command tạo báo cáo vi phạm.
public class CreateReportCommandValidator : AbstractValidator<CreateReportCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho CreateReportCommand.
    /// Luồng xử lý: kiểm tra reporter id, target type/id, conversation ref tùy chọn và reason hợp lệ.
    /// </summary>
    public CreateReportCommandValidator()
    {
        // ReporterId bắt buộc.
        RuleFor(x => x.ReporterId)
            .NotEmpty();

        // TargetType chỉ chấp nhận message/conversation/user.
        RuleFor(x => x.TargetType)
            .NotEmpty()
            .Must(type =>
            {
                // Chuẩn hóa trước khi so khớp để tránh lệch hoa-thường.
                var normalized = type?.Trim().ToLowerInvariant();
                return normalized is "message" or "conversation" or "user";
            })
            .WithMessage("TargetType phải là message, conversation hoặc user.");

        // TargetId bắt buộc và giới hạn độ dài.
        RuleFor(x => x.TargetId)
            .NotEmpty()
            .MaximumLength(128);

        // ConversationRef là tùy chọn nhưng phải giới hạn độ dài.
        RuleFor(x => x.ConversationRef)
            .MaximumLength(128)
            .When(x => string.IsNullOrWhiteSpace(x.ConversationRef) == false);

        // Reason bắt buộc đủ dài để phục vụ moderation.
        RuleFor(x => x.Reason)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(1000);
    }
}

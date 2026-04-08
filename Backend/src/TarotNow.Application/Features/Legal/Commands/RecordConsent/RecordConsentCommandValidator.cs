using FluentValidation;

namespace TarotNow.Application.Features.Legal.Commands.RecordConsent;

// Validator đảm bảo dữ liệu consent tối thiểu hợp lệ trước khi vào handler.
public class RecordConsentCommandValidator : AbstractValidator<RecordConsentCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho command ghi nhận consent.
    /// Luồng xử lý: chặn dữ liệu thiếu bắt buộc và giới hạn DocumentType theo danh sách tài liệu được hỗ trợ.
    /// </summary>
    public RecordConsentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");
        // UserId bắt buộc để bảo đảm consent luôn gắn với một tài khoản cụ thể.

        RuleFor(x => x.DocumentType)
            .NotEmpty()
            .WithMessage("DocumentType is required.")
            .Must(type => type == "TOS" || type == "PrivacyPolicy" || type == "AiDisclaimer")
            .WithMessage("DocumentType must be TOS, PrivacyPolicy, or AiDisclaimer.");
        // Chỉ cho phép các document type hệ thống hỗ trợ để tránh ghi nhận sai ngữ nghĩa pháp lý.

        RuleFor(x => x.Version)
            .NotEmpty()
            .WithMessage("Version is required.");
        // Version bắt buộc để truy vết user đã đồng ý chính xác bản điều khoản nào.
    }
}

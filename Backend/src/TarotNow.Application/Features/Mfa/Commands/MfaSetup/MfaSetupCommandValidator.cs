using FluentValidation;

namespace TarotNow.Application.Features.Mfa.Commands.MfaSetup;

// Validator cho command setup MFA.
public class MfaSetupCommandValidator : AbstractValidator<MfaSetupCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho yêu cầu setup MFA.
    /// Luồng xử lý: đảm bảo luôn có UserId hợp lệ trước khi vào handler.
    /// </summary>
    public MfaSetupCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
        // UserId bắt buộc để tránh chạy setup trên ngữ cảnh user không xác định.
    }
}

using FluentValidation;

namespace TarotNow.Application.Features.Auth.Commands.VerifyEmail;

// Validator đầu vào cho command xác minh email.
public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho VerifyEmailCommand.
    /// Luồng xử lý: kiểm tra email hợp lệ và OTP bắt buộc có độ dài chuẩn.
    /// </summary>
    public VerifyEmailCommandValidator()
    {
        // Email bắt buộc, đúng định dạng.
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(254);

        // OTP bắt buộc và giới hạn độ dài để phù hợp nhiều dạng mã xác thực.
        RuleFor(x => x.OtpCode)
            .NotEmpty()
            .Length(4, 12);
    }
}

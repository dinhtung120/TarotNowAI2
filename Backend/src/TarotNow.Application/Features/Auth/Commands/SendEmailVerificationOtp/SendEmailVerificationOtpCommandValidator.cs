using FluentValidation;

namespace TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;

// Validator đầu vào cho command gửi OTP xác minh email.
public class SendEmailVerificationOtpCommandValidator : AbstractValidator<SendEmailVerificationOtpCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho SendEmailVerificationOtpCommand.
    /// Luồng xử lý: kiểm tra email bắt buộc, đúng định dạng và giới hạn độ dài.
    /// </summary>
    public SendEmailVerificationOtpCommandValidator()
    {
        // Email hợp lệ là điều kiện bắt buộc để gửi OTP.
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(254);
    }
}

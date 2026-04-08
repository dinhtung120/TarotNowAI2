using FluentValidation;

namespace TarotNow.Application.Features.Auth.Commands.ForgotPassword;

// Validator đầu vào cho command quên mật khẩu.
public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho ForgotPasswordCommand.
    /// Luồng xử lý: kiểm tra email bắt buộc, đúng định dạng và giới hạn độ dài.
    /// </summary>
    public ForgotPasswordCommandValidator()
    {
        // Email hợp lệ là điều kiện tối thiểu để gửi OTP reset.
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(254);
    }
}

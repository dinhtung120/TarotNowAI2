using FluentValidation;

namespace TarotNow.Application.Features.Auth.Commands.ResetPassword;

// Validator đầu vào cho command đặt lại mật khẩu.
public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho ResetPasswordCommand.
    /// Luồng xử lý: kiểm tra email, otp code và mật khẩu mới theo độ dài tối thiểu/tối đa.
    /// </summary>
    public ResetPasswordCommandValidator()
    {
        // Email bắt buộc và đúng định dạng.
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(254);

        // OTP bắt buộc và giới hạn chiều dài để phù hợp nhiều kiểu OTP.
        RuleFor(x => x.OtpCode)
            .NotEmpty()
            .Length(4, 12);

        // Mật khẩu mới bắt buộc và giới hạn độ dài an toàn.
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(256);
    }
}

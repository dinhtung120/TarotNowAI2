

using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.ResetPassword;

// Command đặt lại mật khẩu bằng OTP đã gửi qua email.
public class ResetPasswordCommand : IRequest<bool>
{
    // Email tài khoản cần đặt lại mật khẩu.
    public string Email { get; set; } = string.Empty;

    // Mã OTP xác thực reset password.
    public string OtpCode { get; set; } = string.Empty;

    // Mật khẩu mới dạng thô để băm trước khi lưu.
    public string NewPassword { get; set; } = string.Empty;
}

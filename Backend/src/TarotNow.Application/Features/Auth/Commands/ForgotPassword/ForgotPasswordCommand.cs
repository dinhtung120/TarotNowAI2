

using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.ForgotPassword;

// Command yêu cầu gửi OTP đặt lại mật khẩu qua email.
public class ForgotPasswordCommand : IRequest<bool>
{
    // Email tài khoản cần khởi tạo quy trình quên mật khẩu.
    public string Email { get; set; } = string.Empty;
}

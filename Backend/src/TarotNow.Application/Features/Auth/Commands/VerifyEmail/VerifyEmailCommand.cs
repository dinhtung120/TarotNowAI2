

using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.VerifyEmail;

// Command xác minh email bằng OTP.
public class VerifyEmailCommand : IRequest<bool>
{
    // Email tài khoản cần xác minh.
    public string Email { get; set; } = string.Empty;

    // OTP xác minh email.
    public string OtpCode { get; set; } = string.Empty;
}

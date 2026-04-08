

using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;

// Command gửi OTP xác thực email cho người dùng chưa kích hoạt.
public class SendEmailVerificationOtpCommand : IRequest<bool>
{
    // Email cần gửi mã xác thực.
    public string Email { get; set; } = string.Empty;
}

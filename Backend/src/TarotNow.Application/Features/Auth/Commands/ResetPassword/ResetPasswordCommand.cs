using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommand : IRequest<bool>
{
    public string Email { get; set; } = string.Empty;
    public string OtpCode { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

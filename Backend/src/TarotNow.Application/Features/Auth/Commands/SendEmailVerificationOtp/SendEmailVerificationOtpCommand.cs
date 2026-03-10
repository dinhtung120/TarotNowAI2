using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;

public class SendEmailVerificationOtpCommand : IRequest<bool>
{
    public string Email { get; set; } = string.Empty;
}

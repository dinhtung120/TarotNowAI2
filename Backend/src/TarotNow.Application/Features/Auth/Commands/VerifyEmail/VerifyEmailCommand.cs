using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.VerifyEmail;

public class VerifyEmailCommand : IRequest<bool>
{
    public string Email { get; set; } = string.Empty;
    public string OtpCode { get; set; } = string.Empty;
}

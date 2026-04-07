using FluentValidation;

namespace TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;

public class SendEmailVerificationOtpCommandValidator : AbstractValidator<SendEmailVerificationOtpCommand>
{
    public SendEmailVerificationOtpCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(254);
    }
}

using FluentValidation;

namespace TarotNow.Application.Features.Mfa.Commands.MfaVerify;

public class MfaVerifyCommandValidator : AbstractValidator<MfaVerifyCommand>
{
    public MfaVerifyCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .Length(6, 64);
    }
}

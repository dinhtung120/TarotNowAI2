using FluentValidation;

namespace TarotNow.Application.Features.Mfa.Commands.MfaSetup;

public class MfaSetupCommandValidator : AbstractValidator<MfaSetupCommand>
{
    public MfaSetupCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}

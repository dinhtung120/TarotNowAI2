using FluentValidation;

namespace TarotNow.Application.Features.Mfa.Commands.MfaChallenge;

public class MfaChallengeCommandValidator : AbstractValidator<MfaChallengeCommand>
{
    public MfaChallengeCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .Length(6, 64);
    }
}

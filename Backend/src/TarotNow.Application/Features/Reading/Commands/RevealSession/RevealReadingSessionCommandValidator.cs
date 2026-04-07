using FluentValidation;

namespace TarotNow.Application.Features.Reading.Commands.RevealSession;

public class RevealReadingSessionCommandValidator : AbstractValidator<RevealReadingSessionCommand>
{
    public RevealReadingSessionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.SessionId)
            .NotEmpty();
    }
}

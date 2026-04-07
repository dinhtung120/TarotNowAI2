using FluentValidation;

namespace TarotNow.Application.Features.Gamification.Commands;

public class SetActiveTitleCommandValidator : AbstractValidator<SetActiveTitleCommand>
{
    public SetActiveTitleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.TitleCode)
            .MaximumLength(100);
    }
}

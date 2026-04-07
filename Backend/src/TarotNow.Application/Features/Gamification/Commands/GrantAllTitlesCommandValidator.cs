using FluentValidation;

namespace TarotNow.Application.Features.Gamification.Commands;

public class GrantAllTitlesCommandValidator : AbstractValidator<GrantAllTitlesCommand>
{
    public GrantAllTitlesCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}

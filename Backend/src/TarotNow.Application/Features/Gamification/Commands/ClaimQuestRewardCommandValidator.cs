using FluentValidation;

namespace TarotNow.Application.Features.Gamification.Commands;

public class ClaimQuestRewardCommandValidator : AbstractValidator<ClaimQuestRewardCommand>
{
    public ClaimQuestRewardCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.QuestCode)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.PeriodKey)
            .NotEmpty()
            .MaximumLength(100);
    }
}

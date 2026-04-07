using FluentValidation;
using TarotNow.Application.Features.Gamification.Commands;

namespace TarotNow.Application.Features.Gamification.Validators;

public class UpsertQuestDefinitionValidator : AbstractValidator<UpsertQuestDefinitionCommand>
{
    public UpsertQuestDefinitionValidator()
    {
        RuleFor(x => x.Quest.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Quest.TitleVi).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Quest.Target).GreaterThan(0);
        RuleFor(x => x.Quest.Rewards).NotEmpty().WithMessage("Nhiệm vụ phải có ít nhất một phần thưởng.");
    }
}

public class UpsertAchievementDefinitionValidator : AbstractValidator<UpsertAchievementDefinitionCommand>
{
    public UpsertAchievementDefinitionValidator()
    {
        RuleFor(x => x.Achievement.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Achievement.TitleVi).NotEmpty().MaximumLength(200);
    }
}

public class UpsertTitleDefinitionValidator : AbstractValidator<UpsertTitleDefinitionCommand>
{
    public UpsertTitleDefinitionValidator()
    {
        RuleFor(x => x.Title.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Title.NameVi).NotEmpty().MaximumLength(100);
    }
}

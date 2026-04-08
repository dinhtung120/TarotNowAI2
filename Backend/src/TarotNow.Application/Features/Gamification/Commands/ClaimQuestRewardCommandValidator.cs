using FluentValidation;

namespace TarotNow.Application.Features.Gamification.Commands;

// Validator đầu vào cho command claim quest reward.
public class ClaimQuestRewardCommandValidator : AbstractValidator<ClaimQuestRewardCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho ClaimQuestRewardCommand.
    /// Luồng xử lý: bắt buộc UserId/QuestCode/PeriodKey và giới hạn độ dài các mã.
    /// </summary>
    public ClaimQuestRewardCommandValidator()
    {
        // UserId bắt buộc để định vị tiến độ quest.
        RuleFor(x => x.UserId)
            .NotEmpty();

        // QuestCode bắt buộc và giới hạn độ dài.
        RuleFor(x => x.QuestCode)
            .NotEmpty()
            .MaximumLength(100);

        // PeriodKey bắt buộc để claim đúng chu kỳ quest.
        RuleFor(x => x.PeriodKey)
            .NotEmpty()
            .MaximumLength(100);
    }
}

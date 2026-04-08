using FluentValidation;
using TarotNow.Application.Features.Gamification.Commands;

namespace TarotNow.Application.Features.Gamification.Validators;

// Validator command upsert quest definition cho admin.
public class UpsertQuestDefinitionValidator : AbstractValidator<UpsertQuestDefinitionCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho UpsertQuestDefinitionCommand.
    /// Luồng xử lý: kiểm tra code/title/target và yêu cầu quest có ít nhất một reward.
    /// </summary>
    public UpsertQuestDefinitionValidator()
    {
        // Code quest bắt buộc và giới hạn độ dài.
        RuleFor(x => x.Quest.Code).NotEmpty().MaximumLength(50);
        // Tiêu đề tiếng Việt bắt buộc cho hiển thị quản trị.
        RuleFor(x => x.Quest.TitleVi).NotEmpty().MaximumLength(200);
        // Target phải lớn hơn 0.
        RuleFor(x => x.Quest.Target).GreaterThan(0);
        // Quest phải có ít nhất một phần thưởng hợp lệ.
        RuleFor(x => x.Quest.Rewards).NotEmpty().WithMessage("Nhiệm vụ phải có ít nhất một phần thưởng.");
    }
}

// Validator command upsert achievement definition cho admin.
public class UpsertAchievementDefinitionValidator : AbstractValidator<UpsertAchievementDefinitionCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho UpsertAchievementDefinitionCommand.
    /// Luồng xử lý: kiểm tra code achievement và title tiếng Việt bắt buộc.
    /// </summary>
    public UpsertAchievementDefinitionValidator()
    {
        RuleFor(x => x.Achievement.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Achievement.TitleVi).NotEmpty().MaximumLength(200);
    }
}

// Validator command upsert title definition cho admin.
public class UpsertTitleDefinitionValidator : AbstractValidator<UpsertTitleDefinitionCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho UpsertTitleDefinitionCommand.
    /// Luồng xử lý: kiểm tra code title và name tiếng Việt bắt buộc.
    /// </summary>
    public UpsertTitleDefinitionValidator()
    {
        RuleFor(x => x.Title.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Title.NameVi).NotEmpty().MaximumLength(100);
    }
}

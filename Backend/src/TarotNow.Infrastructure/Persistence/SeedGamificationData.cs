using System.Collections.Generic;
using System.Threading.Tasks;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;
using MongoDB.Driver;

namespace TarotNow.Infrastructure.Persistence;

public static class SeedGamificationData
{
    public static async Task SeedAsync(MongoDbContext context)
    {
        await SeedQuestsAsync(context);
        await SeedAchievementsAsync(context);
        await SeedTitlesAsync(context);
    }

    private static async Task SeedQuestsAsync(MongoDbContext context)
    {
        if (await context.Quests.CountDocumentsAsync(_ => true) > 0) return;
        await context.Quests.InsertManyAsync(new List<QuestDefinitionDocument>
        {
            new()
            {
                Code = "daily_1_reading",
                TitleVi = "Rút Thẻ Tarot Hàng Ngày",
                DescriptionVi = "Hoàn thành 1 lần rút thẻ Tarot.",
                QuestType = QuestType.Daily,
                TriggerEvent = "reading_completed",
                Target = 1,
                Rewards = new List<QuestRewardItem> { new() { Type = QuestRewardType.Gold, Amount = 50 } }
            },
            new()
            {
                Code = "daily_checkin",
                TitleVi = "Điểm danh hàng ngày",
                DescriptionVi = "Đăng nhập và điểm danh nhận thưởng.",
                QuestType = QuestType.Daily,
                TriggerEvent = "daily_checkin",
                Target = 1,
                Rewards = new List<QuestRewardItem> { new() { Type = QuestRewardType.Gold, Amount = 20 } }
            }
        });
    }

    private static async Task SeedAchievementsAsync(MongoDbContext context)
    {
        if (await context.Achievements.CountDocumentsAsync(_ => true) > 0) return;
        await context.Achievements.InsertManyAsync(new List<AchievementDefinitionDocument>
        {
            new()
            {
                Code = "first_reading",
                TitleVi = "Lần Đầu Giải Bài",
                DescriptionVi = "Hoàn thành phiên giải bài đầu tiên của bạn.",
                Icon = "ach-first-reading",
                GrantsTitleCode = "novice_reader"
            }
        });
    }

    private static async Task SeedTitlesAsync(MongoDbContext context)
    {
        if (await context.Titles.CountDocumentsAsync(_ => true) > 0) return;
        await context.Titles.InsertManyAsync(new List<TitleDefinitionDocument>
        {
            new()
            {
                Code = "novice_reader",
                NameVi = "Người Tập Sự",
                DescriptionVi = "Danh hiệu dành cho người mới bắt đầu con đường tâm linh.",
                Rarity = "Common"
            }
        });
    }
}

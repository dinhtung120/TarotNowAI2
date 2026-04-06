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
        // 1. Quests Seed
        var count = await context.Quests.CountDocumentsAsync(_ => true);
        if (count == 0)
        {
            var quests = new List<QuestDefinitionDocument>
            {
                new QuestDefinitionDocument
                {
                    Code = "daily_1_reading",
                    TitleVi = "Rút Thẻ Tarot Hàng Ngày",
                    DescriptionVi = "Hoàn thành 1 lần rút thẻ Tarot.",
                    QuestType = QuestType.Daily,
                    TriggerEvent = "reading_completed",
                    Target = 1,
                    Rewards = new List<QuestRewardItem> { new QuestRewardItem { Type = QuestRewardType.Gold, Amount = 50 } }
                },
                new QuestDefinitionDocument
                {
                    Code = "daily_checkin",
                    TitleVi = "Điểm danh hàng ngày",
                    DescriptionVi = "Đăng nhập và điểm danh nhận thưởng.",
                    QuestType = QuestType.Daily,
                    TriggerEvent = "daily_checkin",
                    Target = 1,
                    Rewards = new List<QuestRewardItem> { new QuestRewardItem { Type = QuestRewardType.Gold, Amount = 20 } }
                }
            };
            await context.Quests.InsertManyAsync(quests);
        }

        // 2. Achievements Seed
        count = await context.Achievements.CountDocumentsAsync(_ => true);
        if (count == 0)
        {
            var achs = new List<AchievementDefinitionDocument>
            {
                new AchievementDefinitionDocument
                {
                    Code = "first_reading",
                    TitleVi = "Lần Đầu Giải Bài",
                    DescriptionVi = "Hoàn thành phiên giải bài đầu tiên của bạn.",
                    Icon = "ach-first-reading",
                    GrantsTitleCode = "novice_reader"
                }
            };
            await context.Achievements.InsertManyAsync(achs);
        }

        // 3. Titles Seed
        count = await context.Titles.CountDocumentsAsync(_ => true);
        if (count == 0)
        {
            var titles = new List<TitleDefinitionDocument>
            {
                new TitleDefinitionDocument
                {
                    Code = "novice_reader",
                    NameVi = "Người Tập Sự",
                    DescriptionVi = "Danh hiệu dành cho người mới bắt đầu con đường tâm linh.",
                    Rarity = "Common"
                }
            };
            await context.Titles.InsertManyAsync(titles);
        }
    }
}

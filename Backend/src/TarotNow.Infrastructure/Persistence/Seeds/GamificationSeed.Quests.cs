using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Seeds;

public static partial class GamificationSeed
{
    private static async Task SeedQuestDefinitionsAsync(MongoDbContext context)
    {
        await context.Quests.DeleteManyAsync(x => x.Code == "daily_checkin");

        foreach (var quest in BuildInitialQuests())
        {
            var filter = Builders<QuestDefinitionDocument>.Filter.Eq(x => x.Code, quest.Code);
            var update = Builders<QuestDefinitionDocument>.Update
                .Set(x => x.TitleVi, quest.TitleVi)
                .Set(x => x.TitleEn, quest.TitleEn)
                .Set(x => x.DescriptionVi, quest.DescriptionVi)
                .Set(x => x.DescriptionEn, quest.DescriptionEn)
                .Set(x => x.QuestType, quest.QuestType)
                .Set(x => x.TriggerEvent, quest.TriggerEvent)
                .Set(x => x.Target, quest.Target)
                .Set(x => x.Rewards, quest.Rewards)
                .Set(x => x.IsActive, true);

            await context.Quests.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }
    }

    private static List<QuestDefinitionDocument> BuildInitialQuests() =>
        new()
        {
            new()
            {
                Code = "daily_reading_1",
                TitleVi = "Bói Bài Mỗi Ngày",
                TitleEn = "Daily Reading",
                DescriptionVi = "Hoàn thành 1 trải bài bất kỳ trong ngày hôm nay.",
                DescriptionEn = "Complete 1 tarot reading today.",
                QuestType = "daily",
                TriggerEvent = "ReadingCompleted",
                Target = 1,
                Rewards = new List<QuestRewardItem> { new() { Type = "gold", Amount = 100 } }
            },
            new()
            {
                Code = "weekly_reading_7",
                TitleVi = "Học Giả Tarot",
                TitleEn = "Tarot Scholar",
                DescriptionVi = "Hoàn thành tổng cộng 7 trải bài trong tuần này.",
                DescriptionEn = "Complete a total of 7 readings this week.",
                QuestType = "weekly",
                TriggerEvent = "ReadingCompleted",
                Target = 7,
                Rewards = new List<QuestRewardItem> { new() { Type = "gold", Amount = 1000 }, new() { Type = "diamond", Amount = 5 } }
            },
            new()
            {
                Code = "daily_post_1",
                TitleVi = "Chia Sẻ Câu Chuyện",
                TitleEn = "Share a Story",
                DescriptionVi = "Đăng một bài viết mới lên cộng đồng hôm nay để nhận thưởng.",
                DescriptionEn = "Post a new story to the community today to get reward.",
                QuestType = "daily",
                TriggerEvent = "PostCreated",
                Target = 1,
                Rewards = new List<QuestRewardItem> { new() { Type = "gold", Amount = 150 } }
            }
        };
}

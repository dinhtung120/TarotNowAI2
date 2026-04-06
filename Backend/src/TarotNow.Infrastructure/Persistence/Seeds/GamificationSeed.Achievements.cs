using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Seeds;

public static partial class GamificationSeed
{
    private static async Task SeedAchievementDefinitionsAsync(MongoDbContext context)
    {
        foreach (var achievement in BuildInitialAchievements())
        {
            var filter = Builders<AchievementDefinitionDocument>.Filter.Eq(x => x.Code, achievement.Code);
            var update = Builders<AchievementDefinitionDocument>.Update
                .Set(x => x.TitleVi, achievement.TitleVi)
                .Set(x => x.TitleEn, achievement.TitleEn)
                .Set(x => x.DescriptionVi, achievement.DescriptionVi)
                .Set(x => x.DescriptionEn, achievement.DescriptionEn)
                .Set(x => x.GrantsTitleCode, achievement.GrantsTitleCode);

            await context.Achievements.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }
    }

    private static List<AchievementDefinitionDocument> BuildInitialAchievements()
    {
        return new List<AchievementDefinitionDocument>
        {
            new()
            {
                Code = "first_reading",
                TitleVi = "Bước Chân Đầu Tiên",
                TitleEn = "First Steps",
                DescriptionVi = "Hoàn thành trải bài tarot đầu tiên của bạn.",
                DescriptionEn = "Complete your very first tarot reading."
            },
            new()
            {
                Code = "streak_7",
                TitleVi = "Kiên Trì Bền Bỉ",
                TitleEn = "Persistent Spirit",
                DescriptionVi = "Đạt chuỗi điểm danh 7 ngày liên tiếp.",
                DescriptionEn = "Reach a 7-day check-in streak.",
                GrantsTitleCode = "title_persistent"
            }
        };
    }
}

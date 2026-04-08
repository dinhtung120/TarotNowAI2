using System.Threading.Tasks;

namespace TarotNow.Infrastructure.Persistence.Seeds;

// Seed entrypoint cho toàn bộ dữ liệu gamification.
public static partial class GamificationSeed
{
    /// <summary>
    /// Chạy seed dữ liệu gamification.
    /// Luồng xử lý: seed quest trước, sau đó achievements và titles để đảm bảo phụ thuộc reward được đáp ứng.
    /// </summary>
    public static async Task SeedAsync(MongoDbContext context)
    {
        await SeedQuestDefinitionsAsync(context);
        await SeedAchievementDefinitionsAsync(context);
        await SeedTitleDefinitionsAsync(context);
    }
}

using System.Threading.Tasks;

namespace TarotNow.Infrastructure.Persistence.Seeds;

public static partial class GamificationSeed
{
    public static async Task SeedAsync(MongoDbContext context)
    {
        await SeedQuestDefinitionsAsync(context);
        await SeedAchievementDefinitionsAsync(context);
        await SeedTitleDefinitionsAsync(context);
    }
}

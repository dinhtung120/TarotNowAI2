using MongoDB.Driver;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private void EnsureGamificationIndexes()
    {
        EnsureQuestIndexes();
        EnsureAchievementIndexes();
        EnsureTitleIndexes();
        EnsureLeaderboardIndexes();
    }
}

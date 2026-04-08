using MongoDB.Driver;

namespace TarotNow.Infrastructure.Persistence;

// Khối điều phối tạo index cho toàn bộ module gamification.
public partial class MongoDbContext
{
    /// <summary>
    /// Bảo đảm index cho các collection gamification.
    /// Luồng xử lý: lần lượt tạo index quest, achievement, title và leaderboard theo thứ tự phụ thuộc dữ liệu.
    /// </summary>
    private void EnsureGamificationIndexes()
    {
        EnsureQuestIndexes();
        EnsureAchievementIndexes();
        EnsureTitleIndexes();
        EnsureLeaderboardIndexes();
    }
}

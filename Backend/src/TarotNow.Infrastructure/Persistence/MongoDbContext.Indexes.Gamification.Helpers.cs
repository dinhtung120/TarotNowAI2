using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

// Khối helper cấu hình index cho các collection gamification.
public partial class MongoDbContext
{
    /// <summary>
    /// Tạo index cho quest definitions và quest progress.
    /// Luồng xử lý: đảm bảo mã quest duy nhất, chặn trùng progress theo user-quest-period và TTL dọn lịch sử cũ.
    /// </summary>
    private void EnsureQuestIndexes()
    {
        Quests.Indexes.CreateOne(new CreateIndexModel<QuestDefinitionDocument>(
            Builders<QuestDefinitionDocument>.IndexKeys.Ascending(x => x.Code),
            new CreateIndexOptions { Unique = true }));
        // Code quest là khóa nghiệp vụ, cần unique để đồng bộ giữa seed và runtime.

        var progressIndexes = new[]
        {
            new CreateIndexModel<QuestProgressDocument>(
                Builders<QuestProgressDocument>.IndexKeys
                    .Ascending(x => x.UserId)
                    .Ascending(x => x.QuestCode)
                    .Ascending(x => x.PeriodKey),
                new CreateIndexOptions { Unique = true }),
            new CreateIndexModel<QuestProgressDocument>(
                Builders<QuestProgressDocument>.IndexKeys.Ascending(x => x.CreatedAt),
                new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(90) })
        };
        QuestProgress.Indexes.CreateMany(progressIndexes);
        // Unique index giữ idempotency cập nhật tiến độ; TTL giảm phình dữ liệu lịch sử.
    }

    /// <summary>
    /// Tạo index cho achievements và user achievements.
    /// Luồng xử lý: giữ định nghĩa achievement duy nhất theo code và tránh cấp trùng achievement cho cùng user.
    /// </summary>
    private void EnsureAchievementIndexes()
    {
        Achievements.Indexes.CreateOne(new CreateIndexModel<AchievementDefinitionDocument>(
            Builders<AchievementDefinitionDocument>.IndexKeys.Ascending(x => x.Code),
            new CreateIndexOptions { Unique = true }));

        UserAchievements.Indexes.CreateOne(new CreateIndexModel<UserAchievementDocument>(
            Builders<UserAchievementDocument>.IndexKeys
                .Ascending(x => x.UserId)
                .Ascending(x => x.AchievementCode),
            new CreateIndexOptions { Unique = true }));
        // Business rule: một achievement chỉ unlock một lần cho mỗi user.
    }

    /// <summary>
    /// Tạo index cho titles và user titles.
    /// Luồng xử lý: bảo đảm title definition theo code là duy nhất và chống gán trùng title cho cùng user.
    /// </summary>
    private void EnsureTitleIndexes()
    {
        Titles.Indexes.CreateOne(new CreateIndexModel<TitleDefinitionDocument>(
            Builders<TitleDefinitionDocument>.IndexKeys.Ascending(x => x.Code),
            new CreateIndexOptions { Unique = true }));

        UserTitles.Indexes.CreateOne(new CreateIndexModel<UserTitleDocument>(
            Builders<UserTitleDocument>.IndexKeys
                .Ascending(x => x.UserId)
                .Ascending(x => x.TitleCode),
            new CreateIndexOptions { Unique = true }));
        // Giữ tính nhất quán dữ liệu danh hiệu giữa service và repository.
    }

    /// <summary>
    /// Tạo index cho leaderboard entries và snapshots.
    /// Luồng xử lý: unique theo user-track-period, index xếp hạng theo score và unique snapshot từng kỳ.
    /// </summary>
    private void EnsureLeaderboardIndexes()
    {
        var entryIndexes = new[]
        {
            new CreateIndexModel<LeaderboardEntryDocument>(
                Builders<LeaderboardEntryDocument>.IndexKeys
                    .Ascending(x => x.UserId)
                    .Ascending(x => x.ScoreTrack)
                    .Ascending(x => x.PeriodKey),
                new CreateIndexOptions { Unique = true }),
            new CreateIndexModel<LeaderboardEntryDocument>(
                Builders<LeaderboardEntryDocument>.IndexKeys
                    .Ascending(x => x.ScoreTrack)
                    .Ascending(x => x.PeriodKey)
                    .Descending(x => x.Score))
        };
        LeaderboardEntries.Indexes.CreateMany(entryIndexes);
        // Index score desc tối ưu truy vấn top N theo từng bảng xếp hạng.

        LeaderboardSnapshots.Indexes.CreateOne(new CreateIndexModel<LeaderboardSnapshotDocument>(
            Builders<LeaderboardSnapshotDocument>.IndexKeys
                .Ascending(x => x.ScoreTrack)
                .Ascending(x => x.PeriodKey),
            new CreateIndexOptions { Unique = true }));
        // Mỗi track + period chỉ có một snapshot cuối kỳ để tránh lệch dữ liệu chốt.
    }
}

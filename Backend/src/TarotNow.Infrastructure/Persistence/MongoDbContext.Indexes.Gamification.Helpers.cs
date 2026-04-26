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
        SafeCreateIndex(Quests, new CreateIndexModel<QuestDefinitionDocument>(
            Builders<QuestDefinitionDocument>.IndexKeys.Ascending(x => x.Code),
            new CreateIndexOptions { Unique = true, Name = "ux_quests_code" }));
        // Code quest là khóa nghiệp vụ, cần unique để đồng bộ giữa seed và runtime.

        SafeCreateIndex(QuestProgress, new CreateIndexModel<QuestProgressDocument>(
            Builders<QuestProgressDocument>.IndexKeys
                .Ascending(x => x.UserId)
                .Ascending(x => x.QuestCode)
                .Ascending(x => x.PeriodKey),
            new CreateIndexOptions { Unique = true, Name = "ux_questprogress_userid_questcode_periodkey" }));
        SafeCreateIndex(QuestProgress, new CreateIndexModel<QuestProgressDocument>(
            Builders<QuestProgressDocument>.IndexKeys.Ascending(x => x.CreatedAt),
            new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(90), Name = "idx_questprogress_createdat_ttl_90d" }));
        // Unique index giữ idempotency cập nhật tiến độ; TTL giảm phình dữ liệu lịch sử.
    }

    /// <summary>
    /// Tạo index cho achievements và user achievements.
    /// Luồng xử lý: giữ định nghĩa achievement duy nhất theo code và tránh cấp trùng achievement cho cùng user.
    /// </summary>
    private void EnsureAchievementIndexes()
    {
        SafeCreateIndex(Achievements, new CreateIndexModel<AchievementDefinitionDocument>(
            Builders<AchievementDefinitionDocument>.IndexKeys.Ascending(x => x.Code),
            new CreateIndexOptions { Unique = true, Name = "ux_achievements_code" }));

        SafeCreateIndex(UserAchievements, new CreateIndexModel<UserAchievementDocument>(
            Builders<UserAchievementDocument>.IndexKeys
                .Ascending(x => x.UserId)
                .Ascending(x => x.AchievementCode),
            new CreateIndexOptions { Unique = true, Name = "ux_userachievements_userid_achievementcode" }));
        // Business rule: một achievement chỉ unlock một lần cho mỗi user.
    }

    /// <summary>
    /// Tạo index cho titles và user titles.
    /// Luồng xử lý: bảo đảm title definition theo code là duy nhất và chống gán trùng title cho cùng user.
    /// </summary>
    private void EnsureTitleIndexes()
    {
        SafeCreateIndex(Titles, new CreateIndexModel<TitleDefinitionDocument>(
            Builders<TitleDefinitionDocument>.IndexKeys.Ascending(x => x.Code),
            new CreateIndexOptions { Unique = true, Name = "ux_titles_code" }));

        SafeCreateIndex(UserTitles, new CreateIndexModel<UserTitleDocument>(
            Builders<UserTitleDocument>.IndexKeys
                .Ascending(x => x.UserId)
                .Ascending(x => x.TitleCode),
            new CreateIndexOptions { Unique = true, Name = "ux_usertitles_userid_titlecode" }));
        // Giữ tính nhất quán dữ liệu danh hiệu giữa service và repository.
    }

    /// <summary>
    /// Tạo index cho leaderboard entries và snapshots.
    /// Luồng xử lý: unique theo user-track-period, index xếp hạng theo score và unique snapshot từng kỳ.
    /// </summary>
    private void EnsureLeaderboardIndexes()
    {
        SafeCreateIndex(LeaderboardEntries, new CreateIndexModel<LeaderboardEntryDocument>(
            Builders<LeaderboardEntryDocument>.IndexKeys
                .Ascending(x => x.UserId)
                .Ascending(x => x.ScoreTrack)
                .Ascending(x => x.PeriodKey),
            new CreateIndexOptions { Unique = true, Name = "ux_leaderboardentries_userid_track_period" }));
        SafeCreateIndex(LeaderboardEntries, new CreateIndexModel<LeaderboardEntryDocument>(
            Builders<LeaderboardEntryDocument>.IndexKeys
                .Ascending(x => x.ScoreTrack)
                .Ascending(x => x.PeriodKey)
                .Descending(x => x.Score),
            new CreateIndexOptions { Name = "idx_leaderboardentries_track_period_score_desc" }));
        // Index score desc tối ưu truy vấn top N theo từng bảng xếp hạng.

        SafeCreateIndex(LeaderboardSnapshots, new CreateIndexModel<LeaderboardSnapshotDocument>(
            Builders<LeaderboardSnapshotDocument>.IndexKeys
                .Ascending(x => x.ScoreTrack)
                .Ascending(x => x.PeriodKey),
            new CreateIndexOptions { Unique = true, Name = "ux_leaderboardsnapshots_track_period" }));
        // Mỗi track + period chỉ có một snapshot cuối kỳ để tránh lệch dữ liệu chốt.
    }
}

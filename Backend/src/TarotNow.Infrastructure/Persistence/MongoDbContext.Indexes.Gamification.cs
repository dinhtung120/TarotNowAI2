using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private void EnsureGamificationIndexes()
    {
        // 1. Quests: Unique code
        var questCodeIndex = new CreateIndexModel<QuestDefinitionDocument>(
            Builders<QuestDefinitionDocument>.IndexKeys.Ascending(x => x.Code),
            new CreateIndexOptions { Unique = true });
        Quests.Indexes.CreateOne(questCodeIndex);

        // 2. Quest Progress: Unique (UserId, QuestCode, PeriodKey)
        var qpUniqueIndex = new CreateIndexModel<QuestProgressDocument>(
            Builders<QuestProgressDocument>.IndexKeys
                .Ascending(x => x.UserId)
                .Ascending(x => x.QuestCode)
                .Ascending(x => x.PeriodKey),
            new CreateIndexOptions { Unique = true });
        
        // TTL: Auto-delete quest progress after 90 days to keep DB healthy (Audit Scalability #1)
        var qpTtlIndex = new CreateIndexModel<QuestProgressDocument>(
            Builders<QuestProgressDocument>.IndexKeys.Ascending(x => x.CreatedAt),
            new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(90) });

        QuestProgress.Indexes.CreateMany(new[] { qpUniqueIndex, qpTtlIndex });

        // 3. Achievements: Unique code
        var achievementCodeIndex = new CreateIndexModel<AchievementDefinitionDocument>(
            Builders<AchievementDefinitionDocument>.IndexKeys.Ascending(x => x.Code),
            new CreateIndexOptions { Unique = true });
        Achievements.Indexes.CreateOne(achievementCodeIndex);

        // 4. User Achievements: Unique (UserId, AchievementCode)
        var userAchievementUniqueIndex = new CreateIndexModel<UserAchievementDocument>(
            Builders<UserAchievementDocument>.IndexKeys
                .Ascending(x => x.UserId)
                .Ascending(x => x.AchievementCode),
            new CreateIndexOptions { Unique = true });
        UserAchievements.Indexes.CreateOne(userAchievementUniqueIndex);

        // 5. Titles: Unique code
        var titleCodeIndex = new CreateIndexModel<TitleDefinitionDocument>(
            Builders<TitleDefinitionDocument>.IndexKeys.Ascending(x => x.Code),
            new CreateIndexOptions { Unique = true });
        Titles.Indexes.CreateOne(titleCodeIndex);

        // 6. User Titles: Unique (UserId, TitleCode)
        var userTitleUniqueIndex = new CreateIndexModel<UserTitleDocument>(
            Builders<UserTitleDocument>.IndexKeys
                .Ascending(x => x.UserId)
                .Ascending(x => x.TitleCode),
            new CreateIndexOptions { Unique = true });
        UserTitles.Indexes.CreateOne(userTitleUniqueIndex);

        // 7. Leaderboard Entries: Unique (UserId, ScoreTrack, PeriodKey)
        var entryUniqueIndex = new CreateIndexModel<LeaderboardEntryDocument>(
            Builders<LeaderboardEntryDocument>.IndexKeys
                .Ascending(x => x.UserId)
                .Ascending(x => x.ScoreTrack)
                .Ascending(x => x.PeriodKey),
            new CreateIndexOptions { Unique = true });
        
        // Index for querying top ranks: (ScoreTrack, PeriodKey, Score DESC)
        var entryRankIndex = new CreateIndexModel<LeaderboardEntryDocument>(
            Builders<LeaderboardEntryDocument>.IndexKeys
                .Ascending(x => x.ScoreTrack)
                .Ascending(x => x.PeriodKey)
                .Descending(x => x.Score));
        
        LeaderboardEntries.Indexes.CreateMany(new[] { entryUniqueIndex, entryRankIndex });

        // 8. Leaderboard Snapshots: Unique (ScoreTrack, PeriodKey)
        var snapshotUniqueIndex = new CreateIndexModel<LeaderboardSnapshotDocument>(
            Builders<LeaderboardSnapshotDocument>.IndexKeys
                .Ascending(x => x.ScoreTrack)
                .Ascending(x => x.PeriodKey),
            new CreateIndexOptions { Unique = true });
        LeaderboardSnapshots.Indexes.CreateOne(snapshotUniqueIndex);
    }
}

using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private void EnsureQuestIndexes()
    {
        Quests.Indexes.CreateOne(new CreateIndexModel<QuestDefinitionDocument>(
            Builders<QuestDefinitionDocument>.IndexKeys.Ascending(x => x.Code),
            new CreateIndexOptions { Unique = true }));

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
    }

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
    }

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
    }

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

        LeaderboardSnapshots.Indexes.CreateOne(new CreateIndexModel<LeaderboardSnapshotDocument>(
            Builders<LeaderboardSnapshotDocument>.IndexKeys
                .Ascending(x => x.ScoreTrack)
                .Ascending(x => x.PeriodKey),
            new CreateIndexOptions { Unique = true }));
    }
}

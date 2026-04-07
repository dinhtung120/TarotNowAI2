using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<MongoDbContext> _logger;

    public MongoDbContext(IMongoDatabase database, ILogger<MongoDbContext> logger)
    {
        _database = database;
        _logger = logger;

        try
        {
            EnsureIndexes();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MongoDB] Failed to ensure indexes at startup.");
        }
    }

    public IMongoCollection<CardCatalogDocument> Cards
        => _database.GetCollection<CardCatalogDocument>("cards_catalog");

    public IMongoCollection<UserCollectionDocument> UserCollections
        => _database.GetCollection<UserCollectionDocument>("user_collections");

    public IMongoCollection<ReadingSessionDocument> ReadingSessions
        => _database.GetCollection<ReadingSessionDocument>("reading_sessions");

    public IMongoCollection<DailyCheckinDocument> DailyCheckins
        => _database.GetCollection<DailyCheckinDocument>("daily_checkins");

    public IMongoCollection<AiProviderLogDocument> AiProviderLogs
        => _database.GetCollection<AiProviderLogDocument>("ai_provider_logs");

    public IMongoCollection<NotificationDocument> Notifications
        => _database.GetCollection<NotificationDocument>("notifications");

    public IMongoCollection<ReaderRequestDocument> ReaderRequests
        => _database.GetCollection<ReaderRequestDocument>("reader_requests");

    public IMongoCollection<ReaderProfileDocument> ReaderProfiles
        => _database.GetCollection<ReaderProfileDocument>("reader_profiles");

    public IMongoCollection<ConversationDocument> Conversations
        => _database.GetCollection<ConversationDocument>("conversations");

    public IMongoCollection<ChatMessageDocument> ChatMessages
        => _database.GetCollection<ChatMessageDocument>("chat_messages");

    public IMongoCollection<ReportDocument> Reports
        => _database.GetCollection<ReportDocument>("reports");

    public IMongoCollection<CommunityPostDocument> CommunityPosts
        => _database.GetCollection<CommunityPostDocument>("community_posts");

    public IMongoCollection<CommunityReactionDocument> CommunityReactions
        => _database.GetCollection<CommunityReactionDocument>("community_reactions");

    public IMongoCollection<CommunityCommentDocument> CommunityComments
        => _database.GetCollection<CommunityCommentDocument>("community_comments");

    public IMongoCollection<CallSessionDocument> CallSessions
        => _database.GetCollection<CallSessionDocument>("call_sessions");

    
    public IMongoCollection<QuestDefinitionDocument> Quests
        => _database.GetCollection<QuestDefinitionDocument>("quests");

    public IMongoCollection<QuestProgressDocument> QuestProgress
        => _database.GetCollection<QuestProgressDocument>("quest_progress");

    public IMongoCollection<AchievementDefinitionDocument> Achievements
        => _database.GetCollection<AchievementDefinitionDocument>("achievements");

    public IMongoCollection<UserAchievementDocument> UserAchievements
        => _database.GetCollection<UserAchievementDocument>("user_achievements");

    public IMongoCollection<TitleDefinitionDocument> Titles
        => _database.GetCollection<TitleDefinitionDocument>("titles");

    public IMongoCollection<UserTitleDocument> UserTitles
        => _database.GetCollection<UserTitleDocument>("user_titles");

    public IMongoCollection<LeaderboardEntryDocument> LeaderboardEntries
        => _database.GetCollection<LeaderboardEntryDocument>("leaderboard_entries");

    public IMongoCollection<LeaderboardSnapshotDocument> LeaderboardSnapshots
        => _database.GetCollection<LeaderboardSnapshotDocument>("leaderboard_snapshots");

    
    public IMongoCollection<GachaLogDocument> GachaLogs
        => _database.GetCollection<GachaLogDocument>("gacha_logs");
}

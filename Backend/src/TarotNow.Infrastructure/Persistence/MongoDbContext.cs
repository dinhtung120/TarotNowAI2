using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

// MongoDbContext gom toàn bộ collection và bootstrap index khi startup.
public partial class MongoDbContext
{
    // Database Mongo chính được inject từ DI.
    private readonly IMongoDatabase _database;
    // Logger dùng theo dõi lỗi khi tạo index hoặc thao tác hạ tầng.
    private readonly ILogger<MongoDbContext> _logger;
    private readonly IHostEnvironment _hostEnvironment;

    /// <summary>
    /// Khởi tạo MongoDbContext và chạy bootstrap index.
    /// Luồng xử lý: giữ tham chiếu database/logger, sau đó gọi EnsureIndexes trong try-catch để tránh crash startup.
    /// </summary>
    public MongoDbContext(
        IMongoDatabase database,
        ILogger<MongoDbContext> logger,
        IHostEnvironment hostEnvironment)
    {
        _database = database;
        _logger = logger;
        _hostEnvironment = hostEnvironment;

        try
        {
            EnsureIndexes();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MongoDB] Failed to ensure indexes at startup. Environment={EnvironmentName}", _hostEnvironment.EnvironmentName);
            if (!_hostEnvironment.IsDevelopment())
            {
                throw new InvalidOperationException("MongoDB index bootstrap failed on non-development environment.", ex);
            }

            // Development fallback: cho phép app chạy để dev có thể sửa dữ liệu/index nhanh tại local.
        }
    }

    // Collection danh mục lá bài.
    public IMongoCollection<CardCatalogDocument> Cards
        => _database.GetCollection<CardCatalogDocument>("cards_catalog");

    // Collection bộ sưu tập bài của người dùng.
    public IMongoCollection<UserCollectionDocument> UserCollections
        => _database.GetCollection<UserCollectionDocument>("user_collections");

    // Collection phiên đọc bài tarot.
    public IMongoCollection<ReadingSessionDocument> ReadingSessions
        => _database.GetCollection<ReadingSessionDocument>("reading_sessions");

    // Collection điểm danh hằng ngày.
    public IMongoCollection<DailyCheckinDocument> DailyCheckins
        => _database.GetCollection<DailyCheckinDocument>("daily_checkins");

    // Collection log gọi AI provider.
    public IMongoCollection<AiProviderLogDocument> AiProviderLogs
        => _database.GetCollection<AiProviderLogDocument>("ai_provider_logs");

    // Collection thông báo trong ứng dụng.
    public IMongoCollection<NotificationDocument> Notifications
        => _database.GetCollection<NotificationDocument>("notifications");

    // Collection refresh token cho auth rotation/replay detection.
    public IMongoCollection<RefreshTokenDocument> RefreshTokens
        => _database.GetCollection<RefreshTokenDocument>("refresh_tokens");

    // Collection yêu cầu trở thành reader.
    public IMongoCollection<ReaderRequestDocument> ReaderRequests
        => _database.GetCollection<ReaderRequestDocument>("reader_requests");

    // Collection hồ sơ reader đã/đang hoạt động.
    public IMongoCollection<ReaderProfileDocument> ReaderProfiles
        => _database.GetCollection<ReaderProfileDocument>("reader_profiles");

    // Collection hội thoại chat.
    public IMongoCollection<ConversationDocument> Conversations
        => _database.GetCollection<ConversationDocument>("conversations");

    // Collection tin nhắn chat.
    public IMongoCollection<ChatMessageDocument> ChatMessages
        => _database.GetCollection<ChatMessageDocument>("chat_messages");

    // Collection báo cáo vi phạm.
    public IMongoCollection<ReportDocument> Reports
        => _database.GetCollection<ReportDocument>("reports");

    // Collection bài viết cộng đồng.
    public IMongoCollection<CommunityPostDocument> CommunityPosts
        => _database.GetCollection<CommunityPostDocument>("community_posts");

    // Collection reaction bài viết cộng đồng.
    public IMongoCollection<CommunityReactionDocument> CommunityReactions
        => _database.GetCollection<CommunityReactionDocument>("community_reactions");

    // Collection bình luận cộng đồng.
    public IMongoCollection<CommunityCommentDocument> CommunityComments
        => _database.GetCollection<CommunityCommentDocument>("community_comments");

    // Collection session upload tạm cho token one-time.
    public IMongoCollection<UploadSessionDocument> UploadSessions
        => _database.GetCollection<UploadSessionDocument>("upload_sessions");

    // Collection asset ảnh cộng đồng để track uploaded/attached/orphaned/deleted.
    public IMongoCollection<CommunityMediaAssetDocument> CommunityMediaAssets
        => _database.GetCollection<CommunityMediaAssetDocument>("community_media_assets");

    // Collection định nghĩa quest.
    public IMongoCollection<QuestDefinitionDocument> Quests
        => _database.GetCollection<QuestDefinitionDocument>("quests");

    // Collection tiến độ quest theo người dùng.
    public IMongoCollection<QuestProgressDocument> QuestProgress
        => _database.GetCollection<QuestProgressDocument>("quest_progress");

    // Collection định nghĩa achievement.
    public IMongoCollection<AchievementDefinitionDocument> Achievements
        => _database.GetCollection<AchievementDefinitionDocument>("achievements");

    // Collection achievement đã mở khóa của người dùng.
    public IMongoCollection<UserAchievementDocument> UserAchievements
        => _database.GetCollection<UserAchievementDocument>("user_achievements");

    // Collection định nghĩa title.
    public IMongoCollection<TitleDefinitionDocument> Titles
        => _database.GetCollection<TitleDefinitionDocument>("titles");

    // Collection title người dùng sở hữu.
    public IMongoCollection<UserTitleDocument> UserTitles
        => _database.GetCollection<UserTitleDocument>("user_titles");

    // Collection entry bảng xếp hạng.
    public IMongoCollection<LeaderboardEntryDocument> LeaderboardEntries
        => _database.GetCollection<LeaderboardEntryDocument>("leaderboard_entries");

    // Collection snapshot chốt bảng xếp hạng theo kỳ.
    public IMongoCollection<LeaderboardSnapshotDocument> LeaderboardSnapshots
        => _database.GetCollection<LeaderboardSnapshotDocument>("leaderboard_snapshots");

}

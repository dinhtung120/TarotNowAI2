using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

/// <summary>
/// MongoDbContext — trung tâm quản lý tất cả MongoDB collections.
///
/// Tại sao cần class riêng thay vì inject IMongoDatabase trực tiếp?
/// 1. Encapsulation: Gom tất cả collection references vào 1 nơi.
/// 2. Index Management: Tạo TTL + unique indexes khi khởi động app.
/// 3. Testability: Dễ mock/stub cho unit tests.
/// 4. Convention: Tương tự EF Core DbContext — developer quen thuộc.
///
/// Lifecycle: Singleton — MongoClient thread-safe, chỉ cần 1 instance.
/// </summary>
public class MongoDbContext
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
            throw;
        }
    }

    // ======================================================================
    // COLLECTION PROPERTIES
    // Mỗi property trả về IMongoCollection<T> tương ứng với 1 collection.
    // Tên collection khớp với init.js + schema.md.
    // ======================================================================

    /// <summary>78 lá bài Tarot — dữ liệu tĩnh, seed từ seed_cards.js.</summary>
    public IMongoCollection<CardCatalogDocument> Cards
        => _database.GetCollection<CardCatalogDocument>("cards_catalog");

    /// <summary>Bộ sưu tập bài của user — level, EXP, stats.</summary>
    public IMongoCollection<UserCollectionDocument> UserCollections
        => _database.GetCollection<UserCollectionDocument>("user_collections");

    /// <summary>Phiên đọc bài + AI result + follow-ups.</summary>
    public IMongoCollection<ReadingSessionDocument> ReadingSessions
        => _database.GetCollection<ReadingSessionDocument>("reading_sessions");

    /// <summary>Log gọi AI provider — TTL 90 ngày.</summary>
    public IMongoCollection<AiProviderLogDocument> AiProviderLogs
        => _database.GetCollection<AiProviderLogDocument>("ai_provider_logs");

    /// <summary>Thông báo in-app — TTL 30 ngày.</summary>
    public IMongoCollection<NotificationDocument> Notifications
        => _database.GetCollection<NotificationDocument>("notifications");

    /// <summary>
    /// Đơn xin trở thành Reader — lưu trữ intro_text, proof_documents, trạng thái duyệt.
    /// Tham chiếu: schema.md ## 6. reader_requests
    /// </summary>
    public IMongoCollection<ReaderRequestDocument> ReaderRequests
        => _database.GetCollection<ReaderRequestDocument>("reader_requests");

    /// <summary>
    /// Hồ sơ công khai Reader — bio, pricing, specialties, online status.
    /// Tham chiếu: schema.md ## 5. reader_profiles
    /// </summary>
    public IMongoCollection<ReaderProfileDocument> ReaderProfiles
        => _database.GetCollection<ReaderProfileDocument>("reader_profiles");

    /// <summary>
    /// Conversations chat 1-1 giữa User và Reader.
    /// Tham chiếu: schema.md ## 7. conversations
    /// </summary>
    public IMongoCollection<ConversationDocument> Conversations
        => _database.GetCollection<ConversationDocument>("conversations");

    /// <summary>
    /// Tin nhắn trong conversations.
    /// Tham chiếu: schema.md ## 8. chat_messages
    /// </summary>
    public IMongoCollection<ChatMessageDocument> ChatMessages
        => _database.GetCollection<ChatMessageDocument>("chat_messages");

    /// <summary>
    /// Báo cáo vi phạm — admin queue.
    /// Tham chiếu: schema.md ## 10. reports
    /// </summary>
    public IMongoCollection<ReportDocument> Reports
        => _database.GetCollection<ReportDocument>("reports");

    // ======================================================================
    // INDEX MANAGEMENT
    // Tạo indexes từ code C# — backup cho init.js (phòng trường hợp
    // dev quên chạy init script). CreateIndexModel idempotent.
    // ======================================================================

    /// <summary>
    /// Tạo tất cả indexes cần thiết cho 5 collections Phase 1.
    /// 
    /// Tại sao tạo index từ code thay vì chỉ dùng init.js?
    /// → init.js cần chạy thủ công qua mongosh. Code đảm bảo indexes
    ///   tồn tại khi app khởi động, không phụ thuộc vào DevOps workflow.
    /// → CreateOne() idempotent: nếu index đã tồn tại, MongoDB bỏ qua.
    /// </summary>
    private void EnsureIndexes()
    {
        // --- cards_catalog ---
        Cards.Indexes.CreateOne(new CreateIndexModel<CardCatalogDocument>(
            Builders<CardCatalogDocument>.IndexKeys.Ascending(c => c.Code),
            new CreateIndexOptions { Unique = true, Name = "idx_code_unique" }));

        // --- user_collections ---
        UserCollections.Indexes.CreateOne(new CreateIndexModel<UserCollectionDocument>(
            Builders<UserCollectionDocument>.IndexKeys
                .Ascending(u => u.UserId)
                .Ascending(u => u.CardId),
            new CreateIndexOptions { Unique = true, Name = "idx_userid_cardid_unique" }));

        UserCollections.Indexes.CreateOne(new CreateIndexModel<UserCollectionDocument>(
            Builders<UserCollectionDocument>.IndexKeys
                .Ascending(u => u.UserId)
                .Descending(u => u.Level),
            new CreateIndexOptions { Name = "idx_userid_level_desc" }));

        // --- reading_sessions ---
        ReadingSessions.Indexes.CreateOne(new CreateIndexModel<ReadingSessionDocument>(
            Builders<ReadingSessionDocument>.IndexKeys
                .Ascending(r => r.UserId)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_createdat_desc" }));

        ReadingSessions.Indexes.CreateOne(new CreateIndexModel<ReadingSessionDocument>(
            Builders<ReadingSessionDocument>.IndexKeys
                .Ascending(r => r.AiStatus)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_aistatus_createdat" }));

        ReadingSessions.Indexes.CreateOne(new CreateIndexModel<ReadingSessionDocument>(
            Builders<ReadingSessionDocument>.IndexKeys
                .Ascending(r => r.UserId)
                .Ascending(r => r.SpreadType)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_spreadtype_createdat" }));

        // --- ai_provider_logs (TTL 90 ngày = 7,776,000 giây) ---
        AiProviderLogs.Indexes.CreateOne(new CreateIndexModel<AiProviderLogDocument>(
            Builders<AiProviderLogDocument>.IndexKeys
                .Ascending(a => a.UserId)
                .Descending(a => a.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_createdat_desc" }));

        AiProviderLogs.Indexes.CreateOne(new CreateIndexModel<AiProviderLogDocument>(
            Builders<AiProviderLogDocument>.IndexKeys.Ascending(a => a.CreatedAt),
            new CreateIndexOptions
            {
                Name = "idx_ttl_90d",
                ExpireAfter = TimeSpan.FromDays(90)
            }));

        // --- notifications (TTL 30 ngày = 2,592,000 giây) ---
        Notifications.Indexes.CreateOne(new CreateIndexModel<NotificationDocument>(
            Builders<NotificationDocument>.IndexKeys
                .Ascending(n => n.UserId)
                .Ascending(n => n.IsRead)
                .Descending(n => n.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_isread_createdat" }));

        Notifications.Indexes.CreateOne(new CreateIndexModel<NotificationDocument>(
            Builders<NotificationDocument>.IndexKeys.Ascending(n => n.CreatedAt),
            new CreateIndexOptions
            {
                Name = "idx_ttl_30d",
                ExpireAfter = TimeSpan.FromDays(30)
            }));

        // --- reader_requests (Phase 2.1) ---
        // Index (user_id, created_at desc): tìm đơn mới nhất của user
        ReaderRequests.Indexes.CreateOne(new CreateIndexModel<ReaderRequestDocument>(
            Builders<ReaderRequestDocument>.IndexKeys
                .Ascending(r => r.UserId)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_createdat_desc" }));

        // Index (status, created_at desc): hàng đợi admin duyệt
        ReaderRequests.Indexes.CreateOne(new CreateIndexModel<ReaderRequestDocument>(
            Builders<ReaderRequestDocument>.IndexKeys
                .Ascending(r => r.Status)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_status_createdat_desc" }));

        // --- reader_profiles (Phase 2.1) ---
        // Unique index (user_id): mỗi user chỉ có 1 profile
        ReaderProfiles.Indexes.CreateOne(new CreateIndexModel<ReaderProfileDocument>(
            Builders<ReaderProfileDocument>.IndexKeys.Ascending(r => r.UserId),
            new CreateIndexOptions { Unique = true, Name = "idx_userid_unique" }));

        // Index (status, updated_at desc): listing Reader online
        ReaderProfiles.Indexes.CreateOne(new CreateIndexModel<ReaderProfileDocument>(
            Builders<ReaderProfileDocument>.IndexKeys
                .Ascending(r => r.Status)
                .Descending(r => r.UpdatedAt),
            new CreateIndexOptions { Name = "idx_status_updatedat_desc" }));

        // --- conversations (Phase 2.2) ---
        // Index (user_id, status, updated_at desc): inbox user
        Conversations.Indexes.CreateOne(new CreateIndexModel<ConversationDocument>(
            Builders<ConversationDocument>.IndexKeys
                .Ascending(c => c.UserId)
                .Ascending(c => c.Status)
                .Descending(c => c.UpdatedAt),
            new CreateIndexOptions { Name = "idx_userid_status_updatedat" }));

        // Index (reader_id, status, updated_at desc): inbox reader
        Conversations.Indexes.CreateOne(new CreateIndexModel<ConversationDocument>(
            Builders<ConversationDocument>.IndexKeys
                .Ascending(c => c.ReaderId)
                .Ascending(c => c.Status)
                .Descending(c => c.UpdatedAt),
            new CreateIndexOptions { Name = "idx_readerid_status_updatedat" }));

        // --- chat_messages (Phase 2.2) ---
        // Index (conversation_id, created_at desc): timeline tin nhắn
        ChatMessages.Indexes.CreateOne(new CreateIndexModel<ChatMessageDocument>(
            Builders<ChatMessageDocument>.IndexKeys
                .Ascending(m => m.ConversationId)
                .Descending(m => m.CreatedAt),
            new CreateIndexOptions { Name = "idx_conversationid_createdat_desc" }));

        // Index (sender_id, created_at desc): audit/risk
        ChatMessages.Indexes.CreateOne(new CreateIndexModel<ChatMessageDocument>(
            Builders<ChatMessageDocument>.IndexKeys
                .Ascending(m => m.SenderId)
                .Descending(m => m.CreatedAt),
            new CreateIndexOptions { Name = "idx_senderid_createdat_desc" }));

        // --- reports (Phase 2.2) ---
        // Index (status, created_at desc): admin queue
        Reports.Indexes.CreateOne(new CreateIndexModel<ReportDocument>(
            Builders<ReportDocument>.IndexKeys
                .Ascending(r => r.Status)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_status_createdat_desc" }));
    }
}

using MongoDB.Driver;
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

    public MongoDbContext(IMongoDatabase database)
    {
        _database = database;

        // Tạo indexes khi khởi tạo context — idempotent (gọi lại không lỗi).
        try 
        {
            EnsureIndexes();
        }
        catch (System.Exception ex)
        {
            // Log warning nhưng không làm sập app
            System.Console.WriteLine($"[MongoDB] Warning: Không thể tạo indexes: {ex.Message}");
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
    }
}

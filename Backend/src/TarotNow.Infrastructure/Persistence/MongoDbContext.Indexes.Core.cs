using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

// Khối cấu hình index cho các collection lõi và helper tạo index an toàn.
public partial class MongoDbContext
{
    // Marker để nhận diện lỗi trùng definition index nhưng khác tên.
    private const string DifferentNameConflictMarker = "Index already exists with a different name";
    // Prefix chuỗi Mongo trả về để tách tên index cũ cần migrate.
    private const string DifferentNamePrefix = "a different name: ";

    /// <summary>
    /// Bảo đảm index cho các collection lõi.
    /// Luồng xử lý: gọi lần lượt card, user collection, reading session, ai log và notification.
    /// </summary>
    private void EnsureCoreCollectionIndexes()
    {
        EnsureCardIndexes();
        EnsureUserCollectionIndexes();
        EnsureReadingSessionIndexes();
        EnsureAiLogIndexes();
        EnsureNotificationIndexes();
    }

    /// <summary>
    /// Tạo index cho cards_catalog.
    /// Luồng xử lý: dùng unique index trên code để đảm bảo mỗi lá bài có mã duy nhất.
    /// </summary>
    private void EnsureCardIndexes()
    {
        SafeCreateIndex(Cards, new CreateIndexModel<CardCatalogDocument>(
            Builders<CardCatalogDocument>.IndexKeys.Ascending(c => c.Code),
            new CreateIndexOptions { Unique = true, Name = "idx_code_unique" }));
        // Code là khóa nghiệp vụ ổn định để map card từ các workflow đọc bài.
    }

    /// <summary>
    /// Tạo index cho user_collections.
    /// Luồng xử lý: chặn duplicate user-card và tối ưu truy vấn bộ sưu tập theo level giảm dần.
    /// </summary>
    private void EnsureUserCollectionIndexes()
    {
        SafeCreateIndex(UserCollections, new CreateIndexModel<UserCollectionDocument>(
            Builders<UserCollectionDocument>.IndexKeys.Ascending(u => u.UserId).Ascending(u => u.CardId),
            new CreateIndexOptions { Unique = true, Name = "idx_userid_cardid_unique" }));
        // Business rule: user không thể sở hữu trùng một card record.

        SafeCreateIndex(UserCollections, new CreateIndexModel<UserCollectionDocument>(
            Builders<UserCollectionDocument>.IndexKeys.Ascending(u => u.UserId).Descending(u => u.Level),
            new CreateIndexOptions { Name = "idx_userid_level_desc" }));
        // Dùng cho UI hiển thị card theo độ hiếm/cấp cao trước.
    }

    /// <summary>
    /// Tạo index cho reading_sessions.
    /// Luồng xử lý: tối ưu truy vấn lịch sử toàn cục và lịch sử theo user có lọc soft-delete.
    /// </summary>
    private void EnsureReadingSessionIndexes()
    {
        SafeCreateIndex(ReadingSessions, new CreateIndexModel<ReadingSessionDocument>(
            Builders<ReadingSessionDocument>.IndexKeys.Ascending(r => r.IsDeleted).Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_isdeleted_createdat_desc" }));

        SafeCreateIndex(ReadingSessions, new CreateIndexModel<ReadingSessionDocument>(
            Builders<ReadingSessionDocument>.IndexKeys.Ascending(r => r.UserId).Ascending(r => r.IsDeleted).Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_isdeleted_createdat_desc" }));
    }

    /// <summary>
    /// Tạo index cho ai_provider_logs.
    /// Luồng xử lý: hỗ trợ truy vấn log theo user và tự dọn log sau 90 ngày bằng TTL.
    /// </summary>
    private void EnsureAiLogIndexes()
    {
        SafeCreateIndex(AiProviderLogs, new CreateIndexModel<AiProviderLogDocument>(
            Builders<AiProviderLogDocument>.IndexKeys.Ascending(a => a.UserId).Descending(a => a.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_createdat_desc" }));

        SafeCreateIndex(AiProviderLogs, new CreateIndexModel<AiProviderLogDocument>(
            Builders<AiProviderLogDocument>.IndexKeys.Ascending(a => a.CreatedAt),
            new CreateIndexOptions { Name = "idx_ttl_90d", ExpireAfter = TimeSpan.FromDays(90) }));
        // TTL giữ log đủ dài cho audit nhưng vẫn kiểm soát chi phí lưu trữ.
    }

    /// <summary>
    /// Tạo index cho notifications.
    /// Luồng xử lý: tối ưu inbox thông báo chưa đọc và TTL dọn thông báo cũ.
    /// </summary>
    private void EnsureNotificationIndexes()
    {
        SafeCreateIndex(Notifications, new CreateIndexModel<NotificationDocument>(
            Builders<NotificationDocument>.IndexKeys.Ascending(n => n.UserId).Ascending(n => n.IsRead).Descending(n => n.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_isread_createdat" }));
        // Pattern truy vấn chính là theo user + trạng thái đã đọc.

        SafeCreateIndex(Notifications, new CreateIndexModel<NotificationDocument>(
            Builders<NotificationDocument>.IndexKeys.Ascending(n => n.CreatedAt),
            new CreateIndexOptions { Name = "idx_ttl_30d", ExpireAfter = TimeSpan.FromDays(30) }));
        // Thông báo quá cũ ít giá trị nghiệp vụ nên tự dọn sau 30 ngày.
    }

    /// <summary>
    /// Tạo index với cơ chế tự xử lý xung đột tên index.
    /// Luồng xử lý: thử create index, nếu cùng definition nhưng khác tên thì migrate tên cũ sang tên mới.
    /// </summary>
    private void SafeCreateIndex<TDocument>(
        IMongoCollection<TDocument> collection,
        CreateIndexModel<TDocument> indexModel)
    {
        try
        {
            collection.Indexes.CreateOne(indexModel);
        }
        catch (MongoCommandException ex) when (ex.Message.Contains(DifferentNameConflictMarker))
        {
            var oldName = ExtractConflictingIndexName(ex.Message);
            if (string.IsNullOrEmpty(oldName)) throw;
            // Không suy diễn khi không tách được tên cũ để tránh drop nhầm index.

            MigrateConflictingIndex(collection, indexModel, oldName);
            // Khi đã có tên cũ rõ ràng thì drop-create để chuẩn hóa naming toàn môi trường.
        }
    }

    /// <summary>
    /// Migrate index cũ sang tên chuẩn mới.
    /// Luồng xử lý: ghi log debug, drop index cũ rồi tạo lại index mới cùng definition.
    /// </summary>
    private void MigrateConflictingIndex<TDocument>(
        IMongoCollection<TDocument> collection,
        CreateIndexModel<TDocument> indexModel,
        string oldName)
    {
        _logger.LogDebug(
            "[MongoDB] Auto-migrating index '{OldName}' → '{NewName}' on {Collection}.",
            oldName,
            indexModel.Options.Name,
            collection.CollectionNamespace.CollectionName);

        collection.Indexes.DropOne(oldName);
        collection.Indexes.CreateOne(indexModel);
    }

    /// <summary>
    /// Tách tên index cũ từ lỗi Mongo khi conflict tên.
    /// Luồng xử lý: tìm prefix đặc trưng trong message, cắt phần tên và chuẩn hóa chuỗi kết quả.
    /// </summary>
    private static string? ExtractConflictingIndexName(string errorMessage)
    {
        var idx = errorMessage.IndexOf(DifferentNamePrefix, StringComparison.Ordinal);
        if (idx < 0) return null;
        // Edge case: message không chứa prefix kỳ vọng, không thể migrate an toàn.

        var name = errorMessage[(idx + DifferentNamePrefix.Length)..].Trim().TrimEnd('.');
        return string.IsNullOrEmpty(name) ? null : name;
    }
}

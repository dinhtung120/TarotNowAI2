using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    /// <summary>
    /// Tạo index cho các collection chính (cards, user_collections, reading_sessions, v.v.).
    /// Sử dụng helper SafeCreateIndex để xử lý trường hợp index cũ đã tồn tại
    /// với tên khác (ví dụ: tên tự động "code_1" vs tên mới "idx_code_unique").
    /// </summary>
    private void EnsureCoreCollectionIndexes()
    {
        // --- Cards ---
        // Index unique trên Code để đảm bảo không trùng mã lá bài
        SafeCreateIndex(Cards, new CreateIndexModel<CardCatalogDocument>(
            Builders<CardCatalogDocument>.IndexKeys.Ascending(c => c.Code),
            new CreateIndexOptions { Unique = true, Name = "idx_code_unique" }));

        // --- UserCollections ---
        // Index compound unique (UserId + CardId) → mỗi user chỉ sở hữu 1 bản ghi / lá bài
        SafeCreateIndex(UserCollections, new CreateIndexModel<UserCollectionDocument>(
            Builders<UserCollectionDocument>.IndexKeys.Ascending(u => u.UserId).Ascending(u => u.CardId),
            new CreateIndexOptions { Unique = true, Name = "idx_userid_cardid_unique" }));

        // Index hỗ trợ truy vấn "lá bài của user, sắp theo level giảm dần"
        SafeCreateIndex(UserCollections, new CreateIndexModel<UserCollectionDocument>(
            Builders<UserCollectionDocument>.IndexKeys.Ascending(u => u.UserId).Descending(u => u.Level),
            new CreateIndexOptions { Name = "idx_userid_level_desc" }));

        // --- ReadingSessions ---
        // Index cho truy vấn "tất cả session chưa xoá, mới nhất trước"
        SafeCreateIndex(ReadingSessions, new CreateIndexModel<ReadingSessionDocument>(
            Builders<ReadingSessionDocument>.IndexKeys.Ascending(r => r.IsDeleted).Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_isdeleted_createdat_desc" }));

        // Index cho truy vấn "session của 1 user, chưa xoá, mới nhất trước"
        SafeCreateIndex(ReadingSessions, new CreateIndexModel<ReadingSessionDocument>(
            Builders<ReadingSessionDocument>.IndexKeys.Ascending(r => r.UserId).Ascending(r => r.IsDeleted).Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_isdeleted_createdat_desc" }));

        // --- AiProviderLogs ---
        // Index cho truy vấn log AI theo user, sắp theo thời gian
        SafeCreateIndex(AiProviderLogs, new CreateIndexModel<AiProviderLogDocument>(
            Builders<AiProviderLogDocument>.IndexKeys.Ascending(a => a.UserId).Descending(a => a.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_createdat_desc" }));

        // TTL index tự xoá log AI sau 90 ngày
        SafeCreateIndex(AiProviderLogs, new CreateIndexModel<AiProviderLogDocument>(
            Builders<AiProviderLogDocument>.IndexKeys.Ascending(a => a.CreatedAt),
            new CreateIndexOptions { Name = "idx_ttl_90d", ExpireAfter = TimeSpan.FromDays(90) }));

        // --- Notifications ---
        // Index compound cho truy vấn thông báo của user (đã đọc / chưa đọc)
        SafeCreateIndex(Notifications, new CreateIndexModel<NotificationDocument>(
            Builders<NotificationDocument>.IndexKeys.Ascending(n => n.UserId).Ascending(n => n.IsRead).Descending(n => n.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_isread_createdat" }));

        // TTL index tự xoá thông báo sau 30 ngày
        SafeCreateIndex(Notifications, new CreateIndexModel<NotificationDocument>(
            Builders<NotificationDocument>.IndexKeys.Ascending(n => n.CreatedAt),
            new CreateIndexOptions { Name = "idx_ttl_30d", ExpireAfter = TimeSpan.FromDays(30) }));
    }

    /// <summary>
    /// Tạo index an toàn: nếu MongoDB báo "Index already exists with a different name",
    /// nghĩa là key-spec giống nhưng tên cũ khác → drop index cũ (theo tên trích từ lỗi)
    /// rồi tạo lại với tên mới.
    /// Lý do cần helper này: các phiên bản trước dùng tên tự động (vd: "code_1"),
    /// nay ta đổi sang tên có ý nghĩa (vd: "idx_code_unique"). MongoDB không cho phép
    /// 2 index cùng key-spec nhưng khác tên.
    /// </summary>
    private void SafeCreateIndex<TDocument>(
        IMongoCollection<TDocument> collection,
        CreateIndexModel<TDocument> indexModel)
    {
        try
        {
            collection.Indexes.CreateOne(indexModel);
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("Index already exists with a different name"))
        {
            // Trích tên index cũ gây xung đột từ thông báo lỗi
            // Dạng: "Index already exists with a different name: code_1"
            var oldName = ExtractConflictingIndexName(ex.Message);
            if (!string.IsNullOrEmpty(oldName))
            {
                _logger.LogDebug(
                    "[MongoDB] Auto-migrating index '{OldName}' → '{NewName}' on {Collection}.",
                    oldName,
                    indexModel.Options.Name,
                    collection.CollectionNamespace.CollectionName);

                // Xoá index cũ có tên xung đột
                collection.Indexes.DropOne(oldName);

                // Tạo lại index với tên mới
                collection.Indexes.CreateOne(indexModel);
            }
            else
            {
                // Không trích được tên → ném lại lỗi gốc để không nuốt mất
                throw;
            }
        }
    }

    /// <summary>
    /// Trích tên index cũ từ thông báo lỗi MongoDB.
    /// Ví dụ input: "Index already exists with a different name: code_1"
    /// → trả về "code_1"
    /// </summary>
    private static string? ExtractConflictingIndexName(string errorMessage)
    {
        // Tìm chuỗi sau dấu ": " cuối cùng trong thông báo lỗi
        const string marker = "a different name: ";
        var idx = errorMessage.IndexOf(marker, StringComparison.Ordinal);
        if (idx < 0) return null;

        // Lấy phần sau marker, cắt bỏ dấu chấm cuối (nếu có)
        var name = errorMessage[(idx + marker.Length)..].Trim().TrimEnd('.');
        return string.IsNullOrEmpty(name) ? null : name;
    }
}

using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    /// <summary>
    /// Tạo index cho các collection liên quan đến Reader (reader_requests, reader_profiles).
    /// Sử dụng SafeCreateIndex để xử lí index cũ bị xung đột tên.
    /// </summary>
    private void EnsureReaderCollectionIndexes()
    {
        // --- ReaderRequests ---
        // Index tra cứu đơn đăng ký reader theo user, mới nhất trước
        SafeCreateIndex(ReaderRequests, new CreateIndexModel<ReaderRequestDocument>(
            Builders<ReaderRequestDocument>.IndexKeys
                .Ascending(r => r.UserId)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_createdat_desc" }));

        // Index lọc đơn đăng ký theo trạng thái (pending/approved/rejected)
        SafeCreateIndex(ReaderRequests, new CreateIndexModel<ReaderRequestDocument>(
            Builders<ReaderRequestDocument>.IndexKeys
                .Ascending(r => r.Status)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_status_createdat_desc" }));

        // --- ReaderProfiles ---
        // Index unique: mỗi user chỉ có 1 hồ sơ reader
        SafeCreateIndex(ReaderProfiles, new CreateIndexModel<ReaderProfileDocument>(
            Builders<ReaderProfileDocument>.IndexKeys.Ascending(r => r.UserId),
            new CreateIndexOptions { Unique = true, Name = "idx_userid_unique" }));

        // Index cho danh sách reader công khai (chưa xoá, active/busy, mới cập nhật)
        SafeCreateIndex(ReaderProfiles, new CreateIndexModel<ReaderProfileDocument>(
            Builders<ReaderProfileDocument>.IndexKeys
                .Ascending(r => r.IsDeleted)
                .Ascending(r => r.Status)
                .Descending(r => r.UpdatedAt),
            new CreateIndexOptions { Name = "idx_isdeleted_status_updatedat_desc" }));
    }
}

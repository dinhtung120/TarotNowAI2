using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

// Khối cấu hình index cho reader_requests và reader_profiles.
public partial class MongoDbContext
{
    /// <summary>
    /// Bảo đảm index cho các collection của luồng reader.
    /// Luồng xử lý: tối ưu truy vấn request theo user/status và profile theo user/status để hỗ trợ duyệt hồ sơ.
    /// </summary>
    private void EnsureReaderCollectionIndexes()
    {
        SafeCreateIndex(ReaderRequests, new CreateIndexModel<ReaderRequestDocument>(
            Builders<ReaderRequestDocument>.IndexKeys
                .Ascending(r => r.UserId)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_createdat_desc" }));
        // Hỗ trợ truy vấn lịch sử gửi yêu cầu theo user với bản ghi mới nhất trước.

        SafeCreateIndex(ReaderRequests, new CreateIndexModel<ReaderRequestDocument>(
            Builders<ReaderRequestDocument>.IndexKeys
                .Ascending(r => r.Status)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_status_createdat_desc" }));
        // Admin queue cần lọc theo trạng thái xử lý và thời điểm tạo.

        SafeCreateIndex(ReaderProfiles, new CreateIndexModel<ReaderProfileDocument>(
            Builders<ReaderProfileDocument>.IndexKeys.Ascending(r => r.UserId),
            new CreateIndexOptions { Unique = true, Name = "idx_userid_unique" }));
        // Business rule: mỗi user chỉ có một reader profile.

        SafeCreateIndex(ReaderProfiles, new CreateIndexModel<ReaderProfileDocument>(
            Builders<ReaderProfileDocument>.IndexKeys
                .Ascending(r => r.IsDeleted)
                .Ascending(r => r.Status)
                .Descending(r => r.UpdatedAt),
            new CreateIndexOptions { Name = "idx_isdeleted_status_updatedat_desc" }));
        // Tối ưu danh mục reader theo trạng thái active/pending và thời điểm cập nhật.
    }
}

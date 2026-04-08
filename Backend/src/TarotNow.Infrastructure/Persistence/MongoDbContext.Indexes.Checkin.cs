using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;
using System;

namespace TarotNow.Infrastructure.Persistence;

// Khối cấu hình index cho collection daily_checkins.
public partial class MongoDbContext
{
    /// <summary>
    /// Bảo đảm index cho dữ liệu điểm danh ngày.
    /// Luồng xử lý: tạo unique index user+business_date và TTL index để dọn log cũ.
    /// </summary>
    private void EnsureCheckinIndexes()
    {
        SafeCreateIndex(DailyCheckins, new CreateIndexModel<DailyCheckinDocument>(
            Builders<DailyCheckinDocument>.IndexKeys.Ascending(n => n.UserId).Ascending(n => n.BusinessDate),
            new CreateIndexOptions { Name = "idx_userid_businessdate_unique", Unique = true }));
        // Business rule: mỗi user chỉ được một record check-in trong một business date.

        SafeCreateIndex(DailyCheckins, new CreateIndexModel<DailyCheckinDocument>(
            Builders<DailyCheckinDocument>.IndexKeys.Ascending(n => n.CreatedAt),
            new CreateIndexOptions { Name = "idx_ttl_90d", ExpireAfter = TimeSpan.FromDays(90) }));
        // Chỉ giữ log 90 ngày để giảm kích thước collection lịch sử.
    }
}

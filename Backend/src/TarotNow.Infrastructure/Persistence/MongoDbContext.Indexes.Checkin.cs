using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;
using System;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private void EnsureCheckinIndexes()
    {
        // 1. Chỉ mục Unique Compound cho Điểm Danh (Idempotency chống double-checkin bằng UserId và BusinessDate)
        // Nếu user thử bắn 2 request đồng thời, MongoDB sẽ cản đường không cho insert bản ghi thứ 2.
        SafeCreateIndex(DailyCheckins, new CreateIndexModel<DailyCheckinDocument>(
            Builders<DailyCheckinDocument>.IndexKeys.Ascending(n => n.UserId).Ascending(n => n.BusinessDate),
            new CreateIndexOptions { Name = "idx_userid_businessdate_unique", Unique = true }));

        // 2. TTL Index tự diệt bản ghi rác sau 90 ngày.
        // Chỉ lưu lịch sử checkin để hiển thị cho UI (ví dụ app hiện lịch sử 1 tháng), xa hơn thì bỏ rác đỡ tốn công.
        SafeCreateIndex(DailyCheckins, new CreateIndexModel<DailyCheckinDocument>(
            Builders<DailyCheckinDocument>.IndexKeys.Ascending(n => n.CreatedAt),
            new CreateIndexOptions { Name = "idx_ttl_90d", ExpireAfter = TimeSpan.FromDays(90) }));
    }
}

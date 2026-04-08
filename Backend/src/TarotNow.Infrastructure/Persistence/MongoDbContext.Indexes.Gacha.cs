

using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

// Khối cấu hình index cho gacha_logs.
public partial class MongoDbContext
{
    /// <summary>
    /// Bảo đảm index cho collection gacha_logs.
    /// Luồng xử lý: tạo index truy vấn lịch sử quay, tạo TTL 30 ngày và xử lý xung đột option index cũ.
    /// </summary>
    private void EnsureGachaIndexes()
    {
        try
        {
            var logsCollection = GachaLogs;
            var indexKeysDefinition = Builders<GachaLogDocument>.IndexKeys
                .Descending(x => x.UserId)
                .Descending(x => x.CreatedAt);

            var indexModel = new CreateIndexModel<GachaLogDocument>(indexKeysDefinition, new CreateIndexOptions { Background = true });
            logsCollection.Indexes.CreateOne(indexModel);
            // Tối ưu truy vấn lịch sử quay theo user với bản ghi mới nhất ở đầu.

            var ttlIndexKeys = Builders<GachaLogDocument>.IndexKeys.Ascending(x => x.CreatedAt);
            var ttlOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(30), Background = true };
            var ttlIndexModel = new CreateIndexModel<GachaLogDocument>(ttlIndexKeys, ttlOptions);
            // TTL 30 ngày để giới hạn chi phí lưu log quay thưởng.

            try
            {
                logsCollection.Indexes.CreateOne(ttlIndexModel);
            }
            catch (MongoCommandException ex) when (ex.CodeName == "IndexOptionsConflict")
            {
                logsCollection.Indexes.DropOne("created_at_1");
                logsCollection.Indexes.CreateOne(ttlIndexModel);
                // Edge case: index TTL cũ khác option, drop-recreate để đồng bộ policy hết hạn.
            }

            _logger.LogInformation("[MongoDB] Gacha Logs indexes created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MongoDB] Failed to create Gacha Logs indexes.");
            // Không throw để tránh làm fail startup chỉ vì lỗi tạo index phụ trợ.
        }
    }
}

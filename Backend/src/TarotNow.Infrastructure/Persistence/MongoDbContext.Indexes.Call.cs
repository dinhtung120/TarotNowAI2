using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private void EnsureCallIndexes()
    {
        try
        {
            var callSessions = CallSessions;

            // 1. (conversation_id, created_at desc) – Query timeline cuộc gọi
            var index1 = new CreateIndexModel<CallSessionDocument>(
                Builders<CallSessionDocument>.IndexKeys
                    .Ascending(x => x.ConversationId)
                    .Descending(x => x.CreatedAt),
                new CreateIndexOptions { Background = true }
            );

            // 2. (status, conversation_id) – Query filter active calls
            var index2 = new CreateIndexModel<CallSessionDocument>(
                Builders<CallSessionDocument>.IndexKeys
                    .Ascending(x => x.Status)
                    .Ascending(x => x.ConversationId),
                new CreateIndexOptions { Background = true }
            );

            // 3. (initiator_id, created_at desc) – Tra cứu các cuộc gọi gây ra bởi một người dùng
            var index3 = new CreateIndexModel<CallSessionDocument>(
                Builders<CallSessionDocument>.IndexKeys
                    .Ascending(x => x.InitiatorId)
                    .Descending(x => x.CreatedAt),
                new CreateIndexOptions { Background = true }
            );

            // FIX #11: TTL index – Tự động xóa call sessions cũ hơn 90 ngày.
            // Không có TTL thì data tích luỹ mãi mãi.
            // MongoDB TTL chỉ hoạt động trên field kiểu Date → dùng CreatedAt.
            var ttlIndex = new CreateIndexModel<CallSessionDocument>(
                Builders<CallSessionDocument>.IndexKeys.Ascending(x => x.CreatedAt),
                new CreateIndexOptions
                {
                    Background = true,
                    ExpireAfter = TimeSpan.FromDays(90)
                }
            );

            callSessions.Indexes.CreateMany(new[] { index1, index2, index3, ttlIndex });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MongoDB] Không thể tạo indexes cho collection call_sessions.");
        }
    }
}

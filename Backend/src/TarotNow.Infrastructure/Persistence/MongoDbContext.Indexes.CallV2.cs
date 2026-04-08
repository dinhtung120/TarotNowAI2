using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private void EnsureCallV2Indexes()
    {
        var activeFilter = Builders<CallSessionV2Document>.Filter.In(x => x.Status, CallSessionV2Statuses.ActiveStates);

        var indexes = new List<CreateIndexModel<CallSessionV2Document>>
        {
            new(
                Builders<CallSessionV2Document>.IndexKeys
                    .Ascending(x => x.ConversationId)
                    .Descending(x => x.CreatedAt),
                new CreateIndexOptions { Background = true, Name = "idx_callv2_conversation_timeline" }),
            new(
                Builders<CallSessionV2Document>.IndexKeys.Ascending(x => x.RoomName),
                new CreateIndexOptions { Background = true, Unique = true, Name = "uq_callv2_room_name" }),
            new(
                Builders<CallSessionV2Document>.IndexKeys.Ascending(x => x.ConversationId),
                new CreateIndexOptions<CallSessionV2Document>
                {
                    Background = true,
                    Unique = true,
                    Name = "uq_callv2_active_per_conversation",
                    PartialFilterExpression = activeFilter
                }),
            new(
                Builders<CallSessionV2Document>.IndexKeys.Ascending(x => x.CreatedAt),
                new CreateIndexOptions { Background = true, ExpireAfter = TimeSpan.FromDays(30), Name = "idx_callv2_ttl_30d" })
        };

        try
        {
            CallSessionsV2.Indexes.CreateMany(indexes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MongoDB] Không thể tạo indexes cho collection call_sessions_v2.");
        }
    }
}

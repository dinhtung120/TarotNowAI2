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
            CallSessions.Indexes.CreateMany(CreateCallSessionIndexes());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MongoDB] Không thể tạo indexes cho collection call_sessions.");
        }
    }

    private static IEnumerable<CreateIndexModel<CallSessionDocument>> CreateCallSessionIndexes()
    {
        return
        [
            CreateConversationTimelineIndex(),
            CreateStatusConversationIndex(),
            CreateInitiatorTimelineIndex(),
            CreateActiveConversationUniqueIndex(),
            CreateCallSessionTtlIndex()
        ];
    }

    private static CreateIndexModel<CallSessionDocument> CreateConversationTimelineIndex()
    {
        return new CreateIndexModel<CallSessionDocument>(
            Builders<CallSessionDocument>.IndexKeys
                .Ascending(x => x.ConversationId)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Background = true });
    }

    private static CreateIndexModel<CallSessionDocument> CreateStatusConversationIndex()
    {
        return new CreateIndexModel<CallSessionDocument>(
            Builders<CallSessionDocument>.IndexKeys
                .Ascending(x => x.Status)
                .Ascending(x => x.ConversationId),
            new CreateIndexOptions { Background = true });
    }

    private static CreateIndexModel<CallSessionDocument> CreateInitiatorTimelineIndex()
    {
        return new CreateIndexModel<CallSessionDocument>(
            Builders<CallSessionDocument>.IndexKeys
                .Ascending(x => x.InitiatorId)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Background = true });
    }

    private static CreateIndexModel<CallSessionDocument> CreateActiveConversationUniqueIndex()
    {
        var activeCallFilter = Builders<CallSessionDocument>.Filter.In(x => x.Status, ["requested", "accepted"]);
        return new CreateIndexModel<CallSessionDocument>(
            Builders<CallSessionDocument>.IndexKeys.Ascending(x => x.ConversationId),
            new CreateIndexOptions<CallSessionDocument>
            {
                Background = true,
                Unique = true,
                Name = "uq_call_active_per_conversation",
                PartialFilterExpression = activeCallFilter
            });
    }

    private static CreateIndexModel<CallSessionDocument> CreateCallSessionTtlIndex()
    {
        return new CreateIndexModel<CallSessionDocument>(
            Builders<CallSessionDocument>.IndexKeys.Ascending(x => x.CreatedAt),
            new CreateIndexOptions
            {
                Background = true,
                ExpireAfter = TimeSpan.FromDays(90)
            });
    }
}

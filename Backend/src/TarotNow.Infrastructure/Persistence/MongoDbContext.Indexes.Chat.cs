using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private void EnsureChatCollectionIndexes()
    {
        Conversations.Indexes.CreateOne(new CreateIndexModel<ConversationDocument>(
            Builders<ConversationDocument>.IndexKeys.Ascending(c => c.IsDeleted).Ascending(c => c.UserId).Ascending(c => c.Status).Descending(c => c.UpdatedAt),
            new CreateIndexOptions { Name = "idx_isdeleted_userid_status_updatedat" }));

        Conversations.Indexes.CreateOne(new CreateIndexModel<ConversationDocument>(
            Builders<ConversationDocument>.IndexKeys.Ascending(c => c.IsDeleted).Ascending(c => c.ReaderId).Ascending(c => c.Status).Descending(c => c.UpdatedAt),
            new CreateIndexOptions { Name = "idx_isdeleted_readerid_status_updatedat" }));

        ChatMessages.Indexes.CreateOne(new CreateIndexModel<ChatMessageDocument>(
            Builders<ChatMessageDocument>.IndexKeys.Ascending(m => m.ConversationId).Ascending(m => m.IsDeleted).Descending(m => m.CreatedAt),
            new CreateIndexOptions { Name = "idx_conversationid_isdeleted_createdat_desc" }));

        ChatMessages.Indexes.CreateOne(new CreateIndexModel<ChatMessageDocument>(
            Builders<ChatMessageDocument>.IndexKeys.Ascending(m => m.SenderId).Descending(m => m.CreatedAt),
            new CreateIndexOptions { Name = "idx_senderid_createdat_desc" }));

        Reports.Indexes.CreateOne(new CreateIndexModel<ReportDocument>(
            Builders<ReportDocument>.IndexKeys.Ascending(r => r.Status).Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_status_createdat_desc" }));
    }
}

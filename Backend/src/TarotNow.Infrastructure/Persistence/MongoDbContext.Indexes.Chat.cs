using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private void EnsureChatCollectionIndexes()
    {
        EnsureConversationIndexes();
        EnsureChatMessageIndexes();
        EnsureReportIndexes();
    }

    private void EnsureConversationIndexes()
    {
        SafeCreateIndex(Conversations, new CreateIndexModel<ConversationDocument>(
            Builders<ConversationDocument>.IndexKeys
                .Ascending(c => c.IsDeleted)
                .Ascending(c => c.UserId)
                .Ascending(c => c.Status)
                .Descending(c => c.UpdatedAt),
            new CreateIndexOptions { Name = "idx_isdeleted_userid_status_updatedat" }));

        SafeCreateIndex(Conversations, new CreateIndexModel<ConversationDocument>(
            Builders<ConversationDocument>.IndexKeys
                .Ascending(c => c.IsDeleted)
                .Ascending(c => c.ReaderId)
                .Ascending(c => c.Status)
                .Descending(c => c.UpdatedAt),
            new CreateIndexOptions { Name = "idx_isdeleted_readerid_status_updatedat" }));
    }

    private void EnsureChatMessageIndexes()
    {
        SafeCreateIndex(ChatMessages, new CreateIndexModel<ChatMessageDocument>(
            Builders<ChatMessageDocument>.IndexKeys
                .Ascending(m => m.ConversationId)
                .Ascending(m => m.IsDeleted)
                .Descending(m => m.CreatedAt),
            new CreateIndexOptions { Name = "idx_conversationid_isdeleted_createdat_desc" }));

        SafeCreateIndex(ChatMessages, new CreateIndexModel<ChatMessageDocument>(
            Builders<ChatMessageDocument>.IndexKeys
                .Ascending(m => m.SenderId)
                .Descending(m => m.CreatedAt),
            new CreateIndexOptions { Name = "idx_senderid_createdat_desc" }));
    }

    private void EnsureReportIndexes()
    {
        SafeCreateIndex(Reports, new CreateIndexModel<ReportDocument>(
            Builders<ReportDocument>.IndexKeys
                .Ascending(r => r.Status)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_status_createdat_desc" }));

        SafeCreateIndex(Reports, new CreateIndexModel<ReportDocument>(
            Builders<ReportDocument>.IndexKeys
                .Ascending(r => r.Target.Type)
                .Ascending(r => r.Target.Id)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_targettype_targetid_createdat_desc" }));
    }
}

using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private void EnsureCoreCollectionIndexes()
    {
        Cards.Indexes.CreateOne(new CreateIndexModel<CardCatalogDocument>(
            Builders<CardCatalogDocument>.IndexKeys.Ascending(c => c.Code),
            new CreateIndexOptions { Unique = true, Name = "idx_code_unique" }));

        UserCollections.Indexes.CreateOne(new CreateIndexModel<UserCollectionDocument>(
            Builders<UserCollectionDocument>.IndexKeys.Ascending(u => u.UserId).Ascending(u => u.CardId),
            new CreateIndexOptions { Unique = true, Name = "idx_userid_cardid_unique" }));

        UserCollections.Indexes.CreateOne(new CreateIndexModel<UserCollectionDocument>(
            Builders<UserCollectionDocument>.IndexKeys.Ascending(u => u.UserId).Descending(u => u.Level),
            new CreateIndexOptions { Name = "idx_userid_level_desc" }));

        ReadingSessions.Indexes.CreateOne(new CreateIndexModel<ReadingSessionDocument>(
            Builders<ReadingSessionDocument>.IndexKeys.Ascending(r => r.IsDeleted).Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_isdeleted_createdat_desc" }));

        ReadingSessions.Indexes.CreateOne(new CreateIndexModel<ReadingSessionDocument>(
            Builders<ReadingSessionDocument>.IndexKeys.Ascending(r => r.UserId).Ascending(r => r.IsDeleted).Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_isdeleted_createdat_desc" }));

        AiProviderLogs.Indexes.CreateOne(new CreateIndexModel<AiProviderLogDocument>(
            Builders<AiProviderLogDocument>.IndexKeys.Ascending(a => a.UserId).Descending(a => a.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_createdat_desc" }));

        AiProviderLogs.Indexes.CreateOne(new CreateIndexModel<AiProviderLogDocument>(
            Builders<AiProviderLogDocument>.IndexKeys.Ascending(a => a.CreatedAt),
            new CreateIndexOptions { Name = "idx_ttl_90d", ExpireAfter = TimeSpan.FromDays(90) }));

        Notifications.Indexes.CreateOne(new CreateIndexModel<NotificationDocument>(
            Builders<NotificationDocument>.IndexKeys.Ascending(n => n.UserId).Ascending(n => n.IsRead).Descending(n => n.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_isread_createdat" }));

        Notifications.Indexes.CreateOne(new CreateIndexModel<NotificationDocument>(
            Builders<NotificationDocument>.IndexKeys.Ascending(n => n.CreatedAt),
            new CreateIndexOptions { Name = "idx_ttl_30d", ExpireAfter = TimeSpan.FromDays(30) }));
    }
}

using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private const string DifferentNameConflictMarker = "Index already exists with a different name";
    private const string DifferentNamePrefix = "a different name: ";

    private void EnsureCoreCollectionIndexes()
    {
        EnsureCardIndexes();
        EnsureUserCollectionIndexes();
        EnsureReadingSessionIndexes();
        EnsureAiLogIndexes();
        EnsureNotificationIndexes();
    }

    private void EnsureCardIndexes()
    {
        SafeCreateIndex(Cards, new CreateIndexModel<CardCatalogDocument>(
            Builders<CardCatalogDocument>.IndexKeys.Ascending(c => c.Code),
            new CreateIndexOptions { Unique = true, Name = "idx_code_unique" }));
    }

    private void EnsureUserCollectionIndexes()
    {
        SafeCreateIndex(UserCollections, new CreateIndexModel<UserCollectionDocument>(
            Builders<UserCollectionDocument>.IndexKeys.Ascending(u => u.UserId).Ascending(u => u.CardId),
            new CreateIndexOptions { Unique = true, Name = "idx_userid_cardid_unique" }));

        SafeCreateIndex(UserCollections, new CreateIndexModel<UserCollectionDocument>(
            Builders<UserCollectionDocument>.IndexKeys.Ascending(u => u.UserId).Descending(u => u.Level),
            new CreateIndexOptions { Name = "idx_userid_level_desc" }));
    }

    private void EnsureReadingSessionIndexes()
    {
        SafeCreateIndex(ReadingSessions, new CreateIndexModel<ReadingSessionDocument>(
            Builders<ReadingSessionDocument>.IndexKeys.Ascending(r => r.IsDeleted).Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_isdeleted_createdat_desc" }));

        SafeCreateIndex(ReadingSessions, new CreateIndexModel<ReadingSessionDocument>(
            Builders<ReadingSessionDocument>.IndexKeys.Ascending(r => r.UserId).Ascending(r => r.IsDeleted).Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_isdeleted_createdat_desc" }));
    }

    private void EnsureAiLogIndexes()
    {
        SafeCreateIndex(AiProviderLogs, new CreateIndexModel<AiProviderLogDocument>(
            Builders<AiProviderLogDocument>.IndexKeys.Ascending(a => a.UserId).Descending(a => a.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_createdat_desc" }));

        SafeCreateIndex(AiProviderLogs, new CreateIndexModel<AiProviderLogDocument>(
            Builders<AiProviderLogDocument>.IndexKeys.Ascending(a => a.CreatedAt),
            new CreateIndexOptions { Name = "idx_ttl_90d", ExpireAfter = TimeSpan.FromDays(90) }));
    }

    private void EnsureNotificationIndexes()
    {
        SafeCreateIndex(Notifications, new CreateIndexModel<NotificationDocument>(
            Builders<NotificationDocument>.IndexKeys.Ascending(n => n.UserId).Ascending(n => n.IsRead).Descending(n => n.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_isread_createdat" }));

        
        SafeCreateIndex(Notifications, new CreateIndexModel<NotificationDocument>(
            Builders<NotificationDocument>.IndexKeys.Ascending(n => n.CreatedAt),
            new CreateIndexOptions { Name = "idx_ttl_30d", ExpireAfter = TimeSpan.FromDays(30) }));
    }

    private void SafeCreateIndex<TDocument>(
        IMongoCollection<TDocument> collection,
        CreateIndexModel<TDocument> indexModel)
    {
        try
        {
            collection.Indexes.CreateOne(indexModel);
        }
        catch (MongoCommandException ex) when (ex.Message.Contains(DifferentNameConflictMarker))
        {
            var oldName = ExtractConflictingIndexName(ex.Message);
            if (string.IsNullOrEmpty(oldName)) throw;
            MigrateConflictingIndex(collection, indexModel, oldName);
        }
    }

    private void MigrateConflictingIndex<TDocument>(
        IMongoCollection<TDocument> collection,
        CreateIndexModel<TDocument> indexModel,
        string oldName)
    {
        _logger.LogDebug(
            "[MongoDB] Auto-migrating index '{OldName}' → '{NewName}' on {Collection}.",
            oldName,
            indexModel.Options.Name,
            collection.CollectionNamespace.CollectionName);

        collection.Indexes.DropOne(oldName);
        collection.Indexes.CreateOne(indexModel);
    }

    private static string? ExtractConflictingIndexName(string errorMessage)
    {
        var idx = errorMessage.IndexOf(DifferentNamePrefix, StringComparison.Ordinal);
        if (idx < 0) return null;

        var name = errorMessage[(idx + DifferentNamePrefix.Length)..].Trim().TrimEnd('.');
        return string.IsNullOrEmpty(name) ? null : name;
    }
}

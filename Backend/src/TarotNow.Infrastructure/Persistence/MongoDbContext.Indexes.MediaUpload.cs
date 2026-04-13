using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

// Khối cấu hình index cho collection media upload (sessions + community assets).
public partial class MongoDbContext
{
    /// <summary>
    /// Bảo đảm index cho các collection media upload.
    /// </summary>
    private void EnsureMediaUploadIndexes()
    {
        EnsureUploadSessionIndexes();
        EnsureCommunityMediaAssetIndexes();
    }

    private void EnsureUploadSessionIndexes()
    {
        SafeCreateIndex(UploadSessions, new CreateIndexModel<UploadSessionDocument>(
            Builders<UploadSessionDocument>.IndexKeys
                .Ascending(x => x.ExpiresAtUtc)
                .Ascending(x => x.ConsumedAtUtc)
                .Ascending(x => x.CleanedUpAtUtc),
            new CreateIndexOptions { Name = "idx_expiresat_consumed_cleanup" }));

        SafeCreateIndex(UploadSessions, new CreateIndexModel<UploadSessionDocument>(
            Builders<UploadSessionDocument>.IndexKeys
                .Ascending(x => x.OwnerUserId)
                .Ascending(x => x.Scope)
                .Descending(x => x.CreatedAtUtc),
            new CreateIndexOptions { Name = "idx_owner_scope_createdat" }));
    }

    private void EnsureCommunityMediaAssetIndexes()
    {
        SafeCreateIndex(CommunityMediaAssets, new CreateIndexModel<CommunityMediaAssetDocument>(
            Builders<CommunityMediaAssetDocument>.IndexKeys
                .Ascending(x => x.OwnerUserId)
                .Ascending(x => x.ContextType)
                .Ascending(x => x.ContextEntityId)
                .Ascending(x => x.Status),
            new CreateIndexOptions { Name = "idx_owner_context_entity_status" }));

        SafeCreateIndex(CommunityMediaAssets, new CreateIndexModel<CommunityMediaAssetDocument>(
            Builders<CommunityMediaAssetDocument>.IndexKeys
                .Ascending(x => x.ContextType)
                .Ascending(x => x.ContextDraftId)
                .Ascending(x => x.Status),
            new CreateIndexOptions { Name = "idx_context_draft_status" }));

        SafeCreateIndex(CommunityMediaAssets, new CreateIndexModel<CommunityMediaAssetDocument>(
            Builders<CommunityMediaAssetDocument>.IndexKeys
                .Ascending(x => x.Status)
                .Ascending(x => x.ExpiresAtUtc),
            new CreateIndexOptions { Name = "idx_status_expiresat" }));
    }
}

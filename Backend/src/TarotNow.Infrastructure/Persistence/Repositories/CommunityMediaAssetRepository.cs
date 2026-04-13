using MongoDB.Driver;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository quản lý asset ảnh community trên MongoDB.
/// </summary>
public sealed class CommunityMediaAssetRepository : ICommunityMediaAssetRepository
{
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository community media assets.
    /// </summary>
    public CommunityMediaAssetRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public Task UpsertUploadedAsync(CommunityMediaAssetRecord asset, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityMediaAssetDocument>.Filter.Eq(x => x.ObjectKey, asset.ObjectKey);
        var update = Builders<CommunityMediaAssetDocument>.Update
            .Set(x => x.PublicUrl, asset.PublicUrl)
            .Set(x => x.OwnerUserId, asset.OwnerUserId.ToString())
            .Set(x => x.ContextType, asset.ContextType)
            .Set(x => x.ContextDraftId, asset.ContextDraftId)
            .Set(x => x.ContextEntityId, asset.ContextEntityId)
            .Set(x => x.Status, asset.Status)
            .Set(x => x.UpdatedAtUtc, asset.UpdatedAtUtc)
            .Set(x => x.AttachedAtUtc, asset.AttachedAtUtc)
            .Set(x => x.OrphanedAtUtc, asset.OrphanedAtUtc)
            .Set(x => x.DeletedAtUtc, asset.DeletedAtUtc)
            .Set(x => x.ExpiresAtUtc, asset.ExpiresAtUtc)
            .SetOnInsert(x => x.CreatedAtUtc, asset.CreatedAtUtc);

        return _context.CommunityMediaAssets.UpdateOneAsync(
            filter,
            update,
            new UpdateOptions { IsUpsert = true },
            cancellationToken);
    }

    /// <inheritdoc />
    public Task AttachDraftAssetsAsync(
        AttachDraftCommunityAssetsRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.ObjectKeys.Count == 0)
        {
            return Task.CompletedTask;
        }

        var filter = Builders<CommunityMediaAssetDocument>.Filter.Eq(x => x.OwnerUserId, request.OwnerUserId.ToString())
                     & Builders<CommunityMediaAssetDocument>.Filter.Eq(x => x.ContextType, request.ContextType)
                     & Builders<CommunityMediaAssetDocument>.Filter.Eq(x => x.ContextDraftId, request.ContextDraftId)
                     & Builders<CommunityMediaAssetDocument>.Filter.In(x => x.ObjectKey, request.ObjectKeys)
                     & Builders<CommunityMediaAssetDocument>.Filter.In(x => x.Status, new[]
                     {
                         MediaUploadConstants.AssetStatusUploaded,
                         MediaUploadConstants.AssetStatusOrphaned,
                     });

        var update = BuildAttachUpdate(request.ContextEntityId, request.AttachedAtUtc);
        return _context.CommunityMediaAssets.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public Task AttachByObjectKeysAsync(
        AttachCommunityAssetsByObjectKeysRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.ObjectKeys.Count == 0)
        {
            return Task.CompletedTask;
        }

        var filter = Builders<CommunityMediaAssetDocument>.Filter.Eq(x => x.OwnerUserId, request.OwnerUserId.ToString())
                     & Builders<CommunityMediaAssetDocument>.Filter.Eq(x => x.ContextType, request.ContextType)
                     & Builders<CommunityMediaAssetDocument>.Filter.In(x => x.ObjectKey, request.ObjectKeys)
                     & Builders<CommunityMediaAssetDocument>.Filter.In(x => x.Status, new[]
                     {
                         MediaUploadConstants.AssetStatusUploaded,
                         MediaUploadConstants.AssetStatusOrphaned,
                     });

        var update = BuildAttachUpdate(request.ContextEntityId, request.AttachedAtUtc);
        return _context.CommunityMediaAssets.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public Task ReconcileAttachedAssetsAsync(
        ReconcileCommunityAssetsRequest request,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityMediaAssetDocument>.Filter.Eq(x => x.OwnerUserId, request.OwnerUserId.ToString())
                     & Builders<CommunityMediaAssetDocument>.Filter.Eq(x => x.ContextType, request.ContextType)
                     & Builders<CommunityMediaAssetDocument>.Filter.Eq(x => x.ContextEntityId, request.ContextEntityId)
                     & Builders<CommunityMediaAssetDocument>.Filter.Eq(x => x.Status, MediaUploadConstants.AssetStatusAttached);

        if (request.ActiveObjectKeys.Count > 0)
        {
            filter &= Builders<CommunityMediaAssetDocument>.Filter.Nin(x => x.ObjectKey, request.ActiveObjectKeys);
        }

        var update = Builders<CommunityMediaAssetDocument>.Update
            .Set(x => x.Status, MediaUploadConstants.AssetStatusOrphaned)
            .Set(x => x.OrphanedAtUtc, request.ReconciledAtUtc)
            .Set(x => x.UpdatedAtUtc, request.ReconciledAtUtc)
            .Set(x => x.ExpiresAtUtc, request.OrphanExpiresAtUtc);

        return _context.CommunityMediaAssets.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<CommunityMediaAssetRecord>> GetCleanupCandidatesAsync(
        DateTime nowUtc,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var normalizedLimit = limit <= 0 ? 100 : Math.Min(limit, 1000);
        var filter = Builders<CommunityMediaAssetDocument>.Filter.In(x => x.Status, new[]
                     {
                         MediaUploadConstants.AssetStatusUploaded,
                         MediaUploadConstants.AssetStatusOrphaned,
                     })
                     & Builders<CommunityMediaAssetDocument>.Filter.Eq(x => x.DeletedAtUtc, null)
                     & Builders<CommunityMediaAssetDocument>.Filter.Lte(x => x.ExpiresAtUtc, nowUtc);

        var documents = await _context.CommunityMediaAssets.Find(filter)
            .SortBy(x => x.ExpiresAtUtc)
            .Limit(normalizedLimit)
            .ToListAsync(cancellationToken);

        return documents.Select(ToRecord).ToList();
    }

    /// <inheritdoc />
    public async Task<bool> MarkDeletedAsync(string objectKey, DateTime deletedAtUtc, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityMediaAssetDocument>.Filter.Eq(x => x.ObjectKey, objectKey)
                     & Builders<CommunityMediaAssetDocument>.Filter.Ne(x => x.Status, MediaUploadConstants.AssetStatusDeleted);

        var update = Builders<CommunityMediaAssetDocument>.Update
            .Set(x => x.Status, MediaUploadConstants.AssetStatusDeleted)
            .Set(x => x.DeletedAtUtc, deletedAtUtc)
            .Set(x => x.UpdatedAtUtc, deletedAtUtc);

        var result = await _context.CommunityMediaAssets.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    private static UpdateDefinition<CommunityMediaAssetDocument> BuildAttachUpdate(string contextEntityId, DateTime attachedAtUtc)
    {
        return Builders<CommunityMediaAssetDocument>.Update
            .Set(x => x.Status, MediaUploadConstants.AssetStatusAttached)
            .Set(x => x.ContextEntityId, contextEntityId)
            .Set(x => x.AttachedAtUtc, attachedAtUtc)
            .Set(x => x.UpdatedAtUtc, attachedAtUtc)
            .Set(x => x.OrphanedAtUtc, null)
            .Set(x => x.ExpiresAtUtc, DateTime.MaxValue);
    }

    private static CommunityMediaAssetRecord ToRecord(CommunityMediaAssetDocument document)
    {
        return new CommunityMediaAssetRecord
        {
            ObjectKey = document.ObjectKey,
            PublicUrl = document.PublicUrl,
            OwnerUserId = Guid.TryParse(document.OwnerUserId, out var ownerId) ? ownerId : Guid.Empty,
            ContextType = document.ContextType,
            ContextDraftId = document.ContextDraftId,
            ContextEntityId = document.ContextEntityId,
            Status = document.Status,
            CreatedAtUtc = document.CreatedAtUtc,
            UpdatedAtUtc = document.UpdatedAtUtc,
            AttachedAtUtc = document.AttachedAtUtc,
            OrphanedAtUtc = document.OrphanedAtUtc,
            DeletedAtUtc = document.DeletedAtUtc,
            ExpiresAtUtc = document.ExpiresAtUtc,
        };
    }
}

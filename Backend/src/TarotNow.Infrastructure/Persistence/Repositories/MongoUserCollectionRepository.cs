using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository chính quản lý bộ sưu tập thẻ bài của user.
/// </summary>
public partial class MongoUserCollectionRepository : IUserCollectionRepository
{
    private const int MaxOptimisticRetries = 5;
    private const int MaxAppliedOperationKeyHistory = 128;

    private readonly MongoDbContext _mongoContext;

    /// <summary>
    /// Khởi tạo repository user collection.
    /// </summary>
    public MongoUserCollectionRepository(MongoDbContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    /// <summary>
    /// Upsert thẻ bài cho user khi draw/nhận exp.
    /// </summary>
    public async Task UpsertCardAsync(
        Guid userId,
        int cardId,
        decimal expToGain,
        string orientation = CardOrientation.Upright,
        string? operationKey = null,
        CancellationToken cancellationToken = default)
    {
        ValidateUpsertInputs(userId, cardId, expToGain);

        var filter = BuildUserCardFilter(userId, cardId);
        var normalizedOrientation = ReadingDrawnCardCodec.NormalizeOrientation(orientation);
        var normalizedOperationKey = NormalizeOperationKey(operationKey);
        var context = new UpsertCardContext(
            UserId: userId,
            CardId: cardId,
            ExpToGain: expToGain,
            Filter: filter,
            Orientation: normalizedOrientation,
            OperationKey: normalizedOperationKey);

        for (var attempt = 0; attempt < MaxOptimisticRetries; attempt++)
        {
            var existingDoc = await _mongoContext.UserCollections.Find(context.Filter).FirstOrDefaultAsync(cancellationToken);
            if (existingDoc is null)
            {
                if (await TryInsertNewDocumentAsync(context, cancellationToken))
                {
                    return;
                }

                continue;
            }

            if (await TryReplaceExistingDocumentAsync(existingDoc, context, cancellationToken))
            {
                return;
            }
        }

        throw new InvalidOperationException("Collection card was updated concurrently. Please retry.");
    }

    private async Task<bool> TryInsertNewDocumentAsync(
        UpsertCardContext context,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var createdDoc = CreateBaselineDocument(context.UserId, context.CardId, now);
        if (context.OperationKey is not null)
        {
            createdDoc.AppliedOperationKeys = new List<string> { context.OperationKey };
        }

        IncreaseDrawCount(createdDoc, context.Orientation);
        ApplyExpAndProgression(createdDoc, context.ExpToGain, now);

        try
        {
            await _mongoContext.UserCollections.InsertOneAsync(createdDoc, cancellationToken: cancellationToken);
            return true;
        }
        catch (MongoWriteException writeException) when (writeException.WriteError.Code == 11000)
        {
            return false;
        }
    }

    private async Task<bool> TryReplaceExistingDocumentAsync(
        UserCollectionDocument existingDoc,
        UpsertCardContext context,
        CancellationToken cancellationToken)
    {
        NormalizeAndHydrateLegacyFields(existingDoc);
        if (HasAppliedOperationKey(existingDoc, context.OperationKey))
        {
            return true;
        }

        var now = DateTime.UtcNow;
        var updatedDoc = CloneDocument(existingDoc);
        AppendOperationKey(updatedDoc, context.OperationKey);
        IncreaseDrawCount(updatedDoc, context.Orientation);
        ApplyExpAndProgression(updatedDoc, context.ExpToGain, now);

        var replaceResult = await _mongoContext.UserCollections.ReplaceOneAsync(
            BuildVersionedFilter(context.Filter, existingDoc.UpdatedAt),
            updatedDoc,
            cancellationToken: cancellationToken);
        return replaceResult.ModifiedCount > 0;
    }

    private readonly record struct UpsertCardContext(
        Guid UserId,
        int CardId,
        decimal ExpToGain,
        FilterDefinition<UserCollectionDocument> Filter,
        string Orientation,
        string? OperationKey);

    /// <summary>
    /// Lấy bộ sưu tập thẻ của user.
    /// </summary>
    public async Task<IEnumerable<UserCollection>> GetUserCollectionAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();

        var docs = await _mongoContext.UserCollections
            .Find(u => u.UserId == userIdStr && !u.IsDeleted)
            .SortByDescending(u => u.Level)
            .ToListAsync(cancellationToken);

        return docs.Select(MapToEntity);
    }
}

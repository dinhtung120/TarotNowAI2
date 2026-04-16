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
        CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (cardId < 0)
        {
            throw new ArgumentException("CardId must be greater than or equal to 0.", nameof(cardId));
        }

        if (expToGain < 0m)
        {
            throw new ArgumentException("Exp must be greater than or equal to 0.", nameof(expToGain));
        }

        var filter = BuildUserCardFilter(userId, cardId);
        var normalizedOrientation = ReadingDrawnCardCodec.NormalizeOrientation(orientation);

        for (var attempt = 0; attempt < MaxOptimisticRetries; attempt++)
        {
            var existingDoc = await _mongoContext.UserCollections.Find(filter).FirstOrDefaultAsync(cancellationToken);
            if (existingDoc is null)
            {
                var createdDoc = CreateBaselineDocument(userId, cardId, DateTime.UtcNow);
                IncreaseDrawCount(createdDoc, normalizedOrientation);
                ApplyExpAndProgression(createdDoc, expToGain, DateTime.UtcNow);

                try
                {
                    await _mongoContext.UserCollections.InsertOneAsync(createdDoc, cancellationToken: cancellationToken);
                    return;
                }
                catch (MongoWriteException writeException) when (writeException.WriteError.Code == 11000)
                {
                    continue;
                }
            }

            NormalizeAndHydrateLegacyFields(existingDoc);
            var updatedDoc = CloneDocument(existingDoc);
            IncreaseDrawCount(updatedDoc, normalizedOrientation);
            ApplyExpAndProgression(updatedDoc, expToGain, DateTime.UtcNow);

            var replaceResult = await _mongoContext.UserCollections.ReplaceOneAsync(
                BuildVersionedFilter(filter, existingDoc.UpdatedAt),
                updatedDoc,
                cancellationToken: cancellationToken);
            if (replaceResult.ModifiedCount > 0)
            {
                return;
            }
        }

        throw new InvalidOperationException("Collection card was updated concurrently. Please retry.");
    }

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

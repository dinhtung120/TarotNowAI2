using MongoDB.Driver;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Partial helper dựng filter/update và map user collection.
/// </summary>
public partial class MongoUserCollectionRepository
{
    /// <summary>
    /// Dựng filter theo cặp user-card.
    /// </summary>
    private static FilterDefinition<UserCollectionDocument> BuildUserCardFilter(Guid userId, int cardId)
    {
        var userIdString = userId.ToString();
        return Builders<UserCollectionDocument>.Filter.Eq(u => u.UserId, userIdString)
               & Builders<UserCollectionDocument>.Filter.Eq(u => u.CardId, cardId);
    }

    /// <summary>
    /// Dựng filter version theo UpdatedAt để tránh lost update.
    /// </summary>
    private static FilterDefinition<UserCollectionDocument> BuildVersionedFilter(
        FilterDefinition<UserCollectionDocument> baseFilter,
        DateTime previousUpdatedAt)
    {
        return Builders<UserCollectionDocument>.Filter.And(
            baseFilter,
            Builders<UserCollectionDocument>.Filter.Eq(x => x.UpdatedAt, previousUpdatedAt));
    }

    /// <summary>
    /// Tạo baseline document cho card mới.
    /// </summary>
    private static UserCollectionDocument CreateBaselineDocument(Guid userId, int cardId, DateTime nowUtc)
    {
        return new UserCollectionDocument
        {
            UserId = userId.ToString(),
            CardId = cardId,
            Level = 1,
            Exp = 0m,
            ExpToNextLevel = UserCollection.ResolveExpToNextLevel(1),
            BaseAtk = UserCollection.DefaultBaseAtk,
            BaseDef = UserCollection.DefaultBaseDef,
            BonusAtkPercent = 0m,
            BonusDefPercent = 0m,
            Atk = UserCollection.DefaultBaseAtk,
            Def = UserCollection.DefaultBaseDef,
            Stats = new DrawStats(),
            StatHistory = new List<StatRollRecord>(),
            IsDeleted = false,
            CreatedAt = nowUtc,
            UpdatedAt = nowUtc,
            LastDrawnAt = nowUtc,
        };
    }

    /// <summary>
    /// Tăng số lần bốc của card theo orientation.
    /// </summary>
    private static void IncreaseDrawCount(UserCollectionDocument document, string orientation)
    {
        if (string.Equals(orientation, CardOrientation.Reversed, StringComparison.OrdinalIgnoreCase))
        {
            document.Stats.TimesDrawnReversed += 1;
            return;
        }

        document.Stats.TimesDrawnUpright += 1;
    }

    /// <summary>
    /// Map document user collection sang aggregate domain.
    /// </summary>
    private static UserCollection MapToEntity(UserCollectionDocument doc)
    {
        Guid.TryParse(doc.UserId, out var userId);
        NormalizeAndHydrateLegacyFields(doc);

        var totalDraws = (doc.Stats?.TimesDrawnUpright ?? 0) + (doc.Stats?.TimesDrawnReversed ?? 0);
        var copies = Math.Max(totalDraws, 1);
        var lastDrawnAt = doc.LastDrawnAt == default ? doc.UpdatedAt : doc.LastDrawnAt;

        return UserCollection.Rehydrate(new UserCollectionSnapshot
        {
            UserId = userId,
            CardId = doc.CardId,
            Level = doc.Level,
            Copies = copies,
            CurrentExp = doc.Exp,
            ExpToNextLevel = doc.ExpToNextLevel,
            BaseAtk = doc.BaseAtk,
            BaseDef = doc.BaseDef,
            BonusAtkPercent = doc.BonusAtkPercent,
            BonusDefPercent = doc.BonusDefPercent,
            LastDrawnAt = lastDrawnAt,
        });
    }
}

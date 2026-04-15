using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Partial repository xử lý enhancement card từ inventory items.
/// </summary>
public partial class MongoUserCollectionRepository
{
    /// <inheritdoc />
    public Task<bool> ExistsAsync(Guid userId, int cardId, CancellationToken cancellationToken = default)
    {
        var userIdText = userId.ToString();
        return _mongoContext.UserCollections
            .Find(x => x.UserId == userIdText && x.CardId == cardId && x.IsDeleted == false)
            .AnyAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<CardEnhancementApplyResult> ApplyEnhancementAsync(
        CardEnhancementApplyRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.UserId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(request));
        }

        if (request.CardId <= 0)
        {
            throw new ArgumentException("CardId must be positive.", nameof(request));
        }

        var filter = BuildUserCardFilter(request.UserId, request.CardId);
        var normalizedType = request.EnhancementType.Trim().ToLowerInvariant();

        return normalizedType switch
        {
            TarotNow.Domain.ValueObjects.EnhancementType.Exp => await ApplyExpEnhancementAsync(filter, request, cancellationToken),
            TarotNow.Domain.ValueObjects.EnhancementType.Power => await ApplyPowerEnhancementAsync(filter, request, cancellationToken),
            TarotNow.Domain.ValueObjects.EnhancementType.Defense => await ApplyDefenseEnhancementAsync(filter, request, cancellationToken),
            TarotNow.Domain.ValueObjects.EnhancementType.LevelUpgrade => await ApplyLevelUpgradeEnhancementAsync(filter, cancellationToken, request.SuccessRatePercent),
            _ => throw new InvalidOperationException($"Unsupported enhancement type: {request.EnhancementType}"),
        };
    }

    private async Task<CardEnhancementApplyResult> ApplyExpEnhancementAsync(
        FilterDefinition<UserCollectionDocument> filter,
        CardEnhancementApplyRequest request,
        CancellationToken cancellationToken)
    {
        var delta = Math.Max(1, request.EffectValue);
        var update = Builders<UserCollectionDocument>.Update
            .Inc(x => x.Exp, delta)
            .Set(x => x.UpdatedAt, DateTime.UtcNow)
            .Set(x => x.LastDrawnAt, DateTime.UtcNow);

        var result = await _mongoContext.UserCollections.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        if (result.MatchedCount == 0)
        {
            throw new InvalidOperationException("Target card was not found in collection.");
        }

        return new CardEnhancementApplyResult
        {
            Succeeded = true,
            ExpDelta = delta,
            AttackDelta = 0,
            DefenseDelta = 0,
            LevelUpgraded = false,
        };
    }

    private async Task<CardEnhancementApplyResult> ApplyPowerEnhancementAsync(
        FilterDefinition<UserCollectionDocument> filter,
        CardEnhancementApplyRequest request,
        CancellationToken cancellationToken)
    {
        var delta = Math.Max(1, request.EffectValue);
        var update = Builders<UserCollectionDocument>.Update
            .Inc(x => x.Atk, delta)
            .Set(x => x.UpdatedAt, DateTime.UtcNow)
            .Set(x => x.LastDrawnAt, DateTime.UtcNow);

        var result = await _mongoContext.UserCollections.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        if (result.MatchedCount == 0)
        {
            throw new InvalidOperationException("Target card was not found in collection.");
        }

        return new CardEnhancementApplyResult
        {
            Succeeded = true,
            ExpDelta = 0,
            AttackDelta = delta,
            DefenseDelta = 0,
            LevelUpgraded = false,
        };
    }

    private async Task<CardEnhancementApplyResult> ApplyDefenseEnhancementAsync(
        FilterDefinition<UserCollectionDocument> filter,
        CardEnhancementApplyRequest request,
        CancellationToken cancellationToken)
    {
        var delta = Math.Max(1, request.EffectValue);
        var update = Builders<UserCollectionDocument>.Update
            .Inc(x => x.Def, delta)
            .Set(x => x.UpdatedAt, DateTime.UtcNow)
            .Set(x => x.LastDrawnAt, DateTime.UtcNow);

        var result = await _mongoContext.UserCollections.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        if (result.MatchedCount == 0)
        {
            throw new InvalidOperationException("Target card was not found in collection.");
        }

        return new CardEnhancementApplyResult
        {
            Succeeded = true,
            ExpDelta = 0,
            AttackDelta = 0,
            DefenseDelta = delta,
            LevelUpgraded = false,
        };
    }

}

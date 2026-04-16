using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.ValueObjects;
using TarotNow.Infrastructure.Persistence.MongoDocuments;
using System.Security.Cryptography;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Partial repository xử lý enhancement card từ inventory items.
/// </summary>
public partial class MongoUserCollectionRepository
{
    private const int PercentageBranchOneUpper = 70;
    private const int PercentageBranchTwoUpper = 95;
    private const decimal MinimumStatDelta = 0.01m;

    private const int ExpBranchOneUpper = 60;
    private const int ExpBranchTwoUpper = 85;
    private const int ExpBranchThreeUpper = 95;

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
            EnhancementType.Exp => await ApplyExpEnhancementAsync(filter, cancellationToken),
            EnhancementType.Power => await ApplyPowerEnhancementAsync(filter, cancellationToken),
            EnhancementType.Defense => await ApplyDefenseEnhancementAsync(filter, cancellationToken),
            EnhancementType.LevelUpgrade => await ApplyLevelUpgradeEnhancementAsync(filter, cancellationToken, request.SuccessRatePercent),
            _ => throw new InvalidOperationException($"Unsupported enhancement type: {request.EnhancementType}"),
        };
    }

    private async Task<CardEnhancementApplyResult> ApplyExpEnhancementAsync(
        FilterDefinition<UserCollectionDocument> filter,
        CancellationToken cancellationToken)
    {
        var delta = RollExpDelta();
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
        CancellationToken cancellationToken)
    {
        return await ApplyPercentStatEnhancementAsync(
            filter,
            isAttack: true,
            cancellationToken);
    }

    private async Task<CardEnhancementApplyResult> ApplyDefenseEnhancementAsync(
        FilterDefinition<UserCollectionDocument> filter,
        CancellationToken cancellationToken)
    {
        return await ApplyPercentStatEnhancementAsync(
            filter,
            isAttack: false,
            cancellationToken);
    }

    private async Task<CardEnhancementApplyResult> ApplyPercentStatEnhancementAsync(
        FilterDefinition<UserCollectionDocument> filter,
        bool isAttack,
        CancellationToken cancellationToken)
    {
        var document = await _mongoContext.UserCollections.Find(filter).FirstOrDefaultAsync(cancellationToken);
        if (document is null)
        {
            throw new InvalidOperationException("Target card was not found in collection.");
        }

        var percent = RollStatPercent();
        var baseValue = isAttack ? document.Atk : document.Def;
        var delta = CalculatePercentDelta(baseValue, percent);

        var nowUtc = DateTime.UtcNow;
        var valueFilter = isAttack
            ? Builders<UserCollectionDocument>.Filter.And(
                filter,
                Builders<UserCollectionDocument>.Filter.Eq(x => x.Atk, document.Atk))
            : Builders<UserCollectionDocument>.Filter.And(
                filter,
                Builders<UserCollectionDocument>.Filter.Eq(x => x.Def, document.Def));
        var update = isAttack
            ? Builders<UserCollectionDocument>.Update
                .Inc(x => x.Atk, delta)
                .Set(x => x.UpdatedAt, nowUtc)
                .Set(x => x.LastDrawnAt, nowUtc)
            : Builders<UserCollectionDocument>.Update
                .Inc(x => x.Def, delta)
                .Set(x => x.UpdatedAt, nowUtc)
                .Set(x => x.LastDrawnAt, nowUtc);
        var result = await _mongoContext.UserCollections.UpdateOneAsync(
            valueFilter,
            update,
            cancellationToken: cancellationToken);
        if (result.ModifiedCount == 0)
        {
            throw new InvalidOperationException("Target card was updated concurrently. Please retry.");
        }

        return new CardEnhancementApplyResult
        {
            Succeeded = true,
            ExpDelta = 0m,
            AttackDelta = isAttack ? delta : 0m,
            DefenseDelta = isAttack ? 0m : delta,
            LevelUpgraded = false,
        };
    }

    private static decimal RollExpDelta()
    {
        var branchRoll = RandomNumberGenerator.GetInt32(1, 101);
        if (branchRoll <= ExpBranchOneUpper)
        {
            return RandomNumberGenerator.GetInt32(1, 26);
        }

        if (branchRoll <= ExpBranchTwoUpper)
        {
            return RandomNumberGenerator.GetInt32(26, 56);
        }

        if (branchRoll <= ExpBranchThreeUpper)
        {
            return RandomNumberGenerator.GetInt32(56, 81);
        }

        return RandomNumberGenerator.GetInt32(81, 101);
    }

    private static decimal RollStatPercent()
    {
        var branchRoll = RandomNumberGenerator.GetInt32(1, 101);
        if (branchRoll <= PercentageBranchOneUpper)
        {
            return RandomNumberGenerator.GetInt32(1, 4);
        }

        if (branchRoll <= PercentageBranchTwoUpper)
        {
            return RandomNumberGenerator.GetInt32(4, 7);
        }

        return RandomNumberGenerator.GetInt32(7, 11);
    }

    private static decimal CalculatePercentDelta(decimal baseValue, decimal percent)
    {
        var rawDelta = baseValue * percent / 100m;
        var roundedDelta = Math.Round(rawDelta, 2, MidpointRounding.AwayFromZero);
        return roundedDelta < MinimumStatDelta ? MinimumStatDelta : roundedDelta;
    }
}

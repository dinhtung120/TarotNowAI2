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

        if (request.CardId < 0)
        {
            throw new ArgumentException("CardId must be greater than or equal to 0.", nameof(request));
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

    private Task<CardEnhancementApplyResult> ApplyExpEnhancementAsync(
        FilterDefinition<UserCollectionDocument> filter,
        CancellationToken cancellationToken)
    {
        return ApplyMutationWithRetryAsync(
            filter,
            (document, nowUtc) =>
            {
                var before = BuildStatSnapshot(document);
                var expDelta = RollExpDelta();
                ApplyExpAndProgression(document, expDelta, nowUtc);
                var after = BuildStatSnapshot(document);

                return new CardEnhancementApplyResult
                {
                    Succeeded = true,
                    ExpDelta = expDelta,
                    AttackDelta = Round2(after.TotalAtk - before.TotalAtk),
                    DefenseDelta = Round2(after.TotalDef - before.TotalDef),
                    RolledValue = expDelta,
                    LevelUpgraded = after.Level > before.Level,
                    BeforeStats = before,
                    AfterStats = after,
                };
            },
            cancellationToken);
    }

    private Task<CardEnhancementApplyResult> ApplyPowerEnhancementAsync(
        FilterDefinition<UserCollectionDocument> filter,
        CancellationToken cancellationToken)
    {
        return ApplyPercentStatEnhancementAsync(filter, isAttack: true, cancellationToken);
    }

    private Task<CardEnhancementApplyResult> ApplyDefenseEnhancementAsync(
        FilterDefinition<UserCollectionDocument> filter,
        CancellationToken cancellationToken)
    {
        return ApplyPercentStatEnhancementAsync(filter, isAttack: false, cancellationToken);
    }

    private Task<CardEnhancementApplyResult> ApplyPercentStatEnhancementAsync(
        FilterDefinition<UserCollectionDocument> filter,
        bool isAttack,
        CancellationToken cancellationToken)
    {
        return ApplyMutationWithRetryAsync(
            filter,
            (document, nowUtc) =>
            {
                var before = BuildStatSnapshot(document);
                var percentDelta = RollStatPercent();

                if (isAttack)
                {
                    document.BonusAtkPercent = Round2(document.BonusAtkPercent + percentDelta);
                }
                else
                {
                    document.BonusDefPercent = Round2(document.BonusDefPercent + percentDelta);
                }

                RecalculateTotalStats(document);
                document.LastDrawnAt = nowUtc;
                document.UpdatedAt = nowUtc;

                var after = BuildStatSnapshot(document);
                return new CardEnhancementApplyResult
                {
                    Succeeded = true,
                    ExpDelta = 0m,
                    AttackDelta = Round2(after.TotalAtk - before.TotalAtk),
                    DefenseDelta = Round2(after.TotalDef - before.TotalDef),
                    RolledValue = percentDelta,
                    LevelUpgraded = false,
                    BeforeStats = before,
                    AfterStats = after,
                };
            },
            cancellationToken);
    }

    private async Task<CardEnhancementApplyResult> ApplyMutationWithRetryAsync(
        FilterDefinition<UserCollectionDocument> filter,
        Func<UserCollectionDocument, DateTime, CardEnhancementApplyResult> mutation,
        CancellationToken cancellationToken)
    {
        for (var attempt = 0; attempt < MaxOptimisticRetries; attempt++)
        {
            var existingDocument = await _mongoContext.UserCollections.Find(filter).FirstOrDefaultAsync(cancellationToken);
            if (existingDocument is null)
            {
                throw new InvalidOperationException("Target card was not found in collection.");
            }

            NormalizeAndHydrateLegacyFields(existingDocument);
            var updatedDocument = CloneDocument(existingDocument);
            var result = mutation(updatedDocument, DateTime.UtcNow);
            var replaceResult = await _mongoContext.UserCollections.ReplaceOneAsync(
                BuildVersionedFilter(filter, existingDocument.UpdatedAt),
                updatedDocument,
                cancellationToken: cancellationToken);
            if (replaceResult.ModifiedCount > 0)
            {
                return result;
            }
        }

        throw new InvalidOperationException("Target card was updated concurrently. Please retry.");
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
}

using System.Security.Cryptography;
using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Partial repository xử lý riêng flow level upgrade enhancement.
/// </summary>
public partial class MongoUserCollectionRepository
{
    private async Task<CardEnhancementApplyResult> ApplyLevelUpgradeEnhancementAsync(
        FilterDefinition<UserCollectionDocument> filter,
        CancellationToken cancellationToken,
        decimal successRatePercent)
    {
        if (RollLevelUpgrade(successRatePercent) == false)
        {
            return new CardEnhancementApplyResult
            {
                Succeeded = true,
                ExpDelta = 0m,
                AttackDelta = 0m,
                DefenseDelta = 0m,
                LevelUpgraded = false,
            };
        }

        var document = await GetExistingUserCollectionAsync(filter, cancellationToken);
        var upgradeRoll = RollStatBonusesForUpgrade(document.Level);
        var update = BuildLevelUpgradeUpdate(upgradeRoll.newLevel, upgradeRoll.atkBonus, upgradeRoll.defBonus);
        await EnsureLevelUpgradeUpdatedAsync(filter, document.Level, update, cancellationToken);

        return new CardEnhancementApplyResult
        {
            Succeeded = true,
            ExpDelta = 0m,
            AttackDelta = upgradeRoll.atkBonus,
            DefenseDelta = upgradeRoll.defBonus,
            LevelUpgraded = true,
        };
    }

    private static bool RollLevelUpgrade(decimal successRatePercent)
    {
        var rate = Math.Clamp(successRatePercent, 0m, 100m);
        var rolled = RandomNumberGenerator.GetInt32(0, 10000) / 100m;
        return rolled <= rate;
    }

    private async Task<UserCollectionDocument> GetExistingUserCollectionAsync(
        FilterDefinition<UserCollectionDocument> filter,
        CancellationToken cancellationToken)
    {
        var document = await _mongoContext.UserCollections.Find(filter).FirstOrDefaultAsync(cancellationToken);
        if (document is null)
        {
            throw new InvalidOperationException("Target card was not found in collection.");
        }

        return document;
    }

    private static (int newLevel, decimal atkBonus, decimal defBonus) RollStatBonusesForUpgrade(int currentLevel)
    {
        var newLevel = Math.Max(1, currentLevel + 1);
        var (minBonus, maxBonus) = TarotNow.Domain.Entities.UserCollection.GetStatBonusRange(newLevel);
        var atkBonus = (decimal)RandomNumberGenerator.GetInt32(minBonus, maxBonus + 1);
        var defBonus = (decimal)RandomNumberGenerator.GetInt32(minBonus, maxBonus + 1);
        return (newLevel, atkBonus, defBonus);
    }

    private static UpdateDefinition<UserCollectionDocument> BuildLevelUpgradeUpdate(
        int newLevel,
        decimal atkBonus,
        decimal defBonus)
    {
        var nowUtc = DateTime.UtcNow;
        return Builders<UserCollectionDocument>.Update
            .Set(x => x.Level, newLevel)
            .Inc(x => x.Atk, atkBonus)
            .Inc(x => x.Def, defBonus)
            .Set(x => x.UpdatedAt, nowUtc)
            .Set(x => x.LastDrawnAt, nowUtc)
            .Push(x => x.StatHistory, new StatRollRecord
            {
                Level = newLevel,
                AtkBonus = atkBonus,
                DefBonus = defBonus,
                RolledAt = nowUtc,
            });
    }

    private async Task EnsureLevelUpgradeUpdatedAsync(
        FilterDefinition<UserCollectionDocument> filter,
        int currentLevel,
        UpdateDefinition<UserCollectionDocument> update,
        CancellationToken cancellationToken)
    {
        var versionedFilter = Builders<UserCollectionDocument>.Filter.And(
            filter,
            Builders<UserCollectionDocument>.Filter.Eq(x => x.Level, currentLevel));
        var updateResult = await _mongoContext.UserCollections.UpdateOneAsync(
            versionedFilter,
            update,
            cancellationToken: cancellationToken);
        if (updateResult.ModifiedCount == 0)
        {
            throw new InvalidOperationException("Target card was updated concurrently. Please retry.");
        }
    }
}

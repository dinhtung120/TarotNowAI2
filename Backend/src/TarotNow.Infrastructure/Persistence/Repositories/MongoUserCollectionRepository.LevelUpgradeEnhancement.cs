using System.Security.Cryptography;
using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Partial repository xử lý riêng flow level upgrade enhancement.
/// </summary>
public partial class MongoUserCollectionRepository
{
    private Task<CardEnhancementApplyResult> ApplyLevelUpgradeEnhancementAsync(
        FilterDefinition<UserCollectionDocument> filter,
        CancellationToken cancellationToken,
        decimal successRatePercent)
    {
        return ApplyMutationWithRetryAsync(
            filter,
            (document, nowUtc) => BuildLevelUpgradeResult(document, nowUtc, successRatePercent),
            cancellationToken);
    }

    private static CardEnhancementApplyResult BuildLevelUpgradeResult(
        UserCollectionDocument document,
        DateTime nowUtc,
        decimal successRatePercent)
    {
        var before = BuildStatSnapshot(document);
        if (CanUpgradeLevel(document, successRatePercent) == false)
        {
            return BuildNoUpgradeResult(document, nowUtc, before);
        }

        var (atkBonus, defBonus) = ApplyLevelUpgrade(document, nowUtc);
        var after = BuildStatSnapshot(document);
        return BuildUpgradeResult(before, after, atkBonus, defBonus);
    }

    private static bool CanUpgradeLevel(UserCollectionDocument document, decimal successRatePercent)
    {
        return document.Level < UserCollection.MaxLevel && RollLevelUpgrade(successRatePercent);
    }

    private static CardEnhancementApplyResult BuildNoUpgradeResult(
        UserCollectionDocument document,
        DateTime nowUtc,
        CardEnhancementStatSnapshot before)
    {
        document.UpdatedAt = nowUtc;
        document.LastDrawnAt = nowUtc;
        var after = BuildStatSnapshot(document);

        return new CardEnhancementApplyResult
        {
            Succeeded = true,
            ExpDelta = 0m,
            AttackDelta = 0m,
            DefenseDelta = 0m,
            RolledValue = 0m,
            LevelUpgraded = false,
            BeforeStats = before,
            AfterStats = after,
        };
    }

    private static (decimal atkBonus, decimal defBonus) ApplyLevelUpgrade(UserCollectionDocument document, DateTime nowUtc)
    {
        var targetLevel = Math.Clamp(document.Level + 1, 1, UserCollection.MaxLevel);
        var (minBonus, maxBonus) = UserCollection.GetStatBonusRange(targetLevel);
        var atkBonus = RandomNumberGenerator.GetInt32(minBonus, maxBonus + 1);
        var defBonus = RandomNumberGenerator.GetInt32(minBonus, maxBonus + 1);

        document.Level = targetLevel;
        document.BaseAtk = Round2(document.BaseAtk + atkBonus);
        document.BaseDef = Round2(document.BaseDef + defBonus);
        document.ExpToNextLevel = UserCollection.ResolveExpToNextLevel(document.Level);
        document.StatHistory ??= new List<StatRollRecord>();
        document.StatHistory.Add(new StatRollRecord
        {
            Level = document.Level,
            AtkBonus = atkBonus,
            DefBonus = defBonus,
            RolledAt = nowUtc,
        });

        RecalculateTotalStats(document);
        document.UpdatedAt = nowUtc;
        document.LastDrawnAt = nowUtc;
        return (atkBonus, defBonus);
    }

    private static CardEnhancementApplyResult BuildUpgradeResult(
        CardEnhancementStatSnapshot before,
        CardEnhancementStatSnapshot after,
        decimal atkBonus,
        decimal defBonus)
    {
        return new CardEnhancementApplyResult
        {
            Succeeded = true,
            ExpDelta = 0m,
            AttackDelta = Round2(after.TotalAtk - before.TotalAtk),
            DefenseDelta = Round2(after.TotalDef - before.TotalDef),
            RolledValue = Round2(atkBonus + defBonus),
            LevelUpgraded = true,
            BeforeStats = before,
            AfterStats = after,
        };
    }

    private static bool RollLevelUpgrade(decimal successRatePercent)
    {
        var rate = Math.Clamp(successRatePercent, 0m, 100m);
        var rolled = RandomNumberGenerator.GetInt32(0, 10000) / 100m;
        return rolled <= rate;
    }
}

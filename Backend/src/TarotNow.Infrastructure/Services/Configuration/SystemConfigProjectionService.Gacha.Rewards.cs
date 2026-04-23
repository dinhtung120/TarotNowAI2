using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class SystemConfigProjectionService
{
    private void AddRewardRatesForPool(
        GachaPool pool,
        string poolCode,
        GachaPoolConfig configuredPool,
        Dictionary<string, ItemDefinition> itemDefinitions)
    {
        var rewards = configuredPool.Rewards ?? [];
        if (rewards.Count == 0)
        {
            throw new InvalidOperationException($"Gacha pool '{poolCode}' must define at least one reward.");
        }

        var totalProbability = rewards.Sum(x => x.ProbabilityBasisPoints);
        if (totalProbability != 10000)
        {
            throw new InvalidOperationException(
                $"Gacha pool '{poolCode}' has invalid probability total. Expected 10000, actual {totalProbability}.");
        }

        foreach (var reward in rewards)
        {
            AddRewardRate(pool, poolCode, reward, itemDefinitions);
        }
    }

    private void AddRewardRate(
        GachaPool pool,
        string poolCode,
        GachaRewardRateConfig reward,
        Dictionary<string, ItemDefinition> itemDefinitions)
    {
        var rewardKind = NormalizeRequiredLower(reward.RewardKind, nameof(reward.RewardKind));
        var rarity = NormalizeRequired(reward.Rarity, nameof(reward.Rarity));
        var resolvedReward = ResolveReward(reward, rewardKind, poolCode, itemDefinitions);
        var quantity = reward.QuantityGranted <= 0 ? 1 : reward.QuantityGranted;

        _dbContext.GachaPoolRewardRates.Add(new GachaPoolRewardRate(
            poolId: pool.Id,
            rewardKind: rewardKind,
            rarity: rarity,
            probabilityBasisPoints: reward.ProbabilityBasisPoints,
            itemDefinitionId: resolvedReward.ItemDefinitionId,
            currency: resolvedReward.Currency,
            amount: resolvedReward.Amount,
            quantityGranted: quantity,
            iconUrl: resolvedReward.IconUrl,
            nameVi: resolvedReward.NameVi,
            nameEn: resolvedReward.NameEn,
            nameZh: resolvedReward.NameZh,
            isActive: true));
    }

    private static ResolvedReward ResolveReward(
        GachaRewardRateConfig reward,
        string rewardKind,
        string poolCode,
        Dictionary<string, ItemDefinition> itemDefinitions)
    {
        if (string.Equals(rewardKind, "item", StringComparison.OrdinalIgnoreCase))
        {
            return ResolveItemReward(reward, poolCode, itemDefinitions);
        }

        return ResolveCurrencyReward(reward, poolCode);
    }

    private static ResolvedReward ResolveItemReward(
        GachaRewardRateConfig reward,
        string poolCode,
        Dictionary<string, ItemDefinition> itemDefinitions)
    {
        var itemCode = NormalizeRequired(reward.ItemCode, nameof(reward.ItemCode));
        if (!itemDefinitions.TryGetValue(itemCode, out var itemDefinition))
        {
            throw new InvalidOperationException(
                $"Gacha pool '{poolCode}' references missing item definition '{itemCode}'.");
        }

        var iconUrl = string.IsNullOrWhiteSpace(reward.IconUrl) ? itemDefinition.IconUrl : reward.IconUrl.Trim();
        var nameVi = string.IsNullOrWhiteSpace(reward.NameVi) ? itemDefinition.NameVi : reward.NameVi.Trim();
        var nameEn = string.IsNullOrWhiteSpace(reward.NameEn) ? itemDefinition.NameEn : reward.NameEn.Trim();
        var nameZh = string.IsNullOrWhiteSpace(reward.NameZh) ? itemDefinition.NameZh : reward.NameZh.Trim();

        return new ResolvedReward(itemDefinition.Id, null, null, iconUrl, nameVi, nameEn, nameZh);
    }

    private static ResolvedReward ResolveCurrencyReward(GachaRewardRateConfig reward, string poolCode)
    {
        var currency = NormalizeRequiredLower(reward.Currency, nameof(reward.Currency));
        var amount = reward.Amount;
        if (amount is null || amount <= 0)
        {
            throw new InvalidOperationException(
                $"Gacha pool '{poolCode}' has currency reward without positive amount.");
        }

        var iconUrl = string.IsNullOrWhiteSpace(reward.IconUrl) ? null : reward.IconUrl.Trim();
        var nameVi = string.IsNullOrWhiteSpace(reward.NameVi) ? $"Thưởng {currency}" : reward.NameVi.Trim();
        var nameEn = string.IsNullOrWhiteSpace(reward.NameEn) ? $"Reward {currency}" : reward.NameEn.Trim();
        var nameZh = string.IsNullOrWhiteSpace(reward.NameZh) ? $"奖励 {currency}" : reward.NameZh.Trim();

        return new ResolvedReward(null, currency, amount, iconUrl, nameVi, nameEn, nameZh);
    }

    private static bool CanProjectGachaPools(
        IEnumerable<GachaPoolConfig> configuredPools,
        IReadOnlyDictionary<string, ItemDefinition> itemDefinitions,
        out string[] missingItemCodes)
    {
        var missing = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var pool in configuredPools)
        {
            foreach (var reward in pool.Rewards ?? [])
            {
                var rewardKind = NormalizeRequiredLower(reward.RewardKind, nameof(reward.RewardKind));
                if (!string.Equals(rewardKind, "item", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var itemCode = NormalizeRequired(reward.ItemCode, nameof(reward.ItemCode));
                if (!itemDefinitions.ContainsKey(itemCode))
                {
                    missing.Add(itemCode);
                }
            }
        }

        missingItemCodes = missing.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToArray();
        return missingItemCodes.Length == 0;
    }
}

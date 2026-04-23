using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class SystemConfigProjectionService
{
    private async Task ProjectGachaAsync(IReadOnlyDictionary<string, SnapshotItem> configs, CancellationToken cancellationToken)
    {
        if (!configs.TryGetValue("gacha.pools", out var item))
        {
            return;
        }

        var configuredPools = DeserializeList<GachaPoolConfig>(item.Value, "gacha.pools");
        if (configuredPools.Count == 0)
        {
            _logger.LogWarning("System config key 'gacha.pools' is empty. Skip gacha projection.");
            return;
        }

        var itemDefinitions = await LoadActiveItemDefinitionsAsync(cancellationToken);
        if (!CanProjectGachaPools(configuredPools, itemDefinitions, out var missingItemCodes))
        {
            _logger.LogWarning(
                "Skip gacha projection because item definitions are missing: {MissingItemCodes}",
                string.Join(", ", missingItemCodes));
            return;
        }

        var (existingPools, configuredPoolCodes) = await UpsertGachaPoolsAsync(configuredPools, cancellationToken);
        await RebuildGachaRewardRatesAsync(configuredPools, existingPools, configuredPoolCodes, itemDefinitions, cancellationToken);
    }

    private async Task<Dictionary<string, ItemDefinition>> LoadActiveItemDefinitionsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.ItemDefinitions
            .AsNoTracking()
            .Where(x => x.IsActive)
            .ToDictionaryAsync(x => x.Code, x => x, StringComparer.OrdinalIgnoreCase, cancellationToken);
    }

    private async Task<(Dictionary<string, GachaPool> ExistingPools, HashSet<string> ConfiguredPoolCodes)> UpsertGachaPoolsAsync(
        List<GachaPoolConfig> configuredPools,
        CancellationToken cancellationToken)
    {
        var nowUtc = DateTime.UtcNow;
        var existingPools = await _dbContext.GachaPools
            .ToDictionaryAsync(x => x.Code, StringComparer.OrdinalIgnoreCase, cancellationToken);
        var configuredPoolCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var configuredPool in configuredPools)
        {
            UpsertSinglePool(existingPools, configuredPoolCodes, configuredPool, nowUtc);
        }

        foreach (var existingPool in existingPools.Values.Where(pool => !configuredPoolCodes.Contains(pool.Code)))
        {
            existingPool.SetActive(false);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return (existingPools, configuredPoolCodes);
    }

    private void UpsertSinglePool(
        Dictionary<string, GachaPool> existingPools,
        HashSet<string> configuredPoolCodes,
        GachaPoolConfig configuredPool,
        DateTime nowUtc)
    {
        var normalizedCode = NormalizeRequiredLower(configuredPool.Code, nameof(configuredPool.Code));
        configuredPoolCodes.Add(normalizedCode);

        if (existingPools.TryGetValue(normalizedCode, out var existing))
        {
            ApplyPoolConfiguration(existing, configuredPool, nowUtc);
            return;
        }

        var created = CreatePool(normalizedCode, configuredPool, nowUtc);
        _dbContext.GachaPools.Add(created);
        existingPools[normalizedCode] = created;
    }

    private static void ApplyPoolConfiguration(GachaPool existingPool, GachaPoolConfig configuredPool, DateTime nowUtc)
    {
        existingPool.ApplyConfiguration(new GachaPool.Configuration(
            PoolType: NormalizeRequiredLower(configuredPool.PoolType, nameof(configuredPool.PoolType)),
            NameVi: NormalizeRequired(configuredPool.NameVi, nameof(configuredPool.NameVi)),
            NameEn: NormalizeRequired(configuredPool.NameEn, nameof(configuredPool.NameEn)),
            NameZh: NormalizeRequired(configuredPool.NameZh, nameof(configuredPool.NameZh)),
            DescriptionVi: NormalizeRequired(configuredPool.DescriptionVi, nameof(configuredPool.DescriptionVi)),
            DescriptionEn: NormalizeRequired(configuredPool.DescriptionEn, nameof(configuredPool.DescriptionEn)),
            DescriptionZh: NormalizeRequired(configuredPool.DescriptionZh, nameof(configuredPool.DescriptionZh)),
            CostCurrency: NormalizeRequiredLower(configuredPool.CostCurrency, nameof(configuredPool.CostCurrency)),
            CostAmount: configuredPool.CostAmount,
            OddsVersion: NormalizeRequired(configuredPool.OddsVersion, nameof(configuredPool.OddsVersion)),
            PityEnabled: configuredPool.PityEnabled,
            HardPityCount: configuredPool.HardPityCount,
            EffectiveFrom: configuredPool.EffectiveFromUtc ?? nowUtc.AddDays(-1),
            EffectiveTo: configuredPool.EffectiveToUtc,
            IsActive: configuredPool.IsActive));
    }

    private static GachaPool CreatePool(string normalizedCode, GachaPoolConfig configuredPool, DateTime nowUtc)
    {
        return new GachaPool(
            code: normalizedCode,
            poolType: NormalizeRequiredLower(configuredPool.PoolType, nameof(configuredPool.PoolType)),
            nameVi: NormalizeRequired(configuredPool.NameVi, nameof(configuredPool.NameVi)),
            nameEn: NormalizeRequired(configuredPool.NameEn, nameof(configuredPool.NameEn)),
            nameZh: NormalizeRequired(configuredPool.NameZh, nameof(configuredPool.NameZh)),
            descriptionVi: NormalizeRequired(configuredPool.DescriptionVi, nameof(configuredPool.DescriptionVi)),
            descriptionEn: NormalizeRequired(configuredPool.DescriptionEn, nameof(configuredPool.DescriptionEn)),
            descriptionZh: NormalizeRequired(configuredPool.DescriptionZh, nameof(configuredPool.DescriptionZh)),
            costCurrency: NormalizeRequiredLower(configuredPool.CostCurrency, nameof(configuredPool.CostCurrency)),
            costAmount: configuredPool.CostAmount,
            oddsVersion: NormalizeRequired(configuredPool.OddsVersion, nameof(configuredPool.OddsVersion)),
            pityEnabled: configuredPool.PityEnabled,
            hardPityCount: configuredPool.HardPityCount,
            effectiveFrom: configuredPool.EffectiveFromUtc ?? nowUtc.AddDays(-1),
            effectiveTo: configuredPool.EffectiveToUtc,
            isActive: configuredPool.IsActive);
    }

    private async Task RebuildGachaRewardRatesAsync(
        List<GachaPoolConfig> configuredPools,
        Dictionary<string, GachaPool> existingPools,
        HashSet<string> configuredPoolCodes,
        Dictionary<string, ItemDefinition> itemDefinitions,
        CancellationToken cancellationToken)
    {
        var poolIds = existingPools
            .Where(x => configuredPoolCodes.Contains(x.Key))
            .Select(x => x.Value.Id)
            .ToArray();

        await DeactivateExistingRewardRatesAsync(poolIds, cancellationToken);

        foreach (var configuredPool in configuredPools)
        {
            var poolCode = NormalizeRequiredLower(configuredPool.Code, nameof(configuredPool.Code));
            var pool = existingPools[poolCode];
            AddRewardRatesForPool(pool, poolCode, configuredPool, itemDefinitions);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task DeactivateExistingRewardRatesAsync(Guid[] poolIds, CancellationToken cancellationToken)
    {
        if (poolIds.Length == 0)
        {
            return;
        }

        var existingRates = await _dbContext.GachaPoolRewardRates
            .Where(x => poolIds.Contains(x.PoolId) && x.IsActive)
            .ToListAsync(cancellationToken);
        foreach (var existingRate in existingRates)
        {
            existingRate.SetActive(false);
        }
    }
}

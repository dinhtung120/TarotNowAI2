using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Seeds;

/// <summary>
/// Seed dữ liệu pool/rate cho gacha theo schema mới.
/// </summary>
public static partial class GachaSeed
{
    /// <summary>
    /// Seed pool và reward rates gacha nếu chưa tồn tại.
    /// </summary>
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var existingPoolCodes = await context.GachaPools
            .AsNoTracking()
            .Select(x => x.Code)
            .ToListAsync();

        var itemDefinitionMap = await context.ItemDefinitions
            .AsNoTracking()
            .Where(x => x.IsActive)
            .ToDictionaryAsync(x => x.Code, x => x);

        var poolTemplates = BuildPoolTemplates();
        var addedPoolCount = 0;

        foreach (var template in poolTemplates)
        {
            if (existingPoolCodes.Contains(template.Code))
            {
                continue;
            }

            var pool = new GachaPool(
                template.Code,
                template.PoolType,
                template.NameVi,
                template.NameEn,
                template.NameZh,
                template.DescriptionVi,
                template.DescriptionEn,
                template.DescriptionZh,
                template.CostCurrency,
                template.CostAmount,
                template.OddsVersion,
                template.PityEnabled,
                template.HardPityCount,
                template.EffectiveFromUtc,
                template.EffectiveToUtc,
                isActive: true);

            context.GachaPools.Add(pool);
            var rates = BuildRewardRates(pool, template.RewardTemplates, itemDefinitionMap);
            context.GachaPoolRewardRates.AddRange(rates);
            addedPoolCount++;
        }

        if (addedPoolCount > 0)
        {
            await context.SaveChangesAsync();
        }
    }

    private static IReadOnlyList<GachaPoolRewardRate> BuildRewardRates(
        GachaPool pool,
        IReadOnlyCollection<RewardTemplate> templates,
        IReadOnlyDictionary<string, ItemDefinition> itemDefinitionMap)
    {
        var rates = new List<GachaPoolRewardRate>(templates.Count);

        foreach (var template in templates)
        {
            var itemDefinition = template.ItemCode is null
                ? null
                : ResolveItemDefinition(template.ItemCode, itemDefinitionMap);

            rates.Add(new GachaPoolRewardRate(
                pool.Id,
                template.RewardKind,
                template.Rarity,
                template.ProbabilityBasisPoints,
                itemDefinition?.Id,
                template.Currency,
                template.Amount,
                template.QuantityGranted,
                itemDefinition?.IconUrl ?? template.IconUrl,
                itemDefinition?.NameVi ?? template.NameVi,
                itemDefinition?.NameEn ?? template.NameEn,
                itemDefinition?.NameZh ?? template.NameZh,
                isActive: true));
        }

        ValidateProbability(rates, pool.Code);
        return rates;
    }

    private static ItemDefinition ResolveItemDefinition(
        string itemCode,
        IReadOnlyDictionary<string, ItemDefinition> itemDefinitionMap)
    {
        if (!itemDefinitionMap.TryGetValue(itemCode, out var definition))
        {
            throw new InvalidOperationException($"Cannot seed gacha pool because item definition '{itemCode}' is missing.");
        }

        return definition;
    }

    private static void ValidateProbability(IEnumerable<GachaPoolRewardRate> rates, string poolCode)
    {
        var totalProbability = rates.Sum(x => x.ProbabilityBasisPoints);
        if (totalProbability != 10000)
        {
            throw new InvalidOperationException(
                $"Invalid probability setup for gacha pool '{poolCode}'. Total basis points must be 10000, actual: {totalProbability}.");
        }
    }
}

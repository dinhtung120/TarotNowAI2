using TarotNow.Application.Common.Constants;

namespace TarotNow.Infrastructure.Persistence.Seeds;

public static partial class GachaSeed
{
    private sealed record GachaPoolTemplate(
        string Code,
        string PoolType,
        string NameVi,
        string NameEn,
        string NameZh,
        string DescriptionVi,
        string DescriptionEn,
        string DescriptionZh,
        string CostCurrency,
        long CostAmount,
        string OddsVersion,
        bool PityEnabled,
        int HardPityCount,
        DateTime EffectiveFromUtc,
        DateTime? EffectiveToUtc,
        IReadOnlyCollection<RewardTemplate> RewardTemplates);

    private sealed record RewardTemplate(
        string RewardKind,
        string Rarity,
        int ProbabilityBasisPoints,
        string? ItemCode,
        string? Currency,
        long? Amount,
        int QuantityGranted,
        string? IconUrl,
        string NameVi,
        string NameEn,
        string NameZh)
    {
        public static RewardTemplate Item(string rarity, int probabilityBasisPoints, string itemCode, int quantityGranted = 1)
        {
            return new RewardTemplate(
                GachaRewardTypes.Item,
                rarity,
                probabilityBasisPoints,
                itemCode,
                Currency: null,
                Amount: null,
                quantityGranted,
                IconUrl: null,
                NameVi: itemCode,
                NameEn: itemCode,
                NameZh: itemCode);
        }

        public static RewardTemplate CurrencyReward(
            string rarity,
            int probabilityBasisPoints,
            string currency,
            long amount,
            int quantityGranted = 1)
        {
            var normalizedCurrency = currency.Trim().ToLowerInvariant();
            var nameVi = normalizedCurrency == DiamondCurrency ? "Kim Cương" : "Vàng";
            var nameEn = normalizedCurrency == DiamondCurrency ? "Diamond" : "Gold";
            var nameZh = normalizedCurrency == DiamondCurrency ? "钻石" : "金币";

            return new RewardTemplate(
                GachaRewardTypes.Currency,
                rarity,
                probabilityBasisPoints,
                ItemCode: null,
                normalizedCurrency,
                amount,
                quantityGranted,
                IconUrl: null,
                NameVi: nameVi,
                NameEn: nameEn,
                NameZh: nameZh);
        }
    }
}

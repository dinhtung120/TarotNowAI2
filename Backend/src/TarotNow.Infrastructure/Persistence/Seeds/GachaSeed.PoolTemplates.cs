using TarotNow.Application.Common.Constants;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Seeds;

public static partial class GachaSeed
{
    private const string GoldCurrency = "gold";
    private const string DiamondCurrency = "diamond";

    private static List<GachaPoolTemplate> BuildPoolTemplates()
    {
        var nowUtc = DateTime.UtcNow;
        return
        [
            BuildNormalPoolTemplate(nowUtc),
            BuildPremiumPoolTemplate(nowUtc),
            BuildSpecialPoolTemplate(nowUtc),
        ];
    }

    private static GachaPoolTemplate BuildNormalPoolTemplate(DateTime nowUtc)
    {
        return new GachaPoolTemplate(
            Code: "normal",
            PoolType: "normal",
            NameVi: "Kho Báu Thường",
            NameEn: "Normal Treasure Pool",
            NameZh: "普通宝藏池",
            DescriptionVi: "Pool cơ bản với chi phí thấp, tỷ lệ thưởng cân bằng.",
            DescriptionEn: "Baseline pool with low cost and balanced rewards.",
            DescriptionZh: "低成本基础池，奖励概率均衡。",
            CostCurrency: GoldCurrency,
            CostAmount: 500,
            OddsVersion: "gacha-pool-v1",
            PityEnabled: true,
            HardPityCount: 80,
            EffectiveFromUtc: nowUtc.AddDays(-1),
            EffectiveToUtc: null,
            RewardTemplates: BuildNormalRewardTemplates());
    }

    private static GachaPoolTemplate BuildPremiumPoolTemplate(DateTime nowUtc)
    {
        return new GachaPoolTemplate(
            Code: "premium",
            PoolType: "premium",
            NameVi: "Kho Báu Cao Cấp",
            NameEn: "Premium Treasure Pool",
            NameZh: "高级宝藏池",
            DescriptionVi: "Pool cao cấp tập trung vào item hiếm và phần thưởng lớn.",
            DescriptionEn: "Premium pool focused on rare items and higher rewards.",
            DescriptionZh: "高级池，主打稀有道具与高价值奖励。",
            CostCurrency: DiamondCurrency,
            CostAmount: 50,
            OddsVersion: "gacha-pool-v1",
            PityEnabled: true,
            HardPityCount: 70,
            EffectiveFromUtc: nowUtc.AddDays(-1),
            EffectiveToUtc: null,
            RewardTemplates: BuildPremiumRewardTemplates());
    }

    private static GachaPoolTemplate BuildSpecialPoolTemplate(DateTime nowUtc)
    {
        return new GachaPoolTemplate(
            Code: "special",
            PoolType: "special",
            NameVi: "Kho Báu Sự Kiện",
            NameEn: "Special Event Pool",
            NameZh: "活动限定池",
            DescriptionVi: "Pool sự kiện có tỷ lệ vật phẩm huyền thoại cao hơn.",
            DescriptionEn: "Event pool with increased legendary item rates.",
            DescriptionZh: "活动池，传说道具概率更高。",
            CostCurrency: DiamondCurrency,
            CostAmount: 100,
            OddsVersion: "gacha-pool-v1",
            PityEnabled: true,
            HardPityCount: 50,
            EffectiveFromUtc: nowUtc.AddDays(-1),
            EffectiveToUtc: null,
            RewardTemplates: BuildSpecialRewardTemplates());
    }

    private static IReadOnlyCollection<RewardTemplate> BuildNormalRewardTemplates()
    {
        return
        [
            RewardTemplate.CurrencyReward(GachaRarity.Common, 3500, GoldCurrency, 100),
            RewardTemplate.CurrencyReward(GachaRarity.Common, 2300, GoldCurrency, 250),
            RewardTemplate.Item(GachaRarity.Rare, 1800, InventoryItemCodes.ExpBooster),
            RewardTemplate.Item(GachaRarity.Rare, 1200, InventoryItemCodes.DefenseBooster),
            RewardTemplate.Item(GachaRarity.Epic, 700, InventoryItemCodes.FreeDrawTicket),
            RewardTemplate.Item(GachaRarity.Epic, 300, InventoryItemCodes.DailyFortuneScroll),
            RewardTemplate.Item(GachaRarity.Legendary, 150, InventoryItemCodes.MysteryCardPack),
            RewardTemplate.Item(GachaRarity.Legendary, 50, InventoryItemCodes.RareTitleLuckyStar),
        ];
    }

    private static IReadOnlyCollection<RewardTemplate> BuildPremiumRewardTemplates()
    {
        return
        [
            RewardTemplate.CurrencyReward(GachaRarity.Common, 1800, GoldCurrency, 1000),
            RewardTemplate.CurrencyReward(GachaRarity.Rare, 1700, DiamondCurrency, 20),
            RewardTemplate.Item(GachaRarity.Rare, 1400, InventoryItemCodes.PowerBooster),
            RewardTemplate.Item(GachaRarity.Epic, 1200, InventoryItemCodes.LevelUpgrader),
            RewardTemplate.Item(GachaRarity.Epic, 1200, InventoryItemCodes.FreeDrawTicket),
            RewardTemplate.Item(GachaRarity.Legendary, 1500, InventoryItemCodes.MysteryCardPack),
            RewardTemplate.Item(GachaRarity.Legendary, 700, InventoryItemCodes.RareTitleLuckyStar),
            RewardTemplate.Item(GachaRarity.Rare, 500, InventoryItemCodes.ExpBooster),
        ];
    }

    private static IReadOnlyCollection<RewardTemplate> BuildSpecialRewardTemplates()
    {
        return
        [
            RewardTemplate.CurrencyReward(GachaRarity.Rare, 1200, DiamondCurrency, 50),
            RewardTemplate.Item(GachaRarity.Epic, 1800, InventoryItemCodes.LevelUpgrader),
            RewardTemplate.Item(GachaRarity.Legendary, 2800, InventoryItemCodes.MysteryCardPack),
            RewardTemplate.Item(GachaRarity.Legendary, 900, InventoryItemCodes.RareTitleLuckyStar),
            RewardTemplate.Item(GachaRarity.Epic, 1100, InventoryItemCodes.PowerBooster),
            RewardTemplate.Item(GachaRarity.Epic, 1100, InventoryItemCodes.DefenseBooster),
            RewardTemplate.Item(GachaRarity.Epic, 1100, InventoryItemCodes.ExpBooster),
        ];
    }
}

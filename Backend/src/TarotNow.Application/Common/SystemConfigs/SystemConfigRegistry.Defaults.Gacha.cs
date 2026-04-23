using TarotNow.Application.Common.Constants;

namespace TarotNow.Application.Common.SystemConfigs;

public static partial class SystemConfigRegistry
{
    private static object[] BuildDefaultDepositPackages()
    {
        return
        [
            new { code = "topup_50k", amountVnd = 50_000L, baseDiamond = 500L, isActive = true },
            new { code = "topup_100k", amountVnd = 100_000L, baseDiamond = 1_000L, isActive = true },
            new { code = "topup_200k", amountVnd = 200_000L, baseDiamond = 2_000L, isActive = true },
            new { code = "topup_500k", amountVnd = 500_000L, baseDiamond = 5_000L, isActive = true },
            new { code = "topup_1m", amountVnd = 1_000_000L, baseDiamond = 10_000L, isActive = true }
        ];
    }

    private static int[] BuildDefaultFollowupPriceTiers()
    {
        return [1, 2, 4, 8, 16];
    }

    private static object[] BuildDefaultGachaPools()
    {
        return
        [
            BuildNormalGachaPool(),
            BuildPremiumGachaPool(),
            BuildSpecialGachaPool()
        ];
    }

    private static object BuildNormalGachaPool()
    {
        return new
        {
            code = "normal",
            poolType = "normal",
            nameVi = "Kho Báu Thường",
            nameEn = "Normal Treasure Pool",
            nameZh = "普通宝藏池",
            descriptionVi = "Pool cơ bản với chi phí thấp, tỷ lệ thưởng cân bằng.",
            descriptionEn = "Baseline pool with low cost and balanced rewards.",
            descriptionZh = "低成本基础池，奖励概率均衡。",
            costCurrency = "gold",
            costAmount = 500,
            oddsVersion = "gacha-pool-v1",
            pityEnabled = true,
            hardPityCount = 80,
            isActive = true,
            rewards = BuildNormalGachaRewards()
        };
    }

    private static object BuildPremiumGachaPool()
    {
        return new
        {
            code = "premium",
            poolType = "premium",
            nameVi = "Kho Báu Cao Cấp",
            nameEn = "Premium Treasure Pool",
            nameZh = "高级宝藏池",
            descriptionVi = "Pool cao cấp tập trung vào item hiếm và phần thưởng lớn.",
            descriptionEn = "Premium pool focused on rare items and higher rewards.",
            descriptionZh = "高级池，主打稀有道具与高价值奖励。",
            costCurrency = "diamond",
            costAmount = 50,
            oddsVersion = "gacha-pool-v1",
            pityEnabled = true,
            hardPityCount = 70,
            isActive = true,
            rewards = BuildPremiumGachaRewards()
        };
    }

    private static object BuildSpecialGachaPool()
    {
        return new
        {
            code = "special",
            poolType = "special",
            nameVi = "Kho Báu Sự Kiện",
            nameEn = "Special Event Pool",
            nameZh = "活动限定池",
            descriptionVi = "Pool sự kiện có tỷ lệ vật phẩm huyền thoại cao hơn.",
            descriptionEn = "Event pool with increased legendary item rates.",
            descriptionZh = "活动池，传说道具概率更高。",
            costCurrency = "diamond",
            costAmount = 100,
            oddsVersion = "gacha-pool-v1",
            pityEnabled = true,
            hardPityCount = 50,
            isActive = true,
            rewards = BuildSpecialGachaRewards()
        };
    }

    private static object[] BuildNormalGachaRewards()
    {
        return
        [
            CurrencyReward("Common", 3500, "gold", 100),
            CurrencyReward("Common", 2300, "gold", 250),
            ItemReward("Rare", 1800, InventoryItemCodes.ExpBooster),
            ItemReward("Rare", 1200, InventoryItemCodes.DefenseBooster),
            ItemReward("Epic", 700, InventoryItemCodes.FreeDrawTicket3),
            ItemReward("Epic", 300, InventoryItemCodes.FreeDrawTicket5),
            ItemReward("Legendary", 150, InventoryItemCodes.FreeDrawTicket10),
            ItemReward("Legendary", 50, InventoryItemCodes.RareTitleLuckyStar)
        ];
    }

    private static object[] BuildPremiumGachaRewards()
    {
        return
        [
            CurrencyReward("Common", 1800, "gold", 1000),
            CurrencyReward("Rare", 1700, "diamond", 20),
            ItemReward("Rare", 1400, InventoryItemCodes.PowerBooster),
            ItemReward("Epic", 1200, InventoryItemCodes.DefenseBooster),
            ItemReward("Epic", 1200, InventoryItemCodes.ExpBooster),
            ItemReward("Epic", 900, InventoryItemCodes.FreeDrawTicket5),
            ItemReward("Legendary", 800, InventoryItemCodes.FreeDrawTicket10),
            ItemReward("Legendary", 1000, InventoryItemCodes.RareTitleLuckyStar)
        ];
    }

    private static object[] BuildSpecialGachaRewards()
    {
        return
        [
            CurrencyReward("Rare", 1200, "diamond", 50),
            ItemReward("Legendary", 1800, InventoryItemCodes.FreeDrawTicket10),
            ItemReward("Legendary", 2500, InventoryItemCodes.RareTitleLuckyStar),
            ItemReward("Epic", 1100, InventoryItemCodes.PowerBooster),
            ItemReward("Epic", 1100, InventoryItemCodes.DefenseBooster),
            ItemReward("Epic", 1100, InventoryItemCodes.ExpBooster),
            ItemReward("Epic", 1200, InventoryItemCodes.FreeDrawTicket5)
        ];
    }

    private static object CurrencyReward(string rarity, int probabilityBasisPoints, string currency, long amount)
    {
        return new
        {
            rewardKind = "currency",
            rarity,
            probabilityBasisPoints,
            currency,
            amount,
            quantityGranted = 1
        };
    }

    private static object ItemReward(string rarity, int probabilityBasisPoints, string itemCode)
    {
        return new
        {
            rewardKind = "item",
            rarity,
            probabilityBasisPoints,
            itemCode,
            quantityGranted = 1
        };
    }
}

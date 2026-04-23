namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class SystemConfigProjectionService
{
    private sealed record ResolvedReward(
        Guid? ItemDefinitionId,
        string? Currency,
        long? Amount,
        string? IconUrl,
        string NameVi,
        string NameEn,
        string NameZh);

    private sealed class GachaPoolConfig
    {
        public string? Code { get; set; }
        public string? PoolType { get; set; }
        public string? NameVi { get; set; }
        public string? NameEn { get; set; }
        public string? NameZh { get; set; }
        public string? DescriptionVi { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionZh { get; set; }
        public string? CostCurrency { get; set; }
        public long CostAmount { get; set; }
        public string? OddsVersion { get; set; }
        public bool PityEnabled { get; set; }
        public int HardPityCount { get; set; }
        public DateTime? EffectiveFromUtc { get; set; }
        public DateTime? EffectiveToUtc { get; set; }
        public bool IsActive { get; set; } = true;
        public List<GachaRewardRateConfig>? Rewards { get; set; }
    }

    private sealed class GachaRewardRateConfig
    {
        public string? RewardKind { get; set; }
        public string? Rarity { get; set; }
        public int ProbabilityBasisPoints { get; set; }
        public string? ItemCode { get; set; }
        public string? Currency { get; set; }
        public long? Amount { get; set; }
        public int QuantityGranted { get; set; } = 1;
        public string? IconUrl { get; set; }
        public string? NameVi { get; set; }
        public string? NameEn { get; set; }
        public string? NameZh { get; set; }
    }

    private sealed class QuestDefinitionConfig
    {
        public string? Code { get; set; }
        public string? TitleVi { get; set; }
        public string? TitleEn { get; set; }
        public string? TitleZh { get; set; }
        public string? DescriptionVi { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionZh { get; set; }
        public string? QuestType { get; set; }
        public string? TriggerEvent { get; set; }
        public int Target { get; set; }
        public bool IsActive { get; set; } = true;
        public List<QuestRewardConfig>? Rewards { get; set; }
    }

    private sealed class QuestRewardConfig
    {
        public string? Type { get; set; }
        public int Amount { get; set; }
        public string? TitleCode { get; set; }
    }

    private sealed class AchievementDefinitionConfig
    {
        public string? Code { get; set; }
        public string? TitleVi { get; set; }
        public string? TitleEn { get; set; }
        public string? TitleZh { get; set; }
        public string? DescriptionVi { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionZh { get; set; }
        public string? Icon { get; set; }
        public string? GrantsTitleCode { get; set; }
        public bool IsActive { get; set; } = true;
    }

    private sealed class TitleDefinitionConfig
    {
        public string? Code { get; set; }
        public string? NameVi { get; set; }
        public string? NameEn { get; set; }
        public string? NameZh { get; set; }
        public string? DescriptionVi { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionZh { get; set; }
        public string? Rarity { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

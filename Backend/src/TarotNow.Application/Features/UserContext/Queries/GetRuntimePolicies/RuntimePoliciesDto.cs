namespace TarotNow.Application.Features.UserContext.Queries.GetRuntimePolicies;

public sealed class RuntimePoliciesDto
{
    public RuntimeReadingPolicyDto Reading { get; set; } = new();

    public RuntimeFollowupPolicyDto Followup { get; set; } = new();

    public RuntimeWalletPolicyDto Wallet { get; set; } = new();

    public RuntimeReaderPolicyDto Reader { get; set; } = new();

    public RuntimeChatPolicyDto Chat { get; set; } = new();

    public RuntimeGamificationPolicyDto Gamification { get; set; } = new();
}

public sealed class RuntimeReadingPolicyDto
{
    public long Spread3GoldCost { get; set; }
    public long Spread3DiamondCost { get; set; }
    public long Spread5GoldCost { get; set; }
    public long Spread5DiamondCost { get; set; }
    public long Spread10GoldCost { get; set; }
    public long Spread10DiamondCost { get; set; }
}

public sealed class RuntimeFollowupPolicyDto
{
    public int MaxFollowupsAllowed { get; set; }

    public IReadOnlyList<int> PriceTiers { get; set; } = [];

    public IReadOnlyList<RuntimeFollowupFreeSlotThresholdDto> FreeSlotThresholds { get; set; } = [];
}

public sealed class RuntimeFollowupFreeSlotThresholdDto
{
    public int MinHighestCardLevel { get; set; }

    public int FreeSlots { get; set; }
}

public sealed class RuntimeWalletPolicyDto
{
    public long VndPerDiamond { get; set; }

    public long MinWithdrawDiamond { get; set; }

    public decimal WithdrawFeeRate { get; set; }
}

public sealed class RuntimeReaderPolicyDto
{
    public int MinYearsOfExperience { get; set; }

    public long MinDiamondPerQuestion { get; set; }

    public long DefaultDiamondPerQuestion { get; set; }
}

public sealed class RuntimeChatPolicyDto
{
    public int DefaultSlaHours { get; set; }

    public IReadOnlyList<int> AllowedSlaHours { get; set; } = [];

    public int MaxActiveConversationsPerUser { get; set; }
}

public sealed class RuntimeGamificationPolicyDto
{
    public string DefaultQuestType { get; set; } = string.Empty;

    public string DefaultLeaderboardTrack { get; set; } = string.Empty;
}

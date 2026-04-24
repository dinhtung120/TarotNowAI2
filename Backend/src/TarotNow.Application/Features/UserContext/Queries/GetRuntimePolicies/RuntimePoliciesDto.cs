namespace TarotNow.Application.Features.UserContext.Queries.GetRuntimePolicies;

public sealed class RuntimePoliciesDto
{
    public RuntimeReadingPolicyDto Reading { get; set; } = new();

    public RuntimeFollowupPolicyDto Followup { get; set; } = new();

    public RuntimeWalletPolicyDto Wallet { get; set; } = new();

    public RuntimeReaderPolicyDto Reader { get; set; } = new();

    public RuntimeChatPolicyDto Chat { get; set; } = new();

    public RuntimeGamificationPolicyDto Gamification { get; set; } = new();

    public RuntimeAuthPolicyDto Auth { get; set; } = new();

    public RuntimeAdminDisputePolicyDto AdminDispute { get; set; } = new();

    public RuntimeRealtimePolicyDto Realtime { get; set; } = new();

    public RuntimeHttpPolicyDto Http { get; set; } = new();

    public RuntimeRuntimePoliciesClientPolicyDto RuntimePoliciesClient { get; set; } = new();

    public RuntimeUiPolicyDto Ui { get; set; } = new();

    public RuntimeMediaPolicyDto Media { get; set; } = new();
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

    public RuntimeChatPaymentOfferPolicyDto PaymentOffer { get; set; } = new();

    public RuntimeChatHistoryPolicyDto History { get; set; } = new();

    public RuntimeChatParticipantsPolicyDto Participants { get; set; } = new();
}

public sealed class RuntimeChatPaymentOfferPolicyDto
{
    public long DefaultAmount { get; set; }

    public int MaxNoteLength { get; set; }
}

public sealed class RuntimeChatHistoryPolicyDto
{
    public int PageSize { get; set; }
}

public sealed class RuntimeChatParticipantsPolicyDto
{
    public int DefaultPageSize { get; set; }

    public int MaxPageSize { get; set; }
}

public sealed class RuntimeGamificationPolicyDto
{
    public string DefaultQuestType { get; set; } = string.Empty;

    public string DefaultLeaderboardTrack { get; set; } = string.Empty;
}

public sealed class RuntimeAuthPolicyDto
{
    public int MinimumAge { get; set; }
}

public sealed class RuntimeAdminDisputePolicyDto
{
    public int DefaultSplitPercentToReader { get; set; }

    public RuntimeAdminDisputeReaderFreezePolicyDto ReaderFreezePolicy { get; set; } = new();
}

public sealed class RuntimeAdminDisputeReaderFreezePolicyDto
{
    public int LookbackDays { get; set; }

    public int Threshold { get; set; }
}

public sealed class RuntimeRealtimePolicyDto
{
    public IReadOnlyList<int> ReconnectScheduleMs { get; set; } = [];

    public int NegotiationTimeoutMs { get; set; }

    public int PresenceNegotiationCooldownMs { get; set; }

    public int ChatUnauthorizedCooldownMs { get; set; }

    public int ServerTimeoutMs { get; set; }

    public RuntimeRealtimeChatPolicyDto Chat { get; set; } = new();
}

public sealed class RuntimeRealtimeChatPolicyDto
{
    public int TypingClearMs { get; set; }

    public int InvalidateDebounceMs { get; set; }

    public int InitialLoadGuardMs { get; set; }

    public int AppStartGuardMs { get; set; }
}

public sealed class RuntimeHttpPolicyDto
{
    public int ClientTimeoutMs { get; set; }

    public int ServerTimeoutMs { get; set; }

    public int MinTimeoutMs { get; set; }
}

public sealed class RuntimeRuntimePoliciesClientPolicyDto
{
    public int TimeoutMs { get; set; }

    public int StaleMs { get; set; }
}

public sealed class RuntimeUiPolicyDto
{
    public RuntimeUiReadersPolicyDto Readers { get; set; } = new();

    public RuntimeUiSearchPolicyDto Search { get; set; } = new();

    public RuntimeUiPrefetchPolicyDto Prefetch { get; set; } = new();
}

public sealed class RuntimeUiReadersPolicyDto
{
    public int DirectoryPageSize { get; set; }

    public int FeaturedLimit { get; set; }

    public int DirectoryStaleMs { get; set; }
}

public sealed class RuntimeUiSearchPolicyDto
{
    public int DebounceMs { get; set; }
}

public sealed class RuntimeUiPrefetchPolicyDto
{
    public int ChatInboxStaleMs { get; set; }
}

public sealed class RuntimeMediaPolicyDto
{
    public RuntimeMediaUploadPolicyDto Upload { get; set; } = new();
}

public sealed class RuntimeMediaUploadPolicyDto
{
    public long MaxImageBytes { get; set; }

    public long MaxVoiceBytes { get; set; }

    public int MaxVoiceDurationMs { get; set; }

    public long ImageCompressionTargetBytes { get; set; }

    public IReadOnlyList<RuntimeMediaImageCompressionStepDto> ImageCompressionSteps { get; set; } = [];

    public int RetryAttempts { get; set; }

    public int RetryDelayMs { get; set; }
}

public sealed class RuntimeMediaImageCompressionStepDto
{
    public double InitialQuality { get; set; }

    public double MaxSizeMb { get; set; }

    public int MaxWidthOrHeight { get; set; }
}

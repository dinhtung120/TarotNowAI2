using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.UserContext.Queries.GetRuntimePolicies;

public sealed class GetRuntimePoliciesQueryHandler : IRequestHandler<GetRuntimePoliciesQuery, RuntimePoliciesDto>
{
    private readonly ISystemConfigSettings _systemConfigSettings;

    public GetRuntimePoliciesQueryHandler(ISystemConfigSettings systemConfigSettings)
    {
        _systemConfigSettings = systemConfigSettings;
    }

    public Task<RuntimePoliciesDto> Handle(GetRuntimePoliciesQuery request, CancellationToken cancellationToken)
    {
        var result = new RuntimePoliciesDto
        {
            Reading = BuildReadingPolicy(),
            Followup = BuildFollowupPolicy(),
            Wallet = BuildWalletPolicy(),
            Reader = BuildReaderPolicy(),
            Chat = BuildChatPolicy(),
            Gamification = BuildGamificationPolicy(),
            Auth = BuildAuthPolicy(),
            AdminDispute = BuildAdminDisputePolicy(),
            Realtime = BuildRealtimePolicy(),
            Http = BuildHttpPolicy(),
            RuntimePoliciesClient = BuildRuntimePoliciesClientPolicy(),
            Ui = BuildUiPolicy(),
            Media = BuildMediaPolicy()
        };

        return Task.FromResult(result);
    }

    private RuntimeReadingPolicyDto BuildReadingPolicy()
    {
        return new RuntimeReadingPolicyDto
        {
            Spread3GoldCost = _systemConfigSettings.Spread3GoldCost,
            Spread3DiamondCost = _systemConfigSettings.Spread3DiamondCost,
            Spread5GoldCost = _systemConfigSettings.Spread5GoldCost,
            Spread5DiamondCost = _systemConfigSettings.Spread5DiamondCost,
            Spread10GoldCost = _systemConfigSettings.Spread10GoldCost,
            Spread10DiamondCost = _systemConfigSettings.Spread10DiamondCost
        };
    }

    private RuntimeFollowupPolicyDto BuildFollowupPolicy()
    {
        return new RuntimeFollowupPolicyDto
        {
            MaxFollowupsAllowed = _systemConfigSettings.FollowupMaxAllowed,
            PriceTiers = _systemConfigSettings.FollowupPriceTiers,
            FreeSlotThresholds = BuildFollowupFreeSlotThresholds()
        };
    }

    private IReadOnlyList<RuntimeFollowupFreeSlotThresholdDto> BuildFollowupFreeSlotThresholds()
    {
        return
        [
            new RuntimeFollowupFreeSlotThresholdDto
            {
                MinHighestCardLevel = _systemConfigSettings.FollowupFreeSlotThresholdHigh,
                FreeSlots = 3
            },
            new RuntimeFollowupFreeSlotThresholdDto
            {
                MinHighestCardLevel = _systemConfigSettings.FollowupFreeSlotThresholdMid,
                FreeSlots = 2
            },
            new RuntimeFollowupFreeSlotThresholdDto
            {
                MinHighestCardLevel = _systemConfigSettings.FollowupFreeSlotThresholdLow,
                FreeSlots = 1
            }
        ];
    }

    private RuntimeWalletPolicyDto BuildWalletPolicy()
    {
        return new RuntimeWalletPolicyDto
        {
            VndPerDiamond = _systemConfigSettings.EconomyVndPerDiamond,
            MinWithdrawDiamond = _systemConfigSettings.WithdrawalMinDiamond,
            WithdrawFeeRate = _systemConfigSettings.WithdrawalFeeRate
        };
    }

    private RuntimeReaderPolicyDto BuildReaderPolicy()
    {
        var minReaderDiamond = _systemConfigSettings.ReaderMinDiamondPerQuestion;
        return new RuntimeReaderPolicyDto
        {
            MinYearsOfExperience = _systemConfigSettings.ReaderMinYearsOfExperience,
            MinDiamondPerQuestion = minReaderDiamond,
            DefaultDiamondPerQuestion = minReaderDiamond
        };
    }

    private RuntimeChatPolicyDto BuildChatPolicy()
    {
        return new RuntimeChatPolicyDto
        {
            DefaultSlaHours = _systemConfigSettings.ChatDefaultSlaHours,
            AllowedSlaHours = _systemConfigSettings.ChatAllowedSlaHours,
            MaxActiveConversationsPerUser = _systemConfigSettings.ChatMaxActiveConversationsPerUser,
            PaymentOffer = new RuntimeChatPaymentOfferPolicyDto
            {
                DefaultAmount = _systemConfigSettings.ChatPaymentOfferDefaultAmount,
                MaxNoteLength = _systemConfigSettings.ChatPaymentOfferMaxNoteLength
            },
            History = new RuntimeChatHistoryPolicyDto
            {
                PageSize = _systemConfigSettings.ChatHistoryPageSize
            },
            Participants = new RuntimeChatParticipantsPolicyDto
            {
                DefaultPageSize = _systemConfigSettings.ChatParticipantsDefaultPageSize,
                MaxPageSize = _systemConfigSettings.ChatParticipantsMaxPageSize
            }
        };
    }

    private RuntimeGamificationPolicyDto BuildGamificationPolicy()
    {
        return new RuntimeGamificationPolicyDto
        {
            DefaultQuestType = _systemConfigSettings.GamificationDefaultQuestType,
            DefaultLeaderboardTrack = _systemConfigSettings.GamificationDefaultLeaderboardTrack
        };
    }

    private RuntimeAuthPolicyDto BuildAuthPolicy()
    {
        return new RuntimeAuthPolicyDto
        {
            MinimumAge = _systemConfigSettings.LegalMinimumAge
        };
    }

    private RuntimeAdminDisputePolicyDto BuildAdminDisputePolicy()
    {
        return new RuntimeAdminDisputePolicyDto
        {
            DefaultSplitPercentToReader = _systemConfigSettings.AdminDisputeDefaultSplitPercentToReader,
            ReaderFreezePolicy = new RuntimeAdminDisputeReaderFreezePolicyDto
            {
                LookbackDays = _systemConfigSettings.AdminDisputeReaderFreezeLookbackDays,
                Threshold = _systemConfigSettings.AdminDisputeReaderFreezeThreshold
            }
        };
    }

    private RuntimeRealtimePolicyDto BuildRealtimePolicy()
    {
        return new RuntimeRealtimePolicyDto
        {
            ReconnectScheduleMs = _systemConfigSettings.RealtimeReconnectScheduleMs,
            NegotiationTimeoutMs = _systemConfigSettings.RealtimeNegotiationTimeoutMs,
            PresenceNegotiationCooldownMs = _systemConfigSettings.RealtimePresenceNegotiationCooldownMs,
            ChatUnauthorizedCooldownMs = _systemConfigSettings.RealtimeChatUnauthorizedCooldownMs,
            ServerTimeoutMs = _systemConfigSettings.RealtimeServerTimeoutMs,
            Chat = new RuntimeRealtimeChatPolicyDto
            {
                TypingClearMs = _systemConfigSettings.RealtimeChatTypingClearMs,
                InvalidateDebounceMs = _systemConfigSettings.RealtimeChatInvalidateDebounceMs,
                InitialLoadGuardMs = _systemConfigSettings.RealtimeChatInitialLoadGuardMs,
                AppStartGuardMs = _systemConfigSettings.RealtimeChatAppStartGuardMs
            }
        };
    }

    private RuntimeHttpPolicyDto BuildHttpPolicy()
    {
        return new RuntimeHttpPolicyDto
        {
            ClientTimeoutMs = _systemConfigSettings.OperationalHttpClientTimeoutMs,
            ServerTimeoutMs = _systemConfigSettings.OperationalHttpServerTimeoutMs,
            MinTimeoutMs = _systemConfigSettings.OperationalHttpMinTimeoutMs
        };
    }

    private RuntimeRuntimePoliciesClientPolicyDto BuildRuntimePoliciesClientPolicy()
    {
        return new RuntimeRuntimePoliciesClientPolicyDto
        {
            TimeoutMs = _systemConfigSettings.OperationalRuntimePoliciesTimeoutMs,
            StaleMs = _systemConfigSettings.OperationalRuntimePoliciesStaleMs
        };
    }

    private RuntimeUiPolicyDto BuildUiPolicy()
    {
        return new RuntimeUiPolicyDto
        {
            Readers = new RuntimeUiReadersPolicyDto
            {
                DirectoryPageSize = _systemConfigSettings.UiReadersDirectoryPageSize,
                FeaturedLimit = _systemConfigSettings.UiReadersFeaturedLimit,
                DirectoryStaleMs = _systemConfigSettings.UiReadersDirectoryStaleMs
            },
            Search = new RuntimeUiSearchPolicyDto
            {
                DebounceMs = _systemConfigSettings.UiSearchDebounceMs
            },
            Prefetch = new RuntimeUiPrefetchPolicyDto
            {
                ChatInboxStaleMs = _systemConfigSettings.UiPrefetchChatInboxStaleMs
            }
        };
    }

    private RuntimeMediaPolicyDto BuildMediaPolicy()
    {
        return new RuntimeMediaPolicyDto
        {
            Upload = new RuntimeMediaUploadPolicyDto
            {
                MaxImageBytes = _systemConfigSettings.MediaUploadMaxImageBytes,
                MaxVoiceBytes = _systemConfigSettings.MediaUploadMaxVoiceBytes,
                MaxVoiceDurationMs = _systemConfigSettings.MediaUploadMaxVoiceDurationMs,
                ImageCompressionTargetBytes = _systemConfigSettings.MediaUploadImageCompressionTargetBytes,
                ImageCompressionSteps = _systemConfigSettings.MediaUploadImageCompressionSteps
                    .Select(step => new RuntimeMediaImageCompressionStepDto
                    {
                        InitialQuality = step.InitialQuality,
                        MaxSizeMb = step.MaxSizeMb,
                        MaxWidthOrHeight = step.MaxWidthOrHeight
                    })
                    .ToArray(),
                RetryAttempts = _systemConfigSettings.MediaUploadRetryAttempts,
                RetryDelayMs = _systemConfigSettings.MediaUploadRetryDelayMs
            }
        };
    }
}

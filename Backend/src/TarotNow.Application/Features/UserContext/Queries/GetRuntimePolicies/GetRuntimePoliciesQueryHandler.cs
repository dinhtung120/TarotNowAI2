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
            Gamification = BuildGamificationPolicy()
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
            MaxActiveConversationsPerUser = _systemConfigSettings.ChatMaxActiveConversationsPerUser
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
}

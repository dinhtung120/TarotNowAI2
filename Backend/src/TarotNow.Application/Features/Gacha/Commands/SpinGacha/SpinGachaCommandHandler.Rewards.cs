using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public partial class SpinGachaCommandHandler
{
    private async Task ApplyRewardAsync(
        Guid userId,
        GachaBannerItem selectedItem,
        SpinBatchState state,
        CancellationToken cancellationToken)
    {
        if (selectedItem.RewardType == GachaRewardType.Gold)
        {
            state.TotalGoldReward += ParseRewardAmount(selectedItem.RewardValue);
            return;
        }

        if (selectedItem.RewardType == GachaRewardType.Diamond)
        {
            state.TotalDiamondReward += ParseRewardAmount(selectedItem.RewardValue);
            return;
        }

        if (selectedItem.RewardType == GachaRewardType.Title)
        {
            await _titleRepository.GrantTitleAsync(userId, selectedItem.RewardValue, cancellationToken);
        }
    }

    private static long ParseRewardAmount(string rewardValue)
    {
        if (long.TryParse(rewardValue, out var amount))
        {
            return amount;
        }

        throw new BadRequestException($"Invalid gacha reward amount: {rewardValue}");
    }

    private static SpinGachaItemResult ToSpinItemResult(GachaBannerItem item)
    {
        return new SpinGachaItemResult
        {
            Rarity = item.Rarity,
            RewardType = item.RewardType,
            RewardValue = item.RewardValue,
            DisplayNameVi = item.DisplayNameVi,
            DisplayNameEn = item.DisplayNameEn,
            DisplayIcon = item.DisplayIcon
        };
    }
}

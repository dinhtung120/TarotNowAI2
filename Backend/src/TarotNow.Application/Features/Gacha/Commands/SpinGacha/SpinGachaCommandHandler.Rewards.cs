using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public partial class SpinGachaCommandHandler
{
    /// <summary>
    /// Áp dụng phần thưởng của một item vừa quay được vào batch state hoặc hệ thống danh hiệu.
    /// Luồng xử lý: reward type gold/diamond thì cộng dồn amount; reward type title thì grant title ngay cho user.
    /// </summary>
    private async Task ApplyRewardAsync(
        Guid userId,
        GachaBannerItem selectedItem,
        SpinBatchState state,
        CancellationToken cancellationToken)
    {
        if (selectedItem.RewardType == GachaRewardType.Gold)
        {
            // Cộng dồn vàng để credit một lần sau batch.
            state.TotalGoldReward += ParseRewardAmount(selectedItem.RewardValue);
            return;
        }

        if (selectedItem.RewardType == GachaRewardType.Diamond)
        {
            // Cộng dồn kim cương thưởng để credit một lần sau batch.
            state.TotalDiamondReward += ParseRewardAmount(selectedItem.RewardValue);
            return;
        }

        if (selectedItem.RewardType == GachaRewardType.Title)
        {
            await _domainEventPublisher.PublishAsync(
                new TarotNow.Domain.Events.TitleGrantedDomainEvent
                {
                    UserId = userId,
                    TitleCode = selectedItem.RewardValue,
                    Source = "gacha"
                },
                cancellationToken);
        }
    }

    /// <summary>
    /// Parse reward amount từ chuỗi cấu hình item.
    /// Luồng xử lý: parse long thành công thì trả giá trị; nếu không parse được thì ném lỗi cấu hình reward.
    /// </summary>
    private static long ParseRewardAmount(string rewardValue)
    {
        if (long.TryParse(rewardValue, out var amount))
        {
            return amount;
        }

        throw new BadRequestException($"Invalid gacha reward amount: {rewardValue}");
    }

    /// <summary>
    /// Map entity banner item sang DTO kết quả lượt quay.
    /// Luồng xử lý: sao chép rarity/reward/value và metadata hiển thị.
    /// </summary>
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

using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public partial class SpinGachaCommandHandler
{
    private GachaRewardLog BuildRewardLog(
        SpinGachaCommand request,
        GachaBanner banner,
        SpinSelection selection,
        int spinIndex)
    {
        return GachaRewardLog.Create(
            new GachaRewardLogCreateRequest(
                request.UserId,
                banner.Id,
                selection.Item.Id,
                banner.OddsVersion,
                banner.CostDiamond,
                selection.Item.Rarity,
                selection.Item.RewardType,
                selection.Item.RewardValue,
                selection.PityCountAtSpin,
                selection.WasHardPity,
                selection.RngSeed,
                BuildLogIdempotencyKey(request.IdempotencyKey, spinIndex)));
    }

    private async Task InsertAnalyticsLogSafeAsync(
        SpinGachaCommand request,
        GachaBanner banner,
        SpinSelection selection,
        DateTime createdAt,
        CancellationToken cancellationToken)
    {
        try
        {
            await _gachaLogRepository.InsertLogAsync(
                new GachaLogInsertRequest(
                    request.UserId,
                    banner.Code,
                    selection.Item.Rarity,
                    selection.Item.RewardType,
                    selection.Item.RewardValue,
                    banner.CostDiamond,
                    selection.WasHardPity,
                    selection.RngSeed,
                    createdAt),
                cancellationToken);
        }
        catch
        {
            
        }
    }

    private static string BuildLogIdempotencyKey(string baseKey, int spinIndex) => $"{baseKey}_{spinIndex}";
}

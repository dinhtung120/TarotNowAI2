using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public partial class SpinGachaCommandHandler
{
    private async Task<SpinGachaResult?> HandleIdempotentReplayAsync(
        string baseKey,
        int count,
        CancellationToken cancellationToken)
    {
        var firstLogs = await _gachaRepository.GetRewardLogsByIdempotencyKeyAsync(
            BuildLogIdempotencyKey(baseKey, 0),
            cancellationToken);

        if (!firstLogs.Any())
        {
            return null;
        }

        var allLogs = await LoadReplayLogsAsync(baseKey, count, firstLogs, cancellationToken);
        var bannerItems = await _gachaRepository.GetBannerItemsAsync(allLogs[0].BannerId, cancellationToken);

        var replayItems = allLogs
            .Select(log => BuildReplayItemResult(log, bannerItems))
            .ToList();

        return BuildReplayResult(allLogs, replayItems);
    }

    private async Task<List<GachaRewardLog>> LoadReplayLogsAsync(
        string baseKey,
        int count,
        List<GachaRewardLog> firstLogs,
        CancellationToken cancellationToken)
    {
        var allLogs = new List<GachaRewardLog>();
        for (var spinIndex = 0; spinIndex < count; spinIndex++)
        {
            var logs = await _gachaRepository.GetRewardLogsByIdempotencyKeyAsync(
                BuildLogIdempotencyKey(baseKey, spinIndex),
                cancellationToken);

            if (logs.Any())
            {
                allLogs.AddRange(logs);
            }
        }

        return allLogs.Any() ? allLogs : firstLogs;
    }

    private static SpinGachaItemResult BuildReplayItemResult(
        GachaRewardLog log,
        List<GachaBannerItem> bannerItems)
    {
        var item = bannerItems.FirstOrDefault(x => x.Id == log.BannerItemId);
        return new SpinGachaItemResult
        {
            Rarity = log.Rarity,
            RewardType = log.RewardType,
            RewardValue = log.RewardValue,
            DisplayNameVi = item?.DisplayNameVi ?? string.Empty,
            DisplayNameEn = item?.DisplayNameEn ?? string.Empty,
            DisplayIcon = item?.DisplayIcon
        };
    }

    private static SpinGachaResult BuildReplayResult(
        List<GachaRewardLog> allLogs,
        List<SpinGachaItemResult> replayItems)
    {
        var lastLog = allLogs.OrderByDescending(x => x.CreatedAt).First();
        return new SpinGachaResult
        {
            Success = true,
            IsIdempotentReplay = true,
            CurrentPityCount = lastLog.Rarity == GachaRarity.Legendary ? 0 : lastLog.PityCountAtSpin,
            HardPityThreshold = 90,
            WasPityTriggered = allLogs.Any(x => x.WasPityTriggered),
            Items = replayItems
        };
    }
}

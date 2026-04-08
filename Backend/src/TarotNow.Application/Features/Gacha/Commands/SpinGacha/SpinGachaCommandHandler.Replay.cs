using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public partial class SpinGachaCommandHandler
{
    /// <summary>
    /// Thử trả kết quả replay theo idempotency key.
    /// Luồng xử lý: tìm log lượt đầu theo key, nếu có thì nạp toàn bộ log theo count, map item replay và dựng kết quả idempotent.
    /// </summary>
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
        // Dùng banner items để bổ sung metadata hiển thị cho reward replay.
        var bannerItems = await _gachaRepository.GetBannerItemsAsync(allLogs[0].BannerId, cancellationToken);

        var replayItems = allLogs
            .Select(log => BuildReplayItemResult(log, bannerItems))
            .ToList();

        return BuildReplayResult(allLogs, replayItems);
    }

    /// <summary>
    /// Nạp danh sách reward log cho replay theo từng spin index.
    /// Luồng xử lý: duyệt từ 0..count-1 và gom log theo idempotency key con; fallback về firstLogs nếu không tìm thấy log theo index.
    /// </summary>
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

        // Fallback firstLogs để vẫn trả kết quả replay khi dữ liệu index không đầy đủ.
        return allLogs.Any() ? allLogs : firstLogs;
    }

    /// <summary>
    /// Map reward log replay sang item kết quả trả cho API.
    /// Luồng xử lý: tra banner item theo BannerItemId để điền tên/icon hiển thị, fallback rỗng khi item không còn tồn tại.
    /// </summary>
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

    /// <summary>
    /// Dựng kết quả replay cho toàn bộ batch.
    /// Luồng xử lý: lấy log mới nhất để xác định pity hiện tại, tổng hợp cờ pity và gắn danh sách item replay.
    /// </summary>
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

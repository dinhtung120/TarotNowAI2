using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public partial class SpinGachaCommandHandler
{
    /// <summary>
    /// Dựng reward log domain cho một lượt quay.
    /// Luồng xử lý: map thông tin request/banner/selection sang create-request và gắn idempotency key theo spin index.
    /// </summary>
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

    /// <summary>
    /// Ghi analytics log theo best-effort để không ảnh hưởng giao dịch quay chính.
    /// Luồng xử lý: thử insert log; nếu lỗi thì swallow exception vì analytics không phải đường đi quan trọng.
    /// </summary>
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
            // Bỏ qua lỗi analytics để không rollback luồng quay và trả thưởng chính.
        }
    }

    /// <summary>
    /// Dựng idempotency key cho reward log theo từng lượt quay.
    /// Luồng xử lý: ghép base key với chỉ số spin để tạo khóa duy nhất trong batch.
    /// </summary>
    private static string BuildLogIdempotencyKey(string baseKey, int spinIndex) => $"{baseKey}_{spinIndex}";
}

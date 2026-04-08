using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public partial class SpinGachaCommandHandler
{
    /// <summary>
    /// Chọn kết quả cho một lượt quay theo logic pity.
    /// Luồng xử lý: nếu đạt ngưỡng hard pity thì ép chọn trong nhóm Legendary, ngược lại chọn theo trọng số toàn bộ item.
    /// </summary>
    private SpinSelection SelectSpinSelection(SpinExecutionContext context, int currentPity)
    {
        var shouldTriggerHardPity = context.Banner.PityEnabled
            && currentPity >= context.Banner.HardPityCount - 1;

        return shouldTriggerHardPity
            ? SelectHardPityItem(context.Items, currentPity)
            : SelectWeightedItem(context.Items, currentPity);
    }

    /// <summary>
    /// Chọn item khi hard pity được kích hoạt.
    /// Luồng xử lý: lọc item Legendary, nếu chỉ có một item thì chọn thẳng; nếu nhiều item thì bốc thăm weighted trong nhóm Legendary.
    /// </summary>
    private SpinSelection SelectHardPityItem(List<GachaBannerItem> items, int currentPity)
    {
        var legendaryItems = items.Where(item => item.Rarity == GachaRarity.Legendary).ToList();
        if (!legendaryItems.Any())
        {
            // Banner thiếu item Legendary sẽ phá vỡ đảm bảo pity nên phải fail hard.
            throw new InvalidOperationException("Banner missing legendary items for pity.");
        }

        if (legendaryItems.Count == 1)
        {
            // Chỉ có một Legendary thì không cần RNG.
            return BuildPitySelection(legendaryItems[0], currentPity, wasHardPity: true, rngSeed: null);
        }

        var weighted = legendaryItems.Select(item => new WeightedItem
        {
            ItemId = item.Id,
            WeightBasisPoints = item.WeightBasisPoints
        });

        var rngResult = _rngService.WeightedSelect(weighted);
        var selectedItem = legendaryItems.First(item => item.Id == rngResult.SelectedItemId);
        return BuildPitySelection(selectedItem, currentPity, wasHardPity: true, rngResult.RngSeed);
    }

    /// <summary>
    /// Chọn item theo trọng số chuẩn khi không trigger hard pity.
    /// Luồng xử lý: chuyển danh sách item sang WeightedItem, gọi rng weighted select và map ngược item được chọn.
    /// </summary>
    private SpinSelection SelectWeightedItem(List<GachaBannerItem> items, int currentPity)
    {
        var weighted = items.Select(item => new WeightedItem
        {
            ItemId = item.Id,
            WeightBasisPoints = item.WeightBasisPoints
        });

        var rngResult = _rngService.WeightedSelect(weighted);
        var selectedItem = items.First(item => item.Id == rngResult.SelectedItemId);
        return BuildPitySelection(selectedItem, currentPity, wasHardPity: false, rngResult.RngSeed);
    }

    /// <summary>
    /// Dựng SpinSelection kèm cập nhật pity cho lượt quay.
    /// Luồng xử lý: pityCountAtSpin = current+1, nếu trúng Legendary thì reset pity về 0, ngược lại giữ giá trị tăng dần.
    /// </summary>
    private static SpinSelection BuildPitySelection(
        GachaBannerItem selectedItem,
        int currentPity,
        bool wasHardPity,
        string? rngSeed)
    {
        var pityCountAtSpin = currentPity + 1;
        var nextPityCount = selectedItem.Rarity == GachaRarity.Legendary ? 0 : pityCountAtSpin;
        return new SpinSelection(selectedItem, wasHardPity, pityCountAtSpin, nextPityCount, rngSeed);
    }
}

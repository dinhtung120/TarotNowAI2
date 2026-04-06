using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public partial class SpinGachaCommandHandler
{
    private SpinSelection SelectSpinSelection(SpinExecutionContext context, int currentPity)
    {
        var shouldTriggerHardPity = context.Banner.PityEnabled
            && currentPity >= context.Banner.HardPityCount - 1;

        return shouldTriggerHardPity
            ? SelectHardPityItem(context.Items, currentPity)
            : SelectWeightedItem(context.Items, currentPity);
    }

    private SpinSelection SelectHardPityItem(List<GachaBannerItem> items, int currentPity)
    {
        var legendaryItems = items.Where(item => item.Rarity == GachaRarity.Legendary).ToList();
        if (!legendaryItems.Any())
        {
            throw new InvalidOperationException("Banner missing legendary items for pity.");
        }

        if (legendaryItems.Count == 1)
        {
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

using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public partial class SpinGachaCommandHandler
{
    private sealed record SpinExecutionContext(
        GachaBanner Banner,
        List<GachaBannerItem> Items,
        int CurrentPity,
        long TotalCostDiamond);

    private sealed class SpinBatchState
    {
        public SpinBatchState(int currentPity)
        {
            CurrentPity = currentPity;
        }

        public int CurrentPity { get; set; }

        public long TotalGoldReward { get; set; }

        public long TotalDiamondReward { get; set; }

        public bool AnyPityTriggered { get; set; }

        public List<SpinGachaItemResult> Items { get; } = new();
    }

    private sealed record SpinSelection(
        GachaBannerItem Item,
        bool WasHardPity,
        int PityCountAtSpin,
        int NextPityCount,
        string? RngSeed);
}

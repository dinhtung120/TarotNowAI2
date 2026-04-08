using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public partial class SpinGachaCommandHandler
{
    // Context bất biến dùng cho toàn bộ batch quay.
    private sealed record SpinExecutionContext(
        GachaBanner Banner,
        List<GachaBannerItem> Items,
        int CurrentPity,
        long TotalCostDiamond);

    // Trạng thái tích lũy trong quá trình xử lý nhiều lượt quay.
    private sealed class SpinBatchState
    {
        /// <summary>
        /// Khởi tạo trạng thái batch quay.
        /// Luồng xử lý: gán pity hiện tại ban đầu làm mốc cho lượt quay đầu tiên.
        /// </summary>
        public SpinBatchState(int currentPity)
        {
            CurrentPity = currentPity;
        }

        // Pity hiện tại tại thời điểm đang xử lý batch.
        public int CurrentPity { get; set; }

        // Tổng vàng thưởng cộng dồn qua các lượt quay.
        public long TotalGoldReward { get; set; }

        // Tổng kim cương thưởng cộng dồn qua các lượt quay.
        public long TotalDiamondReward { get; set; }

        // Cờ cho biết batch có lượt nào trigger hard pity hay không.
        public bool AnyPityTriggered { get; set; }

        // Danh sách item kết quả theo thứ tự quay.
        public List<SpinGachaItemResult> Items { get; } = new();
    }

    // Kết quả lựa chọn item cho một lượt quay.
    private sealed record SpinSelection(
        GachaBannerItem Item,
        bool WasHardPity,
        int PityCountAtSpin,
        int NextPityCount,
        string? RngSeed);
}

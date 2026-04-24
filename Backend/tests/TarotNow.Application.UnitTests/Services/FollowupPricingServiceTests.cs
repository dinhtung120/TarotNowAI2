using TarotNow.Application.Common.Services;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.UnitTests.Services;

// Unit test cho FollowupPricingService.
public class FollowupPricingServiceTests
{
    // Service thật vì logic thuần tính toán, không cần mock phụ thuộc ngoài.
    private readonly FollowupPricingService _service = new(new TestSystemConfigSettings());

    /// <summary>
    /// Xác nhận JSON cards rỗng trả số slot miễn phí bằng 0.
    /// Luồng này kiểm tra edge case input thiếu dữ liệu.
    /// </summary>
    [Fact]
    public void CalculateFreeSlotsAllowed_WhenJsonMissing_ShouldReturnZero()
    {
        var freeSlots = _service.CalculateFreeSlotsAllowed(string.Empty);

        Assert.Equal(0, freeSlots);
    }

    /// <summary>
    /// Xác nhận có lá cấp cao sẽ mở 3 slot follow-up miễn phí.
    /// Luồng này kiểm tra rule ưu đãi cao nhất theo card tier.
    /// </summary>
    [Fact]
    public void CalculateFreeSlotsAllowed_WhenContainsHighLevelCard_ShouldReturnThree()
    {
        var freeSlots = _service.CalculateFreeSlotsAllowed("[21,22]");

        Assert.Equal(3, freeSlots);
    }

    /// <summary>
    /// Xác nhận có lá cấp trung sẽ mở 2 slot follow-up miễn phí.
    /// Luồng này kiểm tra rule tier trung gian của pricing service.
    /// </summary>
    [Fact]
    public void CalculateFreeSlotsAllowed_WhenContainsMidLevelCard_ShouldReturnTwo()
    {
        var freeSlots = _service.CalculateFreeSlotsAllowed("[1,22]");

        Assert.Equal(2, freeSlots);
    }

    /// <summary>
    /// Xác nhận follow-up trong phạm vi free slots có chi phí bằng 0.
    /// Luồng này bảo vệ chính sách miễn phí giai đoạn đầu.
    /// </summary>
    [Fact]
    public void CalculateNextFollowupCost_WhenWithinFreeSlots_ShouldReturnZero()
    {
        var cost = _service.CalculateNextFollowupCost("[1,22]", currentFollowupCount: 1);

        Assert.Equal(0, cost);
    }

    /// <summary>
    /// Xác nhận vượt free slots sẽ áp dụng mức giá tier đầu tiên.
    /// Luồng này kiểm tra điểm chuyển từ free sang paid.
    /// </summary>
    [Fact]
    public void CalculateNextFollowupCost_WhenPastFreeSlots_ShouldReturnFirstTier()
    {
        var cost = _service.CalculateNextFollowupCost("[22]", currentFollowupCount: 1);

        Assert.Equal(1, cost);
    }

    /// <summary>
    /// Xác nhận gần ngưỡng cap thì chi phí được clamp về tier cao nhất.
    /// Luồng này kiểm tra nhánh anti-overflow trong bảng giá.
    /// </summary>
    [Fact]
    public void CalculateNextFollowupCost_WhenNearCap_ShouldClampToHighestTier()
    {
        var cost = _service.CalculateNextFollowupCost("[]", currentFollowupCount: 4);

        Assert.Equal(16, cost);
    }

    /// <summary>
    /// Xác nhận đạt mức follow-up tối đa sẽ ném exception.
    /// Luồng này bảo vệ hard cap số câu follow-up cho mỗi phiên.
    /// </summary>
    [Fact]
    public void CalculateNextFollowupCost_WhenAtMax_ShouldThrow()
    {
        Assert.Throws<InvalidOperationException>(() =>
            _service.CalculateNextFollowupCost("[]", currentFollowupCount: 5));
    }

    private sealed class TestSystemConfigSettings : ISystemConfigSettings
    {
        public long Spread3GoldCost => 0;
        public long Spread3DiamondCost => 0;
        public long Spread5GoldCost => 0;
        public long Spread5DiamondCost => 0;
        public long Spread10GoldCost => 0;
        public long Spread10DiamondCost => 0;
        public int DailyAiQuota => 10;
        public int InFlightAiCap => 3;
        public int ReadingRateLimitSeconds => 1;
        public long DailyCheckinGold => 100;
        public int StreakFreezeWindowHours => 48;
        public long GachaCostDiamond => 10;
        public IReadOnlyList<int> ChatAllowedSlaHours => [6, 12, 24];
        public int ChatDefaultSlaHours => 12;
        public int ChatMaxActiveConversationsPerUser => 5;
        public long EconomyVndPerDiamond => 100;
        public IReadOnlyList<int> FollowupPriceTiers => [1, 2, 4, 8, 16];
        public int FollowupMaxAllowed => 5;
        public int FollowupFreeSlotThresholdLow => 5;
        public int FollowupFreeSlotThresholdMid => 10;
        public int FollowupFreeSlotThresholdHigh => 20;
        public long WithdrawalMinDiamond => 0;
        public decimal WithdrawalFeeRate => 0.08m;
        public int PresenceTimeoutMinutes => 15;
        public int PresenceScanIntervalSeconds => 60;
        public int EscrowDisputeWindowHours => 48;
        public int EscrowReaderResponseDueHours => 12;
        public int EscrowAutoRefundHours => 72;
    }
}

using TarotNow.Application.Common.Services;

namespace TarotNow.Application.UnitTests.Services;

// Unit test cho FollowupPricingService.
public class FollowupPricingServiceTests
{
    // Service thật vì logic thuần tính toán, không cần mock phụ thuộc ngoài.
    private readonly FollowupPricingService _service = new();

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
}

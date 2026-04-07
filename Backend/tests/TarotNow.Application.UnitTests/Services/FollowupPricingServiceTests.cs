using TarotNow.Application.Common.Services;

namespace TarotNow.Application.UnitTests.Services;

public class FollowupPricingServiceTests
{
    private readonly FollowupPricingService _service = new();

    [Fact]
    public void CalculateFreeSlotsAllowed_WhenJsonMissing_ShouldReturnZero()
    {
        var freeSlots = _service.CalculateFreeSlotsAllowed(string.Empty);

        Assert.Equal(0, freeSlots);
    }

    [Fact]
    public void CalculateFreeSlotsAllowed_WhenContainsHighLevelCard_ShouldReturnThree()
    {
        var freeSlots = _service.CalculateFreeSlotsAllowed("[21,22]");

        Assert.Equal(3, freeSlots);
    }

    [Fact]
    public void CalculateFreeSlotsAllowed_WhenContainsMidLevelCard_ShouldReturnTwo()
    {
        var freeSlots = _service.CalculateFreeSlotsAllowed("[1,22]");

        Assert.Equal(2, freeSlots);
    }

    [Fact]
    public void CalculateNextFollowupCost_WhenWithinFreeSlots_ShouldReturnZero()
    {
        var cost = _service.CalculateNextFollowupCost("[1,22]", currentFollowupCount: 1);

        Assert.Equal(0, cost);
    }

    [Fact]
    public void CalculateNextFollowupCost_WhenPastFreeSlots_ShouldReturnFirstTier()
    {
        var cost = _service.CalculateNextFollowupCost("[22]", currentFollowupCount: 1);

        Assert.Equal(1, cost);
    }

    [Fact]
    public void CalculateNextFollowupCost_WhenNearCap_ShouldClampToHighestTier()
    {
        var cost = _service.CalculateNextFollowupCost("[]", currentFollowupCount: 4);

        Assert.Equal(16, cost);
    }

    [Fact]
    public void CalculateNextFollowupCost_WhenAtMax_ShouldThrow()
    {
        Assert.Throws<InvalidOperationException>(() =>
            _service.CalculateNextFollowupCost("[]", currentFollowupCount: 5));
    }
}

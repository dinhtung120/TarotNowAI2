using FluentAssertions;
using TarotNow.Infrastructure.Services;

namespace TarotNow.Application.UnitTests.Rng;

public class RngServiceTests
{
    private readonly RngService _rngService;

    public RngServiceTests()
    {
        _rngService = new RngService();
    }

    [Fact]
    public void ShuffleDeck_ShouldReturnShuffledArray()
    {
        // Act
        var deck = _rngService.ShuffleDeck(78);

        // Assert
        deck.Should().NotBeNull();
        deck.Length.Should().Be(78);
        deck.Should().OnlyHaveUniqueItems();
    }
}

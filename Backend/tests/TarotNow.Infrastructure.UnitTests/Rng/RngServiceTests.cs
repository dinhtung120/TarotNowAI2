

using FluentAssertions;
using TarotNow.Infrastructure.Services;
using Xunit; 

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
        var deck = _rngService.ShuffleDeck(78);

        deck.Should().NotBeNull();
        deck.Length.Should().Be(78);
        deck.Should().OnlyHaveUniqueItems(); 
    }
}

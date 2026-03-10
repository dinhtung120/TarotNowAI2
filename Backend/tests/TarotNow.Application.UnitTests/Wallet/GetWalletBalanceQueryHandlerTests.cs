using TarotNow.Application.Features.Wallet.Queries;
using TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;
using TarotNow.Domain.Interfaces;
using Moq;
using FluentAssertions;

namespace TarotNow.Application.UnitTests.Wallet;

public class GetWalletBalanceQueryHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly GetWalletBalanceQueryHandler _handler;

    public GetWalletBalanceQueryHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _handler = new GetWalletBalanceQueryHandler(_mockUserRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsWalletBalance_WithCorrectMappings()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new Domain.Entities.User("test@test.com", "testuser", "hash", "DisplayName", DateTime.UtcNow.AddYears(-20), true);
        
        // Use reflection to bypass private setter
        var propertyId = typeof(Domain.Entities.User).GetProperty("Id");
        propertyId?.SetValue(user, userId);
        var propertyGold = typeof(Domain.Entities.User).GetProperty("GoldBalance");
        propertyGold?.SetValue(user, 1000L);
        var propertyDiamond = typeof(Domain.Entities.User).GetProperty("DiamondBalance");
        propertyDiamond?.SetValue(user, 500L);
        var propertyFrozen = typeof(Domain.Entities.User).GetProperty("FrozenDiamondBalance");
        propertyFrozen?.SetValue(user, 50L);

        var query = new GetWalletBalanceQuery(userId);
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.GoldBalance.Should().Be(1000);
        result.DiamondBalance.Should().Be(500);
        result.FrozenDiamondBalance.Should().Be(50);
    }
}

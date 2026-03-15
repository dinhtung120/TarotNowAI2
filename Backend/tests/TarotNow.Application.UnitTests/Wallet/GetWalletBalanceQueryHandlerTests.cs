using TarotNow.Application.Features.Wallet.Queries;
using TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;
using TarotNow.Application.Interfaces;
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
        
        // Dùng reflection để set Id (vẫn là property trên User entity)
        var propertyId = typeof(Domain.Entities.User).GetProperty("Id");
        propertyId?.SetValue(user, userId);

        // Sau SRP refactoring, GoldBalance/DiamondBalance/FrozenDiamondBalance là
        // computed properties delegate → phải set qua Wallet methods thay vì reflection.
        user.Wallet.Credit("gold", 1000, "bonus");
        user.Wallet.Credit("diamond", 550, "bonus"); // Credit 550 vì sẽ freeze 50
        user.Wallet.FreezeDiamond(50); // 550 - 50 = 500 available, 50 frozen

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

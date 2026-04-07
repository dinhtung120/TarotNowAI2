

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
        var userId = Guid.NewGuid();
        var user = new Domain.Entities.User("test@test.com", "testuser", "hash", "DisplayName", DateTime.UtcNow.AddYears(-20), true);
        typeof(Domain.Entities.User).GetProperty("Id")?.SetValue(user, userId);

        
        user.Wallet.Credit("gold", 1000, "bonus");
        user.Wallet.Credit("diamond", 550, "bonus");
        user.Wallet.FreezeDiamond(50); 

        var query = new GetWalletBalanceQuery(userId);
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.GoldBalance.Should().Be(1000);
        result.DiamondBalance.Should().Be(500);       
        result.FrozenDiamondBalance.Should().Be(50);   
    }
}

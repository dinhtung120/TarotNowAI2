

using TarotNow.Application.Features.Wallet.Queries;
using TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;
using TarotNow.Application.Interfaces;
using Moq;
using FluentAssertions;

namespace TarotNow.Application.UnitTests.Wallet;

// Unit test cho query lấy số dư ví người dùng.
public class GetWalletBalanceQueryHandlerTests
{
    // Mock user repository để điều khiển dữ liệu ví đầu vào.
    private readonly Mock<IUserRepository> _mockUserRepository;
    // Handler cần kiểm thử.
    private readonly GetWalletBalanceQueryHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho GetWalletBalanceQueryHandler.
    /// Luồng dùng mock repo để kiểm thử mapping số dư mà không phụ thuộc DB.
    /// </summary>
    public GetWalletBalanceQueryHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _handler = new GetWalletBalanceQueryHandler(_mockUserRepository.Object);
    }

    /// <summary>
    /// Xác nhận request hợp lệ trả đúng số dư gold/diamond/frozen diamond.
    /// Luồng này kiểm tra mapping số dư sau khi credit và freeze trên ví domain.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_ReturnsWalletBalance_WithCorrectMappings()
    {
        var userId = Guid.NewGuid();
        var user = new Domain.Entities.User("test@test.com", "testuser", "hash", "DisplayName", DateTime.UtcNow.AddYears(-20), true);
        typeof(Domain.Entities.User).GetProperty("Id")?.SetValue(user, userId);

        // Seed biến động ví để kiểm tra handler map đúng các bucket số dư.
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

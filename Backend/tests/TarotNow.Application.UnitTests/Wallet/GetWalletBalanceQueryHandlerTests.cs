/*
 * FILE: GetWalletBalanceQueryHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho query handler lấy số dư ví người dùng.
 *
 *   CÁC TEST CASE:
 *   1. Handle_ValidRequest_ReturnsWalletBalance_WithCorrectMappings:
 *      → Credit Gold 1000, Credit Diamond 550, Freeze 50
 *      → GoldBalance=1000, DiamondBalance=500 (550-50), FrozenDiamond=50
 *
 *   WALLET MODEL (sau SRP refactoring):
 *   → GoldBalance, DiamondBalance, FrozenDiamondBalance là computed properties
 *   → DiamondBalance = tổng credit - tổng debit - frozen
 *   → FrozenDiamondBalance = số Diamond đang bị đóng băng trong Escrow
 */

using TarotNow.Application.Features.Wallet.Queries;
using TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;
using TarotNow.Application.Interfaces;
using Moq;
using FluentAssertions;

namespace TarotNow.Application.UnitTests.Wallet;

/// <summary>
/// Test wallet balance: Gold/Diamond/FrozenDiamond mapping after SRP refactoring.
/// </summary>
public class GetWalletBalanceQueryHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly GetWalletBalanceQueryHandler _handler;

    public GetWalletBalanceQueryHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _handler = new GetWalletBalanceQueryHandler(_mockUserRepository.Object);
    }

    /// <summary>
    /// Credit Gold 1000 + Credit Diamond 550 + Freeze 50
    /// → GoldBalance=1000, DiamondBalance=500, FrozenDiamond=50.
    /// Dùng Wallet.Credit/FreezeDiamond thay vì reflection (SRP refactoring).
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_ReturnsWalletBalance_WithCorrectMappings()
    {
        var userId = Guid.NewGuid();
        var user = new Domain.Entities.User("test@test.com", "testuser", "hash", "DisplayName", DateTime.UtcNow.AddYears(-20), true);
        typeof(Domain.Entities.User).GetProperty("Id")?.SetValue(user, userId);

        // Dùng Wallet methods (không reflection) vì SRP refactoring
        user.Wallet.Credit("gold", 1000, "bonus");
        user.Wallet.Credit("diamond", 550, "bonus");
        user.Wallet.FreezeDiamond(50); // 550 - 50 = 500 available

        var query = new GetWalletBalanceQuery(userId);
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.GoldBalance.Should().Be(1000);
        result.DiamondBalance.Should().Be(500);       // 550 - 50 frozen
        result.FrozenDiamondBalance.Should().Be(50);   // Đang bị đóng băng
    }
}

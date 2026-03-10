using TarotNow.Application.Features.Wallet.Queries;
using TarotNow.Application.Features.Wallet.Queries.GetLedgerList;
using TarotNow.Domain.Interfaces;
using TarotNow.Domain.Entities;
using Moq;
using FluentAssertions;

namespace TarotNow.Application.UnitTests.Wallet;

public class GetLedgerListQueryHandlerTests
{
    private readonly Mock<ILedgerRepository> _mockLedgerRepository;
    private readonly GetLedgerListQueryHandler _handler;

    public GetLedgerListQueryHandlerTests()
    {
        _mockLedgerRepository = new Mock<ILedgerRepository>();
        _handler = new GetLedgerListQueryHandler(_mockLedgerRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsPaginatedLedgerList()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetLedgerListQuery(userId, 1, 20);
        
        // Reflection for protected constructor and private setters of WalletTransaction
        var transaction = (WalletTransaction)Activator.CreateInstance(typeof(WalletTransaction), true)!;
        typeof(WalletTransaction).GetProperty("UserId")?.SetValue(transaction, userId);
        typeof(WalletTransaction).GetProperty("Currency")?.SetValue(transaction, TarotNow.Domain.Enums.CurrencyType.Gold);
        typeof(WalletTransaction).GetProperty("Type")?.SetValue(transaction, TarotNow.Domain.Enums.TransactionType.RegisterBonus);
        typeof(WalletTransaction).GetProperty("Amount")?.SetValue(transaction, 50L);
        typeof(WalletTransaction).GetProperty("BalanceAfter")?.SetValue(transaction, 50L);
        
        var transactions = new List<WalletTransaction> { transaction };

        _mockLedgerRepository.Setup(r => r.GetTotalCountAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _mockLedgerRepository.Setup(r => r.GetTransactionsAsync(userId, 1, 20, It.IsAny<CancellationToken>())).ReturnsAsync(transactions);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalCount.Should().Be(1);
        result.Items.Should().HaveCount(1);
        result.Items.First().Type.Should().Be(TarotNow.Domain.Enums.TransactionType.RegisterBonus);
        result.Items.First().Amount.Should().Be(50);
    }
}

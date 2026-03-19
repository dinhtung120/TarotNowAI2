/*
 * FILE: GetLedgerListQueryHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho query handler lấy danh sách giao dịch ví (Ledger/Transaction History).
 *
 *   CÁC TEST CASE:
 *   1. Handle_ValidRequest_ReturnsPaginatedLedgerList:
 *      → Có transactions → trả paginated list, đúng type + amount
 *
 *   PAGINATION: Dùng TotalCount + Page/PageSize chuẩn
 *   MAPPING: WalletTransaction → LedgerItemDto (Currency, Type, Amount, BalanceAfter)
 */

using TarotNow.Application.Features.Wallet.Queries;
using TarotNow.Application.Features.Wallet.Queries.GetLedgerList;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using Moq;
using FluentAssertions;

namespace TarotNow.Application.UnitTests.Wallet;

/// <summary>
/// Test ledger list: pagination, WalletTransaction → LedgerItemDto mapping.
/// </summary>
public class GetLedgerListQueryHandlerTests
{
    private readonly Mock<ILedgerRepository> _mockLedgerRepository;
    private readonly GetLedgerListQueryHandler _handler;

    public GetLedgerListQueryHandlerTests()
    {
        _mockLedgerRepository = new Mock<ILedgerRepository>();
        _handler = new GetLedgerListQueryHandler(_mockLedgerRepository.Object);
    }

    /// <summary>
    /// Có transactions → trả paginated list + mapping đúng (Type, Amount).
    /// Dùng reflection vì WalletTransaction có private setters.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_ReturnsPaginatedLedgerList()
    {
        var userId = Guid.NewGuid();
        var query = new GetLedgerListQuery(userId, 1, 20);

        // Tạo WalletTransaction bằng reflection (entity có protected constructor)
        var transaction = (WalletTransaction)Activator.CreateInstance(typeof(WalletTransaction), true)!;
        typeof(WalletTransaction).GetProperty("UserId")?.SetValue(transaction, userId);
        typeof(WalletTransaction).GetProperty("Currency")?.SetValue(transaction, TarotNow.Domain.Enums.CurrencyType.Gold);
        typeof(WalletTransaction).GetProperty("Type")?.SetValue(transaction, TarotNow.Domain.Enums.TransactionType.RegisterBonus);
        typeof(WalletTransaction).GetProperty("Amount")?.SetValue(transaction, 50L);
        typeof(WalletTransaction).GetProperty("BalanceAfter")?.SetValue(transaction, 50L);

        var transactions = new List<WalletTransaction> { transaction };

        _mockLedgerRepository.Setup(r => r.GetTotalCountAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _mockLedgerRepository.Setup(r => r.GetTransactionsAsync(userId, 1, 20, It.IsAny<CancellationToken>())).ReturnsAsync(transactions);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.TotalCount.Should().Be(1);
        result.Items.Should().HaveCount(1);
        result.Items.First().Type.Should().Be(TarotNow.Domain.Enums.TransactionType.RegisterBonus);
        result.Items.First().Amount.Should().Be(50);
    }
}

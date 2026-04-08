

using TarotNow.Application.Features.Wallet.Queries;
using TarotNow.Application.Features.Wallet.Queries.GetLedgerList;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using Moq;
using FluentAssertions;

namespace TarotNow.Application.UnitTests.Wallet;

// Unit test cho query lấy danh sách ledger của ví.
public class GetLedgerListQueryHandlerTests
{
    // Mock ledger repo để điều khiển dữ liệu giao dịch và tổng số bản ghi.
    private readonly Mock<ILedgerRepository> _mockLedgerRepository;
    // Handler cần kiểm thử.
    private readonly GetLedgerListQueryHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho GetLedgerListQueryHandler.
    /// Luồng dùng mock repository để cô lập logic phân trang/mapping ledger item.
    /// </summary>
    public GetLedgerListQueryHandlerTests()
    {
        _mockLedgerRepository = new Mock<ILedgerRepository>();
        _handler = new GetLedgerListQueryHandler(_mockLedgerRepository.Object);
    }

    /// <summary>
    /// Xác nhận request hợp lệ trả về danh sách ledger phân trang đúng.
    /// Luồng này kiểm tra mapping loại giao dịch và số tiền từ entity sang response item.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_ReturnsPaginatedLedgerList()
    {
        var userId = Guid.NewGuid();
        var query = new GetLedgerListQuery(userId, 1, 20);

        // Tạo transaction mẫu bằng reflection để chủ động set các thuộc tính private/internal.
        var transaction = (WalletTransaction)Activator.CreateInstance(typeof(WalletTransaction), true)!;
        typeof(WalletTransaction).GetProperty("UserId")?.SetValue(transaction, userId);
        typeof(WalletTransaction).GetProperty("Currency")?.SetValue(transaction, TarotNow.Domain.Enums.CurrencyType.Gold);
        typeof(WalletTransaction).GetProperty("Type")?.SetValue(transaction, TarotNow.Domain.Enums.TransactionType.RegisterBonus);
        typeof(WalletTransaction).GetProperty("Amount")?.SetValue(transaction, 50L);
        typeof(WalletTransaction).GetProperty("BalanceAfter")?.SetValue(transaction, 50L);

        var transactions = new List<WalletTransaction> { transaction };

        _mockLedgerRepository.Setup(r => r.GetTotalCountAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _mockLedgerRepository.Setup(r => r.GetTransactionsAsync(userId, 1, 20, It.IsAny<CancellationToken>())).ReturnsAsync(transactions);

        // Gọi handler và xác minh metadata + item đầu ra.
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.TotalCount.Should().Be(1);
        result.Items.Should().HaveCount(1);
        result.Items.First().Type.Should().Be(TarotNow.Domain.Enums.TransactionType.RegisterBonus);
        result.Items.First().Amount.Should().Be(50);
    }
}

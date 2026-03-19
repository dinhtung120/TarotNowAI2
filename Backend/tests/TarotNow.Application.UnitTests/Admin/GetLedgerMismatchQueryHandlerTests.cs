/*
 * FILE: GetLedgerMismatchQueryHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho query handler kiểm tra SỔ CÁI LỆCH (ledger reconciliation).
 *   Đảm bảo handler phát hiện sai lệch giữa balance User và sổ cái wallet_transactions.
 *
 *   CÁC TEST CASE:
 *   1. Handle_WhenMismatchesExist_ReturnsMismatchList:
 *      → Có mismatch (User.Gold=100, Ledger.Gold=90) → trả list 1 phần tử
 *   2. Handle_WhenNoMismatches_ReturnsEmptyList:
 *      → Không có mismatch → trả empty list (KHÔNG phải null)
 *
 *   TẠI SAO QUAN TRỌNG?
 *   → Reconciliation: kiểm tra freeze_sum - release_sum = frozen_balance
 *   → Nếu lệch → BUG tài chính → cần alert Admin
 *   → Handler phải trả empty list khi healthy, KHÔNG được throw exception.
 */

using TarotNow.Application.Features.Admin.Queries;
using TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using Moq;
using FluentAssertions;

namespace TarotNow.Application.UnitTests.Admin;

/// <summary>
/// Test reconciliation query: phát hiện sổ cái lệch vs balance User.
/// </summary>
public class GetLedgerMismatchQueryHandlerTests
{
    private readonly Mock<IAdminRepository> _mockAdminRepository;
    private readonly GetLedgerMismatchQueryHandler _handler;

    public GetLedgerMismatchQueryHandlerTests()
    {
        _mockAdminRepository = new Mock<IAdminRepository>();
        _handler = new GetLedgerMismatchQueryHandler(_mockAdminRepository.Object);
    }

    /// <summary>
    /// Có mismatch: User Gold=100, Ledger Gold=90 → trả list 1 phần tử.
    /// </summary>
    [Fact]
    public async Task Handle_WhenMismatchesExist_ReturnsMismatchList()
    {
        var query = new GetLedgerMismatchQuery();
        var mismatches = new List<MismatchRecord>
        {
            new MismatchRecord { UserId = Guid.NewGuid(), UserGoldBalance = 100, LedgerGoldBalance = 90 }
        };

        _mockAdminRepository.Setup(r => r.GetLedgerMismatchesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mismatches);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().UserGoldBalance.Should().Be(100);
        result.First().LedgerGoldBalance.Should().Be(90);
    }

    /// <summary>
    /// Không có mismatch → empty list (hệ thống healthy).
    /// Quan trọng: handler KHÔNG throw exception khi empty, trả collection rỗng.
    /// </summary>
    [Fact]
    public async Task Handle_WhenNoMismatches_ReturnsEmptyList()
    {
        var query = new GetLedgerMismatchQuery();
        var emptyList = new List<MismatchRecord>();

        _mockAdminRepository.Setup(r => r.GetLedgerMismatchesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyList);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}

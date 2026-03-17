using TarotNow.Application.Features.Admin.Queries;
using TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using Moq;
using FluentAssertions;

namespace TarotNow.Application.UnitTests.Admin;

public class GetLedgerMismatchQueryHandlerTests
{
    private readonly Mock<IAdminRepository> _mockAdminRepository;
    private readonly GetLedgerMismatchQueryHandler _handler;

    public GetLedgerMismatchQueryHandlerTests()
    {
        _mockAdminRepository = new Mock<IAdminRepository>();
        _handler = new GetLedgerMismatchQueryHandler(_mockAdminRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenMismatchesExist_ReturnsMismatchList()
    {
        // Arrange
        var query = new GetLedgerMismatchQuery();
        var mismatches = new List<MismatchRecord>
        {
            new MismatchRecord { UserId = Guid.NewGuid(), UserGoldBalance = 100, LedgerGoldBalance = 90 }
        };

        _mockAdminRepository.Setup(r => r.GetLedgerMismatchesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mismatches);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().UserGoldBalance.Should().Be(100);
        result.First().LedgerGoldBalance.Should().Be(90);
    }

    /// <summary>
    /// TEST CASE: Không có mismatch → trả empty list.
    ///
    /// Map trực tiếp đến Phase 2.3 TEST.md (Reconciliation):
    /// "Trả 0 rows" = frozen_diamond_balance khớp ledger → hệ thống healthy.
    ///
    /// Tại sao test case này quan trọng?
    /// → Đảm bảo handler KHÔNG throw exception khi empty list.
    /// → Confirm query trả empty collection (không phải null).
    /// → Verify reconciliation logic: khi SUM(freeze) - SUM(release/refund) = users.frozen,
    ///   không có bản ghi nào bị mismatch.
    /// </summary>
    [Fact]
    public async Task Handle_WhenNoMismatches_ReturnsEmptyList()
    {
        // Arrange — reconciliation OK, không có dòng nào lệch
        var query = new GetLedgerMismatchQuery();
        var emptyList = new List<MismatchRecord>();

        _mockAdminRepository.Setup(r => r.GetLedgerMismatchesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyList);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert — trả về empty list (không phải null), 0 rows
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}

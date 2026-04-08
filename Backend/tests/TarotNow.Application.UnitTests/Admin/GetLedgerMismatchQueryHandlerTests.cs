

using TarotNow.Application.Features.Admin.Queries;
using TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using Moq;
using FluentAssertions;

namespace TarotNow.Application.UnitTests.Admin;

// Unit test cho query lấy danh sách lệch số dư ví và ledger.
public class GetLedgerMismatchQueryHandlerTests
{
    // Mock repository admin để kiểm soát dữ liệu mismatch trả về.
    private readonly Mock<IAdminRepository> _mockAdminRepository;
    // Handler cần kiểm thử.
    private readonly GetLedgerMismatchQueryHandler _handler;

    /// <summary>
    /// Khởi tạo test fixture cho handler GetLedgerMismatch.
    /// Luồng dùng mock repository để cô lập handler khỏi tầng persistence.
    /// </summary>
    public GetLedgerMismatchQueryHandlerTests()
    {
        _mockAdminRepository = new Mock<IAdminRepository>();
        _handler = new GetLedgerMismatchQueryHandler(_mockAdminRepository.Object);
    }

    /// <summary>
    /// Xác nhận handler trả đúng danh sách khi repository có mismatch.
    /// Luồng setup một record mẫu rồi assert dữ liệu mapping đầu ra.
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

        // Thực thi handler và kiểm tra số lượng/nội dung mismatch.
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().UserGoldBalance.Should().Be(100);
        result.First().LedgerGoldBalance.Should().Be(90);
    }

    /// <summary>
    /// Xác nhận handler trả danh sách rỗng khi repository không có mismatch.
    /// Luồng này đảm bảo handler không phát sinh null khi không có dữ liệu.
    /// </summary>
    [Fact]
    public async Task Handle_WhenNoMismatches_ReturnsEmptyList()
    {
        var query = new GetLedgerMismatchQuery();
        var emptyList = new List<MismatchRecord>();

        _mockAdminRepository.Setup(r => r.GetLedgerMismatchesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyList);

        // Thực thi handler và xác nhận contract trả về list rỗng hợp lệ.
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}

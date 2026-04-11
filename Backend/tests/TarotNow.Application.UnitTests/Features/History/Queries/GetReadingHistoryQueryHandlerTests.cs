

using FluentAssertions;
using Moq;
using TarotNow.Application.Features.History.Queries.GetReadingHistory;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;
using Xunit;
using System.Threading;

namespace TarotNow.Application.UnitTests.Features.History.Queries;

// Unit test cho query lấy lịch sử xem bài phân trang.
public class GetReadingHistoryQueryHandlerTests
{
    // Mock reading session repo để điều khiển dữ liệu phân trang.
    private readonly Mock<IReadingSessionRepository> _mockSessionRepository;
    // Handler cần kiểm thử.
    private readonly GetReadingHistoryQueryHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho GetReadingHistoryQueryHandler.
    /// Luồng dùng mock repository để cô lập logic phân trang/mapping.
    /// </summary>
    public GetReadingHistoryQueryHandlerTests()
    {
        _mockSessionRepository = new Mock<IReadingSessionRepository>();
        _handler = new GetReadingHistoryQueryHandler(_mockSessionRepository.Object);
    }

    /// <summary>
    /// Xác nhận có dữ liệu sẽ trả lịch sử phân trang đúng tổng số bản ghi.
    /// Luồng này kiểm tra mapping trạng thái completed theo session data.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnPaginatedHistory_WhenSessionsExist()
    {
        var userId = Guid.NewGuid();
        var query = new GetReadingHistoryQuery { UserId = userId, Page = 1, PageSize = 10 };

        var session1 = new ReadingSession(userId.ToString(), "Daily1Card");
        session1.CompleteSession("[1]");
        typeof(ReadingSession).GetProperty("CreatedAt")?.SetValue(session1, DateTime.UtcNow.AddMinutes(-10));

        var session2 = new ReadingSession(userId.ToString(), "Daily1Card");
        var sessions = new List<ReadingSession> { session1, session2 };

        _mockSessionRepository.Setup(r => r.GetSessionsByUserIdAsync(userId, 1, 10, It.IsAny<string?>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(((IEnumerable<ReadingSession>)sessions, 2));

        // Gọi handler và assert metadata phân trang cùng danh sách item.
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(2);
        result.TotalPages.Should().Be(1);
        result.Items.Should().HaveCount(2);
        result.Items.First().IsCompleted.Should().BeTrue();
        result.Items.Last().IsCompleted.Should().BeFalse();
    }

    /// <summary>
    /// Xác nhận không có dữ liệu sẽ trả danh sách rỗng hợp lệ.
    /// Luồng này đảm bảo contract response không trả null ở thuộc tính Items.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoSessionsExist()
    {
        var userId = Guid.NewGuid();
        var query = new GetReadingHistoryQuery { UserId = userId, Page = 1, PageSize = 10 };

        _mockSessionRepository.Setup(r => r.GetSessionsByUserIdAsync(userId, 1, 10, It.IsAny<string?>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(((IEnumerable<ReadingSession>)new List<ReadingSession>(), 0));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.TotalCount.Should().Be(0);
        result.Items.Should().BeEmpty();
    }
}

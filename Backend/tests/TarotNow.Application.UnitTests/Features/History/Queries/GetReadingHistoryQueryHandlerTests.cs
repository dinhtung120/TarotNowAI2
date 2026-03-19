/*
 * FILE: GetReadingHistoryQueryHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho query handler lấy lịch sử đọc bài Tarot (phân trang).
 *
 *   CÁC TEST CASE:
 *   1. Handle_ShouldReturnPaginatedHistory_WhenSessionsExist:
 *      → Có sessions → trả paginated list (Page, PageSize, TotalCount, TotalPages)
 *      → Verify mapping: IsCompleted đúng, order đúng
 *   2. Handle_ShouldReturnEmptyList_WhenNoSessionsExist:
 *      → Không có sessions → trả list rỗng, TotalCount=0
 *
 *   PAGINATION: Page/PageSize pattern chuẩn cho mobile + web
 */

using FluentAssertions;
using Moq;
using TarotNow.Application.Features.History.Queries.GetReadingHistory;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.History.Queries;

/// <summary>
/// Test reading history: pagination, mapping IsCompleted, empty list.
/// </summary>
public class GetReadingHistoryQueryHandlerTests
{
    private readonly Mock<IReadingSessionRepository> _mockSessionRepository;
    private readonly GetReadingHistoryQueryHandler _handler;

    public GetReadingHistoryQueryHandlerTests()
    {
        _mockSessionRepository = new Mock<IReadingSessionRepository>();
        _handler = new GetReadingHistoryQueryHandler(_mockSessionRepository.Object);
    }

    /// <summary>
    /// Có sessions → trả paginated list + mapping IsCompleted đúng.
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

        _mockSessionRepository.Setup(r => r.GetSessionsByUserIdAsync(userId, 1, 10, default))
            .ReturnsAsync(((IEnumerable<ReadingSession>)sessions, 2));

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

    /// <summary>Không có sessions → list rỗng, TotalCount=0.</summary>
    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoSessionsExist()
    {
        var userId = Guid.NewGuid();
        var query = new GetReadingHistoryQuery { UserId = userId, Page = 1, PageSize = 10 };

        _mockSessionRepository.Setup(r => r.GetSessionsByUserIdAsync(userId, 1, 10, default))
            .ReturnsAsync(((IEnumerable<ReadingSession>)new List<ReadingSession>(), 0));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.TotalCount.Should().Be(0);
        result.Items.Should().BeEmpty();
    }
}

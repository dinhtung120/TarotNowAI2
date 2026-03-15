using FluentAssertions;
using Moq;
using TarotNow.Application.Features.History.Queries.GetReadingHistory;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.History.Queries;

public class GetReadingHistoryQueryHandlerTests
{
    private readonly Mock<IReadingSessionRepository> _mockSessionRepository;
    private readonly GetReadingHistoryQueryHandler _handler;

    public GetReadingHistoryQueryHandlerTests()
    {
        _mockSessionRepository = new Mock<IReadingSessionRepository>();
        _handler = new GetReadingHistoryQueryHandler(_mockSessionRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedHistory_WhenSessionsExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetReadingHistoryQuery { UserId = userId, Page = 1, PageSize = 10 };

        var session1 = new ReadingSession(userId, "Daily1Card");
        session1.CompleteSession("[1]");
        typeof(ReadingSession).GetProperty("CreatedAt")?.SetValue(session1, DateTime.UtcNow.AddMinutes(-10));

        var session2 = new ReadingSession(userId, "Daily1Card");

        var sessions = new List<ReadingSession> { session1, session2 };

        _mockSessionRepository.Setup(r => r.GetSessionsByUserIdAsync(userId, 1, 10, default))
            .ReturnsAsync(((IEnumerable<ReadingSession>)sessions, 2));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(2);
        result.TotalPages.Should().Be(1);
        result.Items.Should().HaveCount(2);

        // Ensure order and mapping are correct
        result.Items.First().Id.Should().Be(sessions[0].Id);
        result.Items.First().IsCompleted.Should().BeTrue();
        result.Items.Last().IsCompleted.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoSessionsExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetReadingHistoryQuery { UserId = userId, Page = 1, PageSize = 10 };

        _mockSessionRepository.Setup(r => r.GetSessionsByUserIdAsync(userId, 1, 10, default))
            .ReturnsAsync(((IEnumerable<ReadingSession>)new List<ReadingSession>(), 0));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalCount.Should().Be(0);
        result.Items.Should().BeEmpty();
    }
}

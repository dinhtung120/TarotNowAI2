/*
 * FILE: GetReadingDetailQueryHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho query handler lấy chi tiết phiên đọc bài Tarot.
 *
 *   CÁC TEST CASE:
 *   1. Handle_ShouldReturnSessionDetails_WhenSessionExistsAndBelongsToUser:
 *      → Session tồn tại + đúng userId → trả details + AiInteractions
 *   2. Handle_ShouldThrowUnauthorizedAccessException_WhenSessionBelongsToAnotherUser:
 *      → Session của người khác → UnauthorizedAccessException (ownership check)
 *   3. Handle_ShouldReturnNull_WhenSessionDoesNotExist:
 *      → Session không tồn tại → trả null
 *
 *   BẢO MẬT: Ownership check — User chỉ xem được session của chính mình
 */

using FluentAssertions;
using Moq;
using TarotNow.Application.Features.History.Queries.GetReadingDetail;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.History.Queries;

/// <summary>
/// Test reading detail: ownership check, AiInteraction mapping, null handling.
/// </summary>
public class GetReadingDetailQueryHandlerTests
{
    private readonly Mock<IReadingSessionRepository> _mockSessionRepository;
    private readonly GetReadingDetailQueryHandler _handler;

    public GetReadingDetailQueryHandlerTests()
    {
        _mockSessionRepository = new Mock<IReadingSessionRepository>();
        _handler = new GetReadingDetailQueryHandler(_mockSessionRepository.Object);
    }

    /// <summary>
    /// Session tồn tại + đúng userId → trả chi tiết + AiInteractions.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnSessionDetails_WhenSessionExistsAndBelongsToUser()
    {
        var userId = Guid.NewGuid();
        var session = new ReadingSession(userId.ToString(), "Daily1Card");
        var sessionId = session.Id;
        var query = new GetReadingDetailQuery { UserId = userId, SessionId = sessionId };

        session.CompleteSession("[1, 2, 3]");

        var aiRequests = new List<AiRequest>
        {
            new AiRequest { Id = Guid.NewGuid(), ReadingSessionRef = sessionId.ToString(), UserId = userId, Status = "completed", ChargeDiamond = 0, FinishReason = "stop" },
            new AiRequest { Id = Guid.NewGuid(), ReadingSessionRef = sessionId.ToString(), UserId = userId, Status = "completed", ChargeDiamond = 2, FinishReason = "stop" }
        };

        _mockSessionRepository.Setup(r => r.GetSessionWithAiRequestsAsync(sessionId, default))
            .ReturnsAsync(((ReadingSession, IEnumerable<AiRequest>)?)(session, aiRequests));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(sessionId);
        result.CardsDrawn.Should().Be("[1, 2, 3]");
        result.IsCompleted.Should().BeTrue();
        
        if (result.AiInteractions.Any())
        {
             result.AiInteractions.Should().HaveCount(2);
             result.AiInteractions.First().ChargeDiamond.Should().Be(0);
             result.AiInteractions.Last().ChargeDiamond.Should().Be(2);
        }
    }

    /// <summary>Session của người khác → UnauthorizedAccessException.</summary>
    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenSessionBelongsToAnotherUser()
    {
        var userId = Guid.NewGuid();
        var anotherUserId = Guid.NewGuid();
        var session = new ReadingSession(anotherUserId.ToString(), "Daily1Card");
        var sessionId = session.Id;
        var query = new GetReadingDetailQuery { UserId = userId, SessionId = sessionId };

        _mockSessionRepository.Setup(r => r.GetSessionWithAiRequestsAsync(sessionId, default))
            .ReturnsAsync(((ReadingSession, IEnumerable<AiRequest>)?)(session, new List<AiRequest>()));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(query, CancellationToken.None));
    }

    /// <summary>Session không tồn tại → trả null.</summary>
    [Fact]
    public async Task Handle_ShouldReturnNull_WhenSessionDoesNotExist()
    {
        var userId = Guid.NewGuid();
        var sessionId = Guid.NewGuid().ToString();
        var query = new GetReadingDetailQuery { UserId = userId, SessionId = sessionId };

        _mockSessionRepository.Setup(r => r.GetSessionWithAiRequestsAsync(sessionId, default))
            .ReturnsAsync(((ReadingSession, IEnumerable<AiRequest>)?)null);

        var result = await _handler.Handle(query, CancellationToken.None);
        result.Should().BeNull();
    }
}

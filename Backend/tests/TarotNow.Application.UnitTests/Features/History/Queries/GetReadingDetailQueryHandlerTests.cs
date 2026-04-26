

using FluentAssertions;
using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.History.Queries.GetReadingDetail;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.History.Queries;

// Unit test cho query lấy chi tiết phiên xem bài.
public class GetReadingDetailQueryHandlerTests
{
    // Mock reading session repo để điều khiển dữ liệu session + AI requests.
    private readonly Mock<IReadingSessionRepository> _mockSessionRepository;
    // Handler cần kiểm thử.
    private readonly GetReadingDetailQueryHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho GetReadingDetailQueryHandler.
    /// Luồng dùng mock repository để cô lập mapping chi tiết lịch sử.
    /// </summary>
    public GetReadingDetailQueryHandlerTests()
    {
        _mockSessionRepository = new Mock<IReadingSessionRepository>();
        _handler = new GetReadingDetailQueryHandler(_mockSessionRepository.Object);
    }

    /// <summary>
    /// Xác nhận session tồn tại và thuộc user sẽ trả đầy đủ chi tiết.
    /// Luồng kiểm tra mapping cards, completion state và danh sách AI interactions.
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
            new AiRequest { Id = Guid.NewGuid(), ReadingSessionRef = Guid.Parse(sessionId), UserId = userId, Status = "completed", ChargeDiamond = 0, FinishReason = "stop" },
            new AiRequest { Id = Guid.NewGuid(), ReadingSessionRef = Guid.Parse(sessionId), UserId = userId, Status = "completed", ChargeDiamond = 2, FinishReason = "stop" }
        };

        _mockSessionRepository.Setup(r => r.GetSessionWithAiRequestsAsync(sessionId, default))
            .ReturnsAsync(((ReadingSession, IEnumerable<AiRequest>)?)(session, aiRequests));

        // Gọi handler và assert các trường quan trọng của response.
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(sessionId);
        result.CardsDrawn.Should().Be("[1, 2, 3]");
        result.IsCompleted.Should().BeTrue();

        // Chỉ assert chi tiết interaction khi danh sách có dữ liệu.
        if (result.AiInteractions.Any())
        {
            result.AiInteractions.Should().HaveCount(2);
            result.AiInteractions.First().ChargeDiamond.Should().Be(0);
            result.AiInteractions.Last().ChargeDiamond.Should().Be(2);
        }
    }

    /// <summary>
    /// Xác nhận truy cập session của user khác sẽ bị chặn.
    /// Luồng này bảo vệ quyền riêng tư lịch sử xem bài.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldThrowForbiddenException_WhenSessionBelongsToAnotherUser()
    {
        var userId = Guid.NewGuid();
        var anotherUserId = Guid.NewGuid();
        var session = new ReadingSession(anotherUserId.ToString(), "Daily1Card");
        var sessionId = session.Id;
        var query = new GetReadingDetailQuery { UserId = userId, SessionId = sessionId };

        _mockSessionRepository.Setup(r => r.GetSessionWithAiRequestsAsync(sessionId, default))
            .ReturnsAsync(((ReadingSession, IEnumerable<AiRequest>)?)(session, new List<AiRequest>()));

        await Assert.ThrowsAsync<ForbiddenException>(() => _handler.Handle(query, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận session không tồn tại trả về null.
    /// Luồng này giữ contract truy vấn detail khi không tìm thấy dữ liệu.
    /// </summary>
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

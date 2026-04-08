

using FluentAssertions;
using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reading.Commands.RevealSession;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.UnitTests.Reading;

// Unit test cho handler reveal session xem bài.
public class RevealReadingSessionCommandHandlerTests
{
    // Mock reading repo để điều khiển session đầu vào và cập nhật session sau reveal.
    private readonly Mock<IReadingSessionRepository> _readingRepoMock;
    // Mock collection repo để kiểm tra upsert thẻ bài vào bộ sưu tập user.
    private readonly Mock<IUserCollectionRepository> _collectionRepoMock;
    // Mock user repo cho các nhánh cần dữ liệu user.
    private readonly Mock<IUserRepository> _userRepoMock;
    // Mock RNG service để cố định thứ tự bộ bài trong test.
    private readonly Mock<IRngService> _rngMock;
    // Handler cần kiểm thử.
    private readonly RevealReadingSessionCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho RevealReadingSessionCommandHandler.
    /// Luồng dùng RNG mock để assert chính xác các lá bài được reveal.
    /// </summary>
    public RevealReadingSessionCommandHandlerTests()
    {
        _readingRepoMock = new Mock<IReadingSessionRepository>();
        _collectionRepoMock = new Mock<IUserCollectionRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _rngMock = new Mock<IRngService>();
        _handler = new RevealReadingSessionCommandHandler(
            _readingRepoMock.Object, _collectionRepoMock.Object,
            _userRepoMock.Object, _rngMock.Object);
    }

    /// <summary>
    /// Xác nhận session hợp lệ trả đúng các lá bài và upsert collection tương ứng.
    /// Luồng này kiểm tra cả output cards và side-effect update persistence.
    /// </summary>
    [Fact]
    public async Task Handle_ValidSession_ShouldReturnShuffledCards_AndUpsertCollection()
    {
        var userId = Guid.NewGuid();
        var request = new RevealReadingSessionCommand { UserId = userId, SessionId = Guid.NewGuid().ToString() };
        var mockSession = new ReadingSession(userId.ToString(), SpreadType.Spread3Cards);
        typeof(ReadingSession).GetProperty("Id")?.SetValue(mockSession, request.SessionId);

        _readingRepoMock.Setup(r => r.GetByIdAsync(request.SessionId, CancellationToken.None)).ReturnsAsync(mockSession);

        // Cố định deck để assert deterministic thứ tự 3 lá đầu.
        var deck = Enumerable.Range(0, 78).Reverse().ToArray();
        _rngMock.Setup(r => r.ShuffleDeck(78)).Returns(deck);

        // Thực thi reveal và kiểm tra kết quả đầu ra.
        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Cards.Should().HaveCount(3);
        result.Cards[0].Should().Be(77);
        result.Cards[1].Should().Be(76);
        result.Cards[2].Should().Be(75);

        // Mỗi lá được reveal phải được upsert vào bộ sưu tập user đúng 1 lần.
        _collectionRepoMock.Verify(c => c.UpsertCardAsync(userId, 77, 1, CancellationToken.None), Times.Once);
        _collectionRepoMock.Verify(c => c.UpsertCardAsync(userId, 76, 1, CancellationToken.None), Times.Once);
        _collectionRepoMock.Verify(c => c.UpsertCardAsync(userId, 75, 1, CancellationToken.None), Times.Once);

        // Session phải được đánh dấu completed sau khi reveal thành công.
        _readingRepoMock.Verify(r => r.UpdateAsync(It.Is<ReadingSession>(s => s.IsCompleted == true), CancellationToken.None), Times.Once);
    }

    /// <summary>
    /// Xác nhận session đã completed thì không cho reveal lại.
    /// Luồng này bảo vệ idempotency và tính toàn vẹn của dữ liệu reading session.
    /// </summary>
    [Fact]
    public async Task Handle_CompletedSession_ShouldThrowBadRequest()
    {
        var userId = Guid.NewGuid();
        var request = new RevealReadingSessionCommand { UserId = userId, SessionId = Guid.NewGuid().ToString() };
        var mockSession = new ReadingSession(userId.ToString(), SpreadType.Daily1Card);
        mockSession.CompleteSession("[10]");

        _readingRepoMock.Setup(r => r.GetByIdAsync(request.SessionId, CancellationToken.None)).ReturnsAsync(mockSession);

        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("This session has already been revealed");
    }
}

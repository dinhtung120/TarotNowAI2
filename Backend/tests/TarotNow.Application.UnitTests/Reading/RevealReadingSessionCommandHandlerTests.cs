using FluentAssertions;
using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reading.Commands.RevealSession;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.UnitTests.Reading;

public class RevealReadingSessionCommandHandlerTests
{
    private readonly Mock<IReadingSessionRepository> _readingRepoMock;
    private readonly Mock<IUserCollectionRepository> _collectionRepoMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IRngService> _rngMock;
    private readonly RevealReadingSessionCommandHandler _handler;

    public RevealReadingSessionCommandHandlerTests()
    {
        _readingRepoMock = new Mock<IReadingSessionRepository>();
        _collectionRepoMock = new Mock<IUserCollectionRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _rngMock = new Mock<IRngService>();
        _handler = new RevealReadingSessionCommandHandler(_readingRepoMock.Object, _collectionRepoMock.Object, _userRepoMock.Object, _rngMock.Object);
    }

    [Fact]
    public async Task Handle_ValidSession_ShouldReturnShuffledCards_AndUpsertCollection()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new RevealReadingSessionCommand { UserId = userId, SessionId = Guid.NewGuid().ToString() };
        
        var mockSession = new ReadingSession(userId.ToString(), SpreadType.Spread3Cards);
        typeof(ReadingSession).GetProperty("Id")?.SetValue(mockSession, request.SessionId);

        _readingRepoMock.Setup(r => r.GetByIdAsync(request.SessionId, CancellationToken.None))
            .ReturnsAsync(mockSession);

        // Giả lập hàm Shuffle trả về mảng 78 index từ 0 -> 77
        var deck = Enumerable.Range(0, 78).Reverse().ToArray(); // [77, 76, 75...] Dùng hàm đảo để dễ test
        _rngMock.Setup(r => r.ShuffleDeck(78))
            .Returns(deck);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Cards.Should().HaveCount(3); // Do là Spread3Cards => Lấy 3 lá đầu
        result.Cards[0].Should().Be(77);
        result.Cards[1].Should().Be(76);
        result.Cards[2].Should().Be(75);

        // Đảm bảo Repo Update Kho đồ bị gọi 3 lượt tương ứng với 3 Cards rút được
        _collectionRepoMock.Verify(c => c.UpsertCardAsync(userId, 77, 1, CancellationToken.None), Times.Once);
        _collectionRepoMock.Verify(c => c.UpsertCardAsync(userId, 76, 1, CancellationToken.None), Times.Once);
        _collectionRepoMock.Verify(c => c.UpsertCardAsync(userId, 75, 1, CancellationToken.None), Times.Once);

        // Đảm bảo Session được Mark As Completed
        _readingRepoMock.Verify(r => r.UpdateAsync(It.Is<ReadingSession>(s => s.IsCompleted == true), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_CompletedSession_ShouldThrowBadRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new RevealReadingSessionCommand { UserId = userId, SessionId = Guid.NewGuid().ToString() };
        
        var mockSession = new ReadingSession(userId.ToString(), SpreadType.Daily1Card);
        mockSession.CompleteSession("[10]"); // Giả lập đã gọi trước đó

        _readingRepoMock.Setup(r => r.GetByIdAsync(request.SessionId, CancellationToken.None))
            .ReturnsAsync(mockSession);

        // Act & Assert
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("This session has already been revealed");
    }
}

/*
 * FILE: RevealReadingSessionCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler lật bài Tarot (Reveal Reading Session).
 *
 *   CÁC TEST CASE:
 *   1. Handle_ValidSession_ShouldReturnShuffledCards_AndUpsertCollection:
 *      → Shuffle deck 78 lá → lấy N lá đầu (theo SpreadType) → upsert collection
 *   2. Handle_CompletedSession_ShouldThrowBadRequest:
 *      → Session đã reveal → BadRequest (không cho lật 2 lần)
 *
 *   LOGIC: IRngService.ShuffleDeck(78) → lấy cards[0..N-1] → UpsertCardAsync cho mỗi lá
 *   → Session.CompleteSession() mark IsCompleted=true
 */

using FluentAssertions;
using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reading.Commands.RevealSession;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.UnitTests.Reading;

/// <summary>
/// Test reveal session: shuffle deck, card collection upsert, double-reveal guard.
/// </summary>
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
        _handler = new RevealReadingSessionCommandHandler(
            _readingRepoMock.Object, _collectionRepoMock.Object,
            _userRepoMock.Object, _rngMock.Object);
    }

    /// <summary>
    /// Shuffle 78 lá → lấy 3 lá đầu (Spread3Cards) → upsert collection.
    /// Verify: cards đúng, collection upsert 3 lần, session marked completed.
    /// </summary>
    [Fact]
    public async Task Handle_ValidSession_ShouldReturnShuffledCards_AndUpsertCollection()
    {
        var userId = Guid.NewGuid();
        var request = new RevealReadingSessionCommand { UserId = userId, SessionId = Guid.NewGuid().ToString() };
        var mockSession = new ReadingSession(userId.ToString(), SpreadType.Spread3Cards);
        typeof(ReadingSession).GetProperty("Id")?.SetValue(mockSession, request.SessionId);

        _readingRepoMock.Setup(r => r.GetByIdAsync(request.SessionId, CancellationToken.None)).ReturnsAsync(mockSession);
        // Deck đảo ngược [77, 76, 75, ...] để dễ assert
        var deck = Enumerable.Range(0, 78).Reverse().ToArray();
        _rngMock.Setup(r => r.ShuffleDeck(78)).Returns(deck);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Cards.Should().HaveCount(3);
        result.Cards[0].Should().Be(77);
        result.Cards[1].Should().Be(76);
        result.Cards[2].Should().Be(75);

        // Verify upsert collection cho mỗi lá bốc được
        _collectionRepoMock.Verify(c => c.UpsertCardAsync(userId, 77, 1, CancellationToken.None), Times.Once);
        _collectionRepoMock.Verify(c => c.UpsertCardAsync(userId, 76, 1, CancellationToken.None), Times.Once);
        _collectionRepoMock.Verify(c => c.UpsertCardAsync(userId, 75, 1, CancellationToken.None), Times.Once);

        // Verify session marked as completed
        _readingRepoMock.Verify(r => r.UpdateAsync(It.Is<ReadingSession>(s => s.IsCompleted == true), CancellationToken.None), Times.Once);
    }

    /// <summary>Session đã reveal → BadRequest (không cho lật 2 lần).</summary>
    [Fact]
    public async Task Handle_CompletedSession_ShouldThrowBadRequest()
    {
        var userId = Guid.NewGuid();
        var request = new RevealReadingSessionCommand { UserId = userId, SessionId = Guid.NewGuid().ToString() };
        var mockSession = new ReadingSession(userId.ToString(), SpreadType.Daily1Card);
        mockSession.CompleteSession("[10]"); // Đã reveal

        _readingRepoMock.Setup(r => r.GetByIdAsync(request.SessionId, CancellationToken.None)).ReturnsAsync(mockSession);

        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("This session has already been revealed");
    }
}

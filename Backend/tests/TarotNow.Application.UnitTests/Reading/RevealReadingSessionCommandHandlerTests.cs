

using FluentAssertions;
using Moq;
using TarotNow.Application.Features.Reading.Commands.RevealSession;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.UnitTests.Reading;

// Unit test cho handler reveal session xem bài.
public class RevealReadingSessionCommandHandlerTests
{
    // Mock inline dispatcher để xác nhận command chỉ publish event.
    private readonly Mock<IInlineDomainEventDispatcher> _inlineDispatcherMock;
    // Handler cần kiểm thử.
    private readonly RevealReadingSessionCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho RevealReadingSessionCommandHandler.
    /// Luồng dùng RNG mock để assert chính xác các lá bài được reveal.
    /// </summary>
    public RevealReadingSessionCommandHandlerTests()
    {
        _inlineDispatcherMock = new Mock<IInlineDomainEventDispatcher>();
        _handler = new RevealReadingSessionCommandHandler(_inlineDispatcherMock.Object);
    }

    /// <summary>
    /// Xác nhận command publish đúng domain event và map output cards từ event.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldPublishRevealRequestedDomainEvent_AndReturnCardsFromEvent()
    {
        var userId = Guid.NewGuid();
        var sessionId = Guid.NewGuid().ToString();
        var request = new RevealReadingSessionCommand
        {
            UserId = userId,
            SessionId = sessionId,
            Language = "en"
        };

        _inlineDispatcherMock
            .Setup(x => x.PublishAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Callback<IDomainEvent, CancellationToken>((domainEvent, _) =>
            {
                var revealEvent = domainEvent.Should().BeOfType<ReadingSessionRevealRequestedDomainEvent>().Subject;
                revealEvent.RevealedCards = new[]
                {
                    new ReadingDrawnCard
                    {
                        CardId = 12,
                        Position = 0,
                        Orientation = CardOrientation.Upright
                    },
                    new ReadingDrawnCard
                    {
                        CardId = 55,
                        Position = 1,
                        Orientation = CardOrientation.Reversed
                    }
                };
            })
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Cards.Should().HaveCount(2);
        result.Cards[0].CardId.Should().Be(12);
        result.Cards[1].Orientation.Should().Be(CardOrientation.Reversed);

        _inlineDispatcherMock.Verify(
            x => x.PublishAsync(
                It.Is<ReadingSessionRevealRequestedDomainEvent>(e =>
                    e.UserId == userId
                    && e.SessionId == sessionId
                    && e.Language == "en"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}

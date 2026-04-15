

using FluentAssertions;
using Moq;
using TarotNow.Application.Features.Reading.Commands.InitSession;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.UnitTests.Reading;

// Unit test cho handler khởi tạo phiên xem bài.
public class InitReadingSessionCommandHandlerTests
{
    // Mock inline dispatcher để xác nhận command chỉ publish event.
    private readonly Mock<IInlineDomainEventDispatcher> _inlineDispatcherMock;
    // Handler cần kiểm thử.
    private readonly InitReadingSessionCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho InitReadingSessionCommandHandler.
    /// Luồng setup giá spread và entitlement mặc định giúp test ổn định theo policy hiện tại.
    /// </summary>
    public InitReadingSessionCommandHandlerTests()
    {
        _inlineDispatcherMock = new Mock<IInlineDomainEventDispatcher>();
        _handler = new InitReadingSessionCommandHandler(_inlineDispatcherMock.Object);
    }

    /// <summary>
    /// Xác nhận command publish đúng domain event và trả data từ event handler.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldPublishInitRequestedDomainEvent_AndReturnResult()
    {
        var userId = Guid.NewGuid();
        var request = new InitReadingSessionCommand
        {
            UserId = userId,
            SpreadType = "spread_3",
            Question = "Career path?",
            Currency = "diamond"
        };

        _inlineDispatcherMock
            .Setup(x => x.PublishAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Callback<IDomainEvent, CancellationToken>((domainEvent, _) =>
            {
                var initEvent = domainEvent.Should().BeOfType<ReadingSessionInitRequestedDomainEvent>().Subject;
                initEvent.SessionId = "session_123";
                initEvent.CostGold = 0;
                initEvent.CostDiamond = 5;
            })
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.SessionId.Should().Be("session_123");
        result.CostGold.Should().Be(0);
        result.CostDiamond.Should().Be(5);

        _inlineDispatcherMock.Verify(
            x => x.PublishAsync(
                It.Is<ReadingSessionInitRequestedDomainEvent>(e =>
                    e.UserId == userId
                    && e.SpreadType == "spread_3"
                    && e.Question == "Career path?"
                    && e.Currency == "diamond"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}

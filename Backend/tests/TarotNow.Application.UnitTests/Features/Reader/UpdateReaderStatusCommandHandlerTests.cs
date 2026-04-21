using Moq;
using TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Reader;

/// <summary>
/// Unit tests cho UpdateReaderStatusCommandHandler.
/// </summary>
public class UpdateReaderStatusCommandHandlerTests
{
    private readonly Mock<IInlineDomainEventDispatcher> _mockDispatcher;
    private readonly UpdateReaderStatusCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture test.
    /// </summary>
    public UpdateReaderStatusCommandHandlerTests()
    {
        _mockDispatcher = new Mock<IInlineDomainEventDispatcher>();
        _handler = new UpdateReaderStatusCommandHandler(_mockDispatcher.Object);
    }

    /// <summary>
    /// Xác nhận handler publish event và trả kết quả hydrated.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldPublishEventAndReturnHydratedResult()
    {
        var command = new UpdateReaderStatusCommand
        {
            UserId = Guid.NewGuid(),
            Status = "busy"
        };

        _mockDispatcher
            .Setup(x => x.PublishAsync(It.IsAny<ReaderStatusUpdateRequestedDomainEvent>(), It.IsAny<CancellationToken>()))
            .Callback((IDomainEvent domainEvent, CancellationToken _) =>
            {
                var statusEvent = Assert.IsType<ReaderStatusUpdateRequestedDomainEvent>(domainEvent);
                statusEvent.Updated = true;
            })
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        _mockDispatcher.Verify(x => x.PublishAsync(It.Is<ReaderStatusUpdateRequestedDomainEvent>(e =>
            e.UserId == command.UserId && e.Status == command.Status), It.IsAny<CancellationToken>()), Times.Once);
    }
}

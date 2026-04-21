using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Admin.Commands.ApproveReader;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Admin;

/// <summary>
/// Unit tests cho ApproveReaderCommandHandler.
/// </summary>
public class ApproveReaderCommandHandlerTests
{
    private readonly Mock<IInlineDomainEventDispatcher> _mockDispatcher;
    private readonly ApproveReaderCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture test.
    /// </summary>
    public ApproveReaderCommandHandlerTests()
    {
        _mockDispatcher = new Mock<IInlineDomainEventDispatcher>();
        _handler = new ApproveReaderCommandHandler(_mockDispatcher.Object);
    }

    /// <summary>
    /// Xác nhận action không hợp lệ ném lỗi.
    /// </summary>
    [Fact]
    public async Task Handle_InvalidAction_ShouldThrowBadRequest()
    {
        var command = new ApproveReaderCommand
        {
            RequestId = "request-id",
            Action = "invalid"
        };

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        _mockDispatcher.Verify(x => x.PublishAsync(It.IsAny<ReaderRequestReviewRequestedDomainEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    /// <summary>
    /// Xác nhận handler publish đúng event và trả kết quả hydrated.
    /// </summary>
    [Fact]
    public async Task Handle_ValidAction_ShouldPublishEventAndReturnResult()
    {
        var command = new ApproveReaderCommand
        {
            RequestId = "request-id",
            Action = "approve",
            AdminNote = "ok",
            AdminId = Guid.NewGuid()
        };

        _mockDispatcher
            .Setup(x => x.PublishAsync(It.IsAny<ReaderRequestReviewRequestedDomainEvent>(), It.IsAny<CancellationToken>()))
            .Callback((IDomainEvent domainEvent, CancellationToken _) =>
            {
                var reviewEvent = Assert.IsType<ReaderRequestReviewRequestedDomainEvent>(domainEvent);
                reviewEvent.Processed = true;
            })
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        _mockDispatcher.Verify(x => x.PublishAsync(It.Is<ReaderRequestReviewRequestedDomainEvent>(e =>
            e.RequestId == command.RequestId
            && e.Action == "approve"
            && e.AdminNote == command.AdminNote
            && e.AdminId == command.AdminId), It.IsAny<CancellationToken>()), Times.Once);
    }
}

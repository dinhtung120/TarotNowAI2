using Moq;
using TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Reader;

/// <summary>
/// Unit tests cho SubmitReaderRequestCommandHandler.
/// </summary>
public class SubmitReaderRequestCommandHandlerTests
{
    private readonly Mock<IInlineDomainEventDispatcher> _mockDispatcher;
    private readonly SubmitReaderRequestCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture test.
    /// </summary>
    public SubmitReaderRequestCommandHandlerTests()
    {
        _mockDispatcher = new Mock<IInlineDomainEventDispatcher>();
        _handler = new SubmitReaderRequestCommandHandler(_mockDispatcher.Object);
    }

    /// <summary>
    /// Xác nhận command handler publish đúng event và trả kết quả hydrated.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldPublishEventAndReturnHydratedResult()
    {
        var command = new SubmitReaderRequestCommand
        {
            UserId = Guid.NewGuid(),
            Bio = "Reader bio content is long enough for validation.",
            Specialties = ["love", "career"],
            YearsOfExperience = 3,
            FacebookUrl = "https://facebook.com/tarot.reader",
            DiamondPerQuestion = 120,
            ProofDocuments = ["proof-1"]
        };

        _mockDispatcher
            .Setup(x => x.PublishAsync(It.IsAny<ReaderRequestSubmitRequestedDomainEvent>(), It.IsAny<CancellationToken>()))
            .Callback((IDomainEvent domainEvent, CancellationToken _) =>
            {
                var submitEvent = Assert.IsType<ReaderRequestSubmitRequestedDomainEvent>(domainEvent);
                submitEvent.Submitted = true;
            })
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        _mockDispatcher.Verify(x => x.PublishAsync(It.Is<ReaderRequestSubmitRequestedDomainEvent>(e =>
            e.UserId == command.UserId
            && e.Bio == command.Bio
            && e.Specialties.SequenceEqual(command.Specialties)
            && e.YearsOfExperience == command.YearsOfExperience
            && e.FacebookUrl == command.FacebookUrl
            && e.DiamondPerQuestion == command.DiamondPerQuestion), It.IsAny<CancellationToken>()), Times.Once);
    }
}

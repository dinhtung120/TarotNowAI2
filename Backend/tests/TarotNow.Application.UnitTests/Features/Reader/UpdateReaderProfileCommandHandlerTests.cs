using Moq;
using TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Reader;

/// <summary>
/// Unit tests cho UpdateReaderProfileCommandHandler.
/// </summary>
public class UpdateReaderProfileCommandHandlerTests
{
    private readonly Mock<IInlineDomainEventDispatcher> _mockDispatcher;
    private readonly UpdateReaderProfileCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture test.
    /// </summary>
    public UpdateReaderProfileCommandHandlerTests()
    {
        _mockDispatcher = new Mock<IInlineDomainEventDispatcher>();
        _handler = new UpdateReaderProfileCommandHandler(_mockDispatcher.Object);
    }

    /// <summary>
    /// Xác nhận handler publish đúng event và trả kết quả hydrated.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldPublishEventAndReturnHydratedResult()
    {
        var command = new UpdateReaderProfileCommand
        {
            UserId = Guid.NewGuid(),
            BioVi = "Bio",
            Specialties = ["general", "health"],
            YearsOfExperience = 5,
            InstagramUrl = "https://instagram.com/tarot_reader",
            DiamondPerQuestion = 80
        };

        _mockDispatcher
            .Setup(x => x.PublishAsync(It.IsAny<ReaderProfileUpdateRequestedDomainEvent>(), It.IsAny<CancellationToken>()))
            .Callback((IDomainEvent domainEvent, CancellationToken _) =>
            {
                var updateEvent = Assert.IsType<ReaderProfileUpdateRequestedDomainEvent>(domainEvent);
                updateEvent.Updated = true;
            })
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        _mockDispatcher.Verify(x => x.PublishAsync(It.Is<ReaderProfileUpdateRequestedDomainEvent>(e =>
            e.UserId == command.UserId
            && e.BioVi == command.BioVi
            && e.Specialties!.SequenceEqual(command.Specialties!)
            && e.YearsOfExperience == command.YearsOfExperience
            && e.InstagramUrl == command.InstagramUrl
            && e.DiamondPerQuestion == command.DiamondPerQuestion), It.IsAny<CancellationToken>()), Times.Once);
    }
}

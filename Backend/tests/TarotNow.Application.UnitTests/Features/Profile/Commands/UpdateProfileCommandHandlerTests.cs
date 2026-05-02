using Moq;
using TarotNow.Application.Features.Profile.Commands.UpdateProfile;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Profile.Commands;

public class UpdateProfileCommandHandlerTests
{
    private readonly Mock<IInlineDomainEventDispatcher> _dispatcherMock = new();
    private readonly UpdateProfileCommandHandler _handler;

    public UpdateProfileCommandHandlerTests()
    {
        _handler = new UpdateProfileCommandHandler(_dispatcherMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldNormalizeUnspecifiedDateOfBirthToUtc()
    {
        var command = new UpdateProfileCommand
        {
            UserId = Guid.NewGuid(),
            DisplayName = "Reader",
            DateOfBirth = new DateTime(1994, 11, 3), // Kind=Unspecified (from yyyy-MM-dd payload)
        };

        UserProfileUpdateRequestedDomainEvent? capturedEvent = null;
        _dispatcherMock
            .Setup(x => x.PublishAsync(It.IsAny<UserProfileUpdateRequestedDomainEvent>(), It.IsAny<CancellationToken>()))
            .Callback((IDomainEvent domainEvent, CancellationToken _) =>
            {
                capturedEvent = Assert.IsType<UserProfileUpdateRequestedDomainEvent>(domainEvent);
                capturedEvent.Updated = true;
            })
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.NotNull(capturedEvent);
        Assert.Equal(DateTimeKind.Utc, capturedEvent!.DateOfBirth.Kind);
        Assert.Equal(new DateTime(1994, 11, 3, 0, 0, 0, DateTimeKind.Utc), capturedEvent.DateOfBirth);
    }

    [Fact]
    public async Task Handle_ShouldKeepCalendarDate_WhenDateOfBirthHasLocalKind()
    {
        var command = new UpdateProfileCommand
        {
            UserId = Guid.NewGuid(),
            DisplayName = "Reader",
            DateOfBirth = new DateTime(1994, 11, 3, 17, 45, 0, DateTimeKind.Local),
        };

        UserProfileUpdateRequestedDomainEvent? capturedEvent = null;
        _dispatcherMock
            .Setup(x => x.PublishAsync(It.IsAny<UserProfileUpdateRequestedDomainEvent>(), It.IsAny<CancellationToken>()))
            .Callback((IDomainEvent domainEvent, CancellationToken _) =>
            {
                capturedEvent = Assert.IsType<UserProfileUpdateRequestedDomainEvent>(domainEvent);
                capturedEvent.Updated = true;
            })
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(capturedEvent);
        Assert.Equal(DateTimeKind.Utc, capturedEvent!.DateOfBirth.Kind);
        Assert.Equal(new DateTime(1994, 11, 3, 0, 0, 0, DateTimeKind.Utc), capturedEvent.DateOfBirth);
    }
}

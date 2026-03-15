using FluentAssertions;
using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reading.Commands.InitSession;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.UnitTests.Reading;

public class InitReadingSessionCommandHandlerTests
{
    private readonly Mock<IReadingSessionRepository> _repoMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IRngService> _rngMock;
    private readonly InitReadingSessionCommandHandler _handler;

    public InitReadingSessionCommandHandlerTests()
    {
        _repoMock = new Mock<IReadingSessionRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _rngMock = new Mock<IRngService>();
        _handler = new InitReadingSessionCommandHandler(_repoMock.Object, _userRepoMock.Object, _rngMock.Object);
    }

    [Fact]
    public async Task Handle_Daily1Card_ShouldThrowBadRequest_IfAlreadyDrawnToday()
    {
        // Arrange
        var request = new InitReadingSessionCommand 
        { 
            UserId = Guid.NewGuid(), 
            SpreadType = SpreadType.Daily1Card
        };
        
        var user = new User("hash", "user@test.com", "pass", "disp", DateTime.UtcNow.AddYears(-20), true);
        typeof(User).GetProperty("Id")?.SetValue(user, request.UserId);

        _userRepoMock.Setup(x => x.GetByIdAsync(request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Giả lập người dùng đã bốc 1 lá hôm nay
        _repoMock.Setup(x => x.HasDrawnDailyCardAsync(request.UserId, It.IsAny<DateTime>(), CancellationToken.None))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("You have already drawn your free daily card today. Please try other spreads.");
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldReturnSessionResultAndDeductGold()
    {
        // Arrange
        var request = new InitReadingSessionCommand 
        { 
            UserId = Guid.NewGuid(), 
            SpreadType = SpreadType.Spread3Cards
        };

        var user = new User("hash", "user@test.com", "pass", "disp", DateTime.UtcNow.AddYears(-20), true);
        typeof(User).GetProperty("Id")?.SetValue(user, request.UserId);

        _userRepoMock.Setup(x => x.GetByIdAsync(request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Cho phép rút vì không phải bản Daily1Card


        _repoMock.Setup(x => x.StartPaidSessionAtomicAsync(
            request.UserId, request.SpreadType, It.IsAny<ReadingSession>(), 50, 0, CancellationToken.None))
            .ReturnsAsync((true, string.Empty));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CostGold.Should().Be(50);
        result.CostDiamond.Should().Be(0);
        result.SessionId.Should().NotBeEmpty();
    }
}



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
    private readonly Mock<IReadingSessionOrchestrator> _orchestratorMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IRngService> _rngMock;
    private readonly Mock<ISystemConfigSettings> _systemConfigSettingsMock;
    private readonly Mock<IEntitlementService> _entitlementServiceMock;
    private readonly InitReadingSessionCommandHandler _handler;

    public InitReadingSessionCommandHandlerTests()
    {
        _repoMock = new Mock<IReadingSessionRepository>();
        _orchestratorMock = new Mock<IReadingSessionOrchestrator>();
        _userRepoMock = new Mock<IUserRepository>();
        _rngMock = new Mock<IRngService>();
        _systemConfigSettingsMock = new Mock<ISystemConfigSettings>();
        _entitlementServiceMock = new Mock<IEntitlementService>();
        
        _systemConfigSettingsMock.SetupGet(x => x.Spread3GoldCost).Returns(50);
        _systemConfigSettingsMock.SetupGet(x => x.Spread5GoldCost).Returns(100);
        _systemConfigSettingsMock.SetupGet(x => x.Spread10DiamondCost).Returns(50);

        _entitlementServiceMock.Setup(x => x.ConsumeAsync(
            It.IsAny<EntitlementConsumeRequest>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EntitlementConsumeResult(false, "No entitlement"));

        _handler = new InitReadingSessionCommandHandler(
            _repoMock.Object, _orchestratorMock.Object, _userRepoMock.Object,
            _rngMock.Object, _systemConfigSettingsMock.Object, _entitlementServiceMock.Object);
    }

        [Fact]
    public async Task Handle_Daily1Card_ShouldThrowBadRequest_IfAlreadyDrawnToday()
    {
        var request = new InitReadingSessionCommand { UserId = Guid.NewGuid(), SpreadType = SpreadType.Daily1Card };
        var user = new User("hash", "user@test.com", "pass", "disp", DateTime.UtcNow.AddYears(-20), true);
        typeof(User).GetProperty("Id")?.SetValue(user, request.UserId);

        _userRepoMock.Setup(x => x.GetByIdAsync(request.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _repoMock.Setup(x => x.HasDrawnDailyCardAsync(request.UserId, It.IsAny<DateTime>(), CancellationToken.None)).ReturnsAsync(true);

        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("You have already drawn your free daily card today. Please try other spreads.");
    }

        [Fact]
    public async Task Handle_ValidRequest_ShouldReturnSessionResultAndDeductGold()
    {
        var request = new InitReadingSessionCommand { UserId = Guid.NewGuid(), SpreadType = SpreadType.Spread3Cards };
        var user = new User("hash", "user@test.com", "pass", "disp", DateTime.UtcNow.AddYears(-20), true);
        typeof(User).GetProperty("Id")?.SetValue(user, request.UserId);

        _userRepoMock.Setup(x => x.GetByIdAsync(request.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _orchestratorMock.Setup(x => x.StartPaidSessionAsync(
            It.Is<StartPaidSessionRequest>(payload =>
                payload.UserId == request.UserId
                && payload.SpreadType == request.SpreadType
                && payload.CostGold == 50
                && payload.CostDiamond == 0),
            CancellationToken.None))
            .ReturnsAsync((true, string.Empty));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.CostGold.Should().Be(50);
        result.CostDiamond.Should().Be(0);
        result.SessionId.Should().NotBeEmpty();
    }
}

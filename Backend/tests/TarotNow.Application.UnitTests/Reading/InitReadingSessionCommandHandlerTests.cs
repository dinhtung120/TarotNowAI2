/*
 * FILE: InitReadingSessionCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler khởi tạo phiên đọc bài Tarot (Init Reading Session).
 *
 *   CÁC TEST CASE:
 *   1. Handle_Daily1Card_ShouldThrowBadRequest_IfAlreadyDrawnToday:
 *      → Daily1Card miễn phí → chỉ 1 lần/ngày → nếu đã bốc → 400
 *   2. Handle_ValidRequest_ShouldReturnSessionResultAndDeductGold:
 *      → Spread3Cards (50 Gold) → tạo session + deduct currency
 *
 *   PRICING (từ config):
 *   → Daily1Card: miễn phí (0 Gold, 0 Diamond)
 *   → Spread3Cards: 50 Gold
 *   → Spread5Cards: 100 Gold
 *   → Spread10Cards: 50 Diamond
 */

using FluentAssertions;
using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reading.Commands.InitSession;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.UnitTests.Reading;

/// <summary>
/// Test init reading session: daily card limit, pricing deduction.
/// </summary>
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
        // Cấu hình giá từ config (giống production)
        _systemConfigSettingsMock.SetupGet(x => x.Spread3GoldCost).Returns(50);
        _systemConfigSettingsMock.SetupGet(x => x.Spread5GoldCost).Returns(100);
        _systemConfigSettingsMock.SetupGet(x => x.Spread10DiamondCost).Returns(50);

        _entitlementServiceMock.Setup(x => x.ConsumeAsync(
            It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EntitlementConsumeResult(false, "No entitlement"));

        _handler = new InitReadingSessionCommandHandler(
            _repoMock.Object, _orchestratorMock.Object, _userRepoMock.Object,
            _rngMock.Object, _systemConfigSettingsMock.Object, _entitlementServiceMock.Object);
    }

    /// <summary>Daily1Card đã bốc hôm nay → BadRequest (max 1 lần/ngày).</summary>
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

    /// <summary>Spread3Cards → deduct 50 Gold, trả session result.</summary>
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

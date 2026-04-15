

using FluentAssertions;
using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reading.Commands.InitSession;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.UnitTests.Reading;

// Unit test cho handler khởi tạo phiên xem bài.
public class InitReadingSessionCommandHandlerTests
{
    // Mock reading session repo để kiểm soát kiểm tra daily card.
    private readonly Mock<IReadingSessionRepository> _repoMock;
    // Mock orchestrator để kiểm tra luồng trừ phí/tạo session trả phí.
    private readonly Mock<IReadingSessionOrchestrator> _orchestratorMock;
    // Mock user repo để lấy dữ liệu user.
    private readonly Mock<IUserRepository> _userRepoMock;
    // Mock RNG service cho các nhánh cần random.
    private readonly Mock<IRngService> _rngMock;
    // Mock config settings để cố định giá spread.
    private readonly Mock<ISystemConfigSettings> _systemConfigSettingsMock;
    // Handler cần kiểm thử.
    private readonly InitReadingSessionCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho InitReadingSessionCommandHandler.
    /// Luồng setup giá spread và entitlement mặc định giúp test ổn định theo policy hiện tại.
    /// </summary>
    public InitReadingSessionCommandHandlerTests()
    {
        _repoMock = new Mock<IReadingSessionRepository>();
        _orchestratorMock = new Mock<IReadingSessionOrchestrator>();
        _userRepoMock = new Mock<IUserRepository>();
        _rngMock = new Mock<IRngService>();
        _systemConfigSettingsMock = new Mock<ISystemConfigSettings>();
        _systemConfigSettingsMock.SetupGet(x => x.Spread3GoldCost).Returns(50);
        _systemConfigSettingsMock.SetupGet(x => x.Spread5GoldCost).Returns(100);
        _systemConfigSettingsMock.SetupGet(x => x.Spread10DiamondCost).Returns(50);

        _handler = new InitReadingSessionCommandHandler(
            _repoMock.Object, _orchestratorMock.Object, _userRepoMock.Object,
            _rngMock.Object, _systemConfigSettingsMock.Object);
    }

    /// <summary>
    /// Xác nhận daily card đã rút trong ngày sẽ bị từ chối.
    /// Luồng này bảo vệ rule chỉ một lượt Daily1Card miễn phí mỗi ngày.
    /// </summary>
    [Fact]
    public async Task Handle_Daily1Card_ShouldThrowBadRequest_IfAlreadyDrawnToday()
    {
        var request = new InitReadingSessionCommand { UserId = Guid.NewGuid(), SpreadType = SpreadType.Daily1Card };
        var user = new User("hash", "user@test.com", "pass", "disp", DateTime.UtcNow.AddYears(-20), true);
        typeof(User).GetProperty("Id")?.SetValue(user, request.UserId);

        _userRepoMock.Setup(x => x.GetByIdAsync(request.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _repoMock.Setup(x => x.HasDrawnDailyCardAsync(request.UserId, It.IsAny<DateTime>(), CancellationToken.None)).ReturnsAsync(true);

        // Kỳ vọng ném BadRequest với thông điệp hướng dẫn chọn spread khác.
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("You have already drawn your free daily card today. Please try other spreads.");
    }

    /// <summary>
    /// Xác nhận request hợp lệ tạo session và áp đúng chi phí spread.
    /// Luồng này kiểm tra handler truyền payload đúng cho orchestrator.
    /// </summary>
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

        // Thực thi handler và kiểm tra cost/sessionId trong kết quả trả về.
        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.CostGold.Should().Be(50);
        result.CostDiamond.Should().Be(0);
        result.SessionId.Should().NotBeEmpty();
    }
}

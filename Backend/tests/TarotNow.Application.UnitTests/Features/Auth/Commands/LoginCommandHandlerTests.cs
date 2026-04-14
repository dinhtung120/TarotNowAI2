

using Moq;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

// Unit test cho handler đăng nhập và phát hành token.
public class LoginCommandHandlerTests
{
    // Mock user repository để kiểm soát luồng tìm user.
    private readonly Mock<IUserRepository> _userRepositoryMock;
    // Mock password hasher để mô phỏng đúng/sai mật khẩu.
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    // Mock token service để xác nhận token được tạo đúng.
    private readonly Mock<ITokenService> _tokenServiceMock;
    // Mock settings JWT để cố định expiry trong test.
    private readonly Mock<IJwtTokenSettings> _jwtTokenSettingsMock;
    // Mock refresh token repo để kiểm tra persistence refresh token.
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IAuthSessionRepository> _authSessionRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IDomainEventPublisher> _domainEventPublisherMock;
    // Handler cần kiểm thử.
    private readonly LoginCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho LoginCommandHandler.
    /// Luồng thiết lập expiry mặc định giúp assert kết quả ổn định giữa các lần chạy.
    /// </summary>
    public LoginCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _tokenServiceMock = new Mock<ITokenService>();
        _jwtTokenSettingsMock = new Mock<IJwtTokenSettings>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _authSessionRepositoryMock = new Mock<IAuthSessionRepository>();
        _cacheServiceMock = new Mock<ICacheService>();
        _domainEventPublisherMock = new Mock<IDomainEventPublisher>();

        _jwtTokenSettingsMock.SetupGet(x => x.RefreshTokenExpiryDays).Returns(7);
        _jwtTokenSettingsMock.SetupGet(x => x.AccessTokenExpiryMinutes).Returns(15);
        _domainEventPublisherMock
            .Setup(x => x.PublishAsync(It.IsAny<Domain.Events.IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _cacheServiceMock
            .Setup(x => x.GetAsync<long>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0L);
        _cacheServiceMock
            .Setup(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _handler = new LoginCommandHandler(
            _userRepositoryMock.Object, _passwordHasherMock.Object,
            _tokenServiceMock.Object, _jwtTokenSettingsMock.Object,
            _refreshTokenRepositoryMock.Object,
            _authSessionRepositoryMock.Object,
            _cacheServiceMock.Object,
            _domainEventPublisherMock.Object
        );
    }

    /// <summary>
    /// Xác nhận user không tồn tại trả lỗi INVALID_CREDENTIALS.
    /// Luồng này tránh lộ thông tin tồn tại tài khoản qua thông điệp đăng nhập.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserNotFound()
    {
        var command = new LoginCommand { EmailOrUsername = "notfound@example.com", Password = "Password123" };
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.EmailOrUsername, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((User?)null);

        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal(AuthErrorCodes.Unauthorized, ex.ErrorCode);
    }

    /// <summary>
    /// Xác nhận mật khẩu sai trả lỗi INVALID_CREDENTIALS.
    /// Luồng này kiểm tra nhánh verify hash thất bại.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldThrowException_WhenPasswordIsWrong()
    {
        var command = new LoginCommand { EmailOrUsername = "testuser", Password = "WrongPassword" };
        var user = new User("test@example.com", "testuser", "correctHash", "Test User", new DateTime(2000, 1, 1), true);

        _userRepositoryMock.Setup(r => r.GetByUsernameAsync(command.EmailOrUsername, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.VerifyPassword("correctHash", "WrongPassword")).Returns(false);

        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal(AuthErrorCodes.Unauthorized, ex.ErrorCode);
    }

    /// <summary>
    /// Xác nhận user chưa active bị chặn với mã lỗi USER_PENDING.
    /// Luồng này bảo vệ business rule không cho đăng nhập tài khoản chưa kích hoạt.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserStatusIsPending()
    {
        var command = new LoginCommand { EmailOrUsername = "test@example.com", Password = "Password123" };
        var user = new User("test@example.com", "testuser", "correctHash", "Test User", new DateTime(2000, 1, 1), true);
        // Không gọi Activate() để mô phỏng trạng thái pending.

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.EmailOrUsername, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.VerifyPassword("correctHash", "Password123")).Returns(true);

        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal(AuthErrorCodes.Unauthorized, ex.ErrorCode);
    }

    /// <summary>
    /// Xác nhận đăng nhập hợp lệ trả access token + refresh token và lưu refresh token.
    /// Luồng này kiểm tra cả dữ liệu response và side-effect lưu token.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnAuthResponseAndRefreshToken_WhenValid()
    {
        var command = new LoginCommand
        {
            EmailOrUsername = "activeuser",
            Password = "Password123",
            ClientIpAddress = "127.0.0.1",
            DeviceId = "device-1",
            UserAgentHash = "ua-hash-1"
        };
        var user = new User("test@example.com", "activeuser", "correctHash", "Test User", new DateTime(2000, 1, 1), true);
        user.Activate();
        var session = new AuthSession(user.Id, command.DeviceId, command.UserAgentHash, "ip-hash");
        var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(15);
        var accessTokenJti = "jti-1";

        _userRepositoryMock.Setup(r => r.GetByUsernameAsync(command.EmailOrUsername, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.VerifyPassword("correctHash", "Password123")).Returns(true);
        _authSessionRepositoryMock
            .Setup(x => x.CreateAsync(user.Id, command.DeviceId, command.UserAgentHash, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);
        _tokenServiceMock.Setup(t => t.GenerateAccessToken(user, session.Id, out accessTokenExpiresAt, out accessTokenJti)).Returns("mocked_jwt");
        _tokenServiceMock.Setup(t => t.GenerateRefreshToken()).Returns("mocked_refresh_token");

        // Thực thi đăng nhập thành công và lấy tuple response + refresh token.
        var result = await _handler.Handle(command, CancellationToken.None);
        var response = result.Response;
        var refreshToken = result.RefreshToken;

        Assert.NotNull(response);
        Assert.Equal("mocked_jwt", response.AccessToken);
        Assert.Equal("mocked_refresh_token", refreshToken);
        Assert.Equal(user.Id, response.User.Id);

        // Kiểm tra refresh token được lưu với token value, userId và client IP đúng.
        _refreshTokenRepositoryMock.Verify(r => r.AddAsync(It.Is<RefreshToken>(rt =>
            rt.MatchesToken("mocked_refresh_token") &&
            rt.UserId == user.Id &&
            rt.CreatedByIp == "127.0.0.1" &&
            rt.SessionId == session.Id
        ), It.IsAny<CancellationToken>()), Times.Once);
    }
}

/*
 * FILE: LoginCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler đăng nhập (Login).
 *
 *   CÁC TEST CASE:
 *   1. Handle_ShouldThrowException_WhenUserNotFound:
 *      → Email/username không tồn tại → BusinessRuleException INVALID_CREDENTIALS
 *   2. Handle_ShouldThrowException_WhenPasswordIsWrong:
 *      → Password sai → BusinessRuleException INVALID_CREDENTIALS (CÙNG lỗi với user not found → chống dò)
 *   3. Handle_ShouldThrowException_WhenUserStatusIsPending:
 *      → User chưa verify email (Pending) → BusinessRuleException USER_PENDING
 *   4. Handle_ShouldReturnAuthResponseAndRefreshToken_WhenValid:
 *      → Happy path: login OK → trả JWT + refresh token + lưu refresh vào DB
 *
 *   BẢO MẬT:
 *   → User not found + wrong password → CÙNG error code INVALID_CREDENTIALS (chống email enumeration)
 *   → Refresh token lưu kèm IP address (tracking)
 */

using Moq;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

/// <summary>
/// Test login: credentials validation, status check, JWT + refresh token generation.
/// </summary>
public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IJwtTokenSettings> _jwtTokenSettingsMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _tokenServiceMock = new Mock<ITokenService>();
        _jwtTokenSettingsMock = new Mock<IJwtTokenSettings>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();

        _jwtTokenSettingsMock.SetupGet(x => x.RefreshTokenExpiryDays).Returns(7);
        _jwtTokenSettingsMock.SetupGet(x => x.AccessTokenExpiryMinutes).Returns(15);

        _handler = new LoginCommandHandler(
            _userRepositoryMock.Object, _passwordHasherMock.Object,
            _tokenServiceMock.Object, _jwtTokenSettingsMock.Object,
            _refreshTokenRepositoryMock.Object
        );
    }

    /// <summary>User không tồn tại → INVALID_CREDENTIALS (chống dò email).</summary>
    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserNotFound()
    {
        var command = new LoginCommand { EmailOrUsername = "notfound@example.com", Password = "Password123" };
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.EmailOrUsername, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((User?)null);

        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("INVALID_CREDENTIALS", ex.ErrorCode);
    }

    /// <summary>Password sai → CÙNG error code INVALID_CREDENTIALS (chống dò).</summary>
    [Fact]
    public async Task Handle_ShouldThrowException_WhenPasswordIsWrong()
    {
        var command = new LoginCommand { EmailOrUsername = "testuser", Password = "WrongPassword" };
        var user = new User("test@example.com", "testuser", "correctHash", "Test User", new DateTime(2000, 1, 1), true);

        _userRepositoryMock.Setup(r => r.GetByUsernameAsync(command.EmailOrUsername, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.VerifyPassword("correctHash", "WrongPassword")).Returns(false);

        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("INVALID_CREDENTIALS", ex.ErrorCode);
    }

    /// <summary>User status = Pending (chưa verify email) → USER_PENDING.</summary>
    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserStatusIsPending()
    {
        var command = new LoginCommand { EmailOrUsername = "test@example.com", Password = "Password123" };
        var user = new User("test@example.com", "testuser", "correctHash", "Test User", new DateTime(2000, 1, 1), true);
        // User mới tạo → status = Pending (chưa verify email)

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.EmailOrUsername, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.VerifyPassword("correctHash", "Password123")).Returns(true);

        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("USER_PENDING", ex.ErrorCode);
    }

    /// <summary>
    /// HAPPY PATH: login OK → trả JWT access token + refresh token.
    /// Verify: refresh token lưu DB kèm IP address + user ID.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnAuthResponseAndRefreshToken_WhenValid()
    {
        var command = new LoginCommand 
        { 
            EmailOrUsername = "activeuser", 
            Password = "Password123",
            ClientIpAddress = "127.0.0.1" 
        };
        var user = new User("test@example.com", "activeuser", "correctHash", "Test User", new DateTime(2000, 1, 1), true);
        user.Activate(); // Status → Active

        _userRepositoryMock.Setup(r => r.GetByUsernameAsync(command.EmailOrUsername, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.VerifyPassword("correctHash", "Password123")).Returns(true);
        _tokenServiceMock.Setup(t => t.GenerateAccessToken(user)).Returns("mocked_jwt");
        _tokenServiceMock.Setup(t => t.GenerateRefreshToken()).Returns("mocked_refresh_token");

        var (response, refreshToken) = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(response);
        Assert.Equal("mocked_jwt", response.AccessToken);
        Assert.Equal("mocked_refresh_token", refreshToken);
        Assert.Equal(user.Id, response.User.Id);

        // Verify: refresh token lưu DB kèm IP + userId
        _refreshTokenRepositoryMock.Verify(r => r.AddAsync(It.Is<RefreshToken>(rt => 
            rt.MatchesToken("mocked_refresh_token") && 
            rt.UserId == user.Id &&
            rt.CreatedByIp == "127.0.0.1"
        ), It.IsAny<CancellationToken>()), Times.Once);
    }
}

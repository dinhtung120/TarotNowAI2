

using Moq;
using TarotNow.Application.Features.Auth.Commands.ResetPassword;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

// Unit test cho handler đặt lại mật khẩu bằng OTP email.
public class ResetPasswordCommandHandlerTests
{
    // Mock user repository để điều khiển trạng thái user theo email.
    private readonly Mock<IUserRepository> _userRepositoryMock;
    // Mock OTP repository để kiểm tra validate và đánh dấu OTP đã dùng.
    private readonly Mock<IEmailOtpRepository> _emailOtpRepositoryMock;
    // Mock hasher để xác nhận password mới được băm.
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    // Mock refresh token repo để kiểm tra revoke token sau đổi mật khẩu.
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    // Handler cần kiểm thử.
    private readonly ResetPasswordCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho ResetPasswordCommandHandler.
    /// Luồng tiêm mock dependencies giúp test cô lập hoàn toàn logic đổi mật khẩu.
    /// </summary>
    public ResetPasswordCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _emailOtpRepositoryMock = new Mock<IEmailOtpRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();

        _handler = new ResetPasswordCommandHandler(
            _userRepositoryMock.Object, _emailOtpRepositoryMock.Object,
            _passwordHasherMock.Object, _refreshTokenRepositoryMock.Object
        );
    }

    /// <summary>
    /// Xác nhận user không tồn tại trả lỗi INVALID_OTP.
    /// Luồng này che giấu tồn tại tài khoản, đồng nhất thông điệp lỗi bảo mật.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserDoesNotExist()
    {
        var command = new ResetPasswordCommand { Email = "notfound@example.com", OtpCode = "123456", NewPassword = "NewPassword123!" };
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((User?)null);

        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("INVALID_OTP", ex.ErrorCode);
    }

    /// <summary>
    /// Xác nhận OTP không hợp lệ hoặc hết hạn trả lỗi INVALID_OTP.
    /// Luồng này bảo vệ quy trình reset chỉ chấp nhận OTP active đúng loại.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldThrowException_WhenOtpIsInvalid()
    {
        var command = new ResetPasswordCommand { Email = "found@example.com", OtpCode = "wrongOtp", NewPassword = "NewPassword123!" };
        var user = new User("found@example.com", "founduser", "hash", "DisplayName", new DateTime(2000, 1, 1), true);

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _emailOtpRepositoryMock.Setup(r => r.GetLatestActiveOtpAsync(user.Id, OtpType.ResetPassword, It.IsAny<CancellationToken>()))
                               .ReturnsAsync((EmailOtp?)null);

        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("INVALID_OTP", ex.ErrorCode);
    }

    /// <summary>
    /// Xác nhận OTP hợp lệ sẽ đổi mật khẩu và thu hồi toàn bộ refresh token.
    /// Luồng này kiểm tra đủ các side-effect: mark OTP used, update user hash, revoke refresh tokens.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldUpdatePasswordAndRevokeTokens_WhenOtpIsValid()
    {
        var command = new ResetPasswordCommand { Email = "found@example.com", OtpCode = "123456", NewPassword = "NewPassword123!" };
        var user = new User("found@example.com", "founduser", "oldHash", "DisplayName", new DateTime(2000, 1, 1), true);
        var validOtp = new EmailOtp(user.Id, "123456", OtpType.ResetPassword, 15);

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);                           
        _emailOtpRepositoryMock.Setup(r => r.GetLatestActiveOtpAsync(user.Id, OtpType.ResetPassword, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(validOtp);
        _passwordHasherMock.Setup(h => h.HashPassword(command.NewPassword)).Returns("newHash");

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.True(validOtp.IsUsed);
        Assert.Equal("newHash", user.PasswordHash);

        // Xác nhận toàn bộ trạng thái quan trọng được persist đúng một lần.
        _emailOtpRepositoryMock.Verify(r => r.UpdateAsync(validOtp, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.PasswordHash == "newHash"), It.IsAny<CancellationToken>()), Times.Once);
        _refreshTokenRepositoryMock.Verify(r => r.RevokeAllByUserIdAsync(user.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}

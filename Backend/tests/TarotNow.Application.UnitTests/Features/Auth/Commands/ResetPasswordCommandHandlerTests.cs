using Moq;
using TarotNow.Application.Features.Auth.Commands.ResetPassword;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Exceptions;
using TarotNow.Domain.Interfaces;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

public class ResetPasswordCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IEmailOtpRepository> _emailOtpRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly ResetPasswordCommandHandler _handler;

    public ResetPasswordCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _emailOtpRepositoryMock = new Mock<IEmailOtpRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();

        _handler = new ResetPasswordCommandHandler(
            _userRepositoryMock.Object,
            _emailOtpRepositoryMock.Object,
            _passwordHasherMock.Object,
            _refreshTokenRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new ResetPasswordCommand { Email = "notfound@example.com", OtpCode = "123456", NewPassword = "NewPassword123!" };
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((User?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("INVALID_OTP", ex.ErrorCode);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenOtpIsInvalid()
    {
        // Arrange
        var command = new ResetPasswordCommand { Email = "found@example.com", OtpCode = "wrongOtp", NewPassword = "NewPassword123!" };
        var user = new User("found@example.com", "founduser", "hash", "DisplayName", new DateTime(2000, 1, 1), true);

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);

        // Mock DB không trả về OTP hợp lệ nào
        _emailOtpRepositoryMock.Setup(r => r.GetLatestActiveOtpAsync(user.Id, OtpType.ResetPassword, It.IsAny<CancellationToken>()))
                               .ReturnsAsync((EmailOtp?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("INVALID_OTP", ex.ErrorCode);
    }

    [Fact]
    public async Task Handle_ShouldUpdatePasswordAndRevokeTokens_WhenOtpIsValid()
    {
        // Arrange
        var command = new ResetPasswordCommand { Email = "found@example.com", OtpCode = "123456", NewPassword = "NewPassword123!" };
        var user = new User("found@example.com", "founduser", "oldHash", "DisplayName", new DateTime(2000, 1, 1), true);
        var validOtp = new EmailOtp(user.Id, "123456", OtpType.ResetPassword, 15);

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
                           
        _emailOtpRepositoryMock.Setup(r => r.GetLatestActiveOtpAsync(user.Id, OtpType.ResetPassword, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(validOtp);

        _passwordHasherMock.Setup(h => h.HashPassword(command.NewPassword)).Returns("newHash");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        Assert.True(validOtp.IsUsed);
        Assert.Equal("newHash", user.PasswordHash);
        
        _emailOtpRepositoryMock.Verify(r => r.UpdateAsync(validOtp, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.PasswordHash == "newHash"), It.IsAny<CancellationToken>()), Times.Once);
        _refreshTokenRepositoryMock.Verify(r => r.RevokeAllByUserIdAsync(user.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}

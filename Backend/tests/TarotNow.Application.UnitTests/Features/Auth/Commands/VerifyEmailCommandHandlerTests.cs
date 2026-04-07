

using Moq;
using TarotNow.Application.Features.Auth.Commands.VerifyEmail;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

public class VerifyEmailCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IEmailOtpRepository> _emailOtpRepositoryMock;
    private readonly VerifyEmailCommandHandler _handler;

    public VerifyEmailCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _emailOtpRepositoryMock = new Mock<IEmailOtpRepository>();

        _handler = new VerifyEmailCommandHandler(
            _userRepositoryMock.Object, _emailOtpRepositoryMock.Object
        );
    }

        [Fact]
    public async Task Handle_ShouldThrowException_WhenUserDoesNotExist()
    {
        var command = new VerifyEmailCommand { Email = "notfound@example.com", OtpCode = "123456" };
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((User?)null);

        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("INVALID_OTP", ex.ErrorCode);
    }

        [Fact]
    public async Task Handle_ShouldThrowException_WhenUserAlreadyActive()
    {
        var command = new VerifyEmailCommand { Email = "active@example.com", OtpCode = "123456" };
        var user = new User("active@example.com", "active", "hash", "DisplayName", new DateTime(2000, 1, 1), true);
        user.Activate(); 

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);

        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("EMAIL_ALREADY_VERIFIED", ex.ErrorCode);
    }

        [Fact]
    public async Task Handle_ShouldThrowException_WhenOtpIsInvalidOrExpired()
    {
        var command = new VerifyEmailCommand { Email = "pending@example.com", OtpCode = "999999" };
        var user = new User("pending@example.com", "pending", "hash", "DisplayName", new DateTime(2000, 1, 1), true);

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _emailOtpRepositoryMock.Setup(r => r.GetLatestActiveOtpAsync(user.Id, OtpType.VerifyEmail, It.IsAny<CancellationToken>()))
                               .ReturnsAsync((EmailOtp?)null);

        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("INVALID_OTP", ex.ErrorCode);
    }

        [Fact]
    public async Task Handle_ShouldVerifyEmailAndActivateUser_WhenOtpIsValid()
    {
        var command = new VerifyEmailCommand { Email = "pending@example.com", OtpCode = "123456" };
        var user = new User("pending@example.com", "pending", "hash", "DisplayName", new DateTime(2000, 1, 1), true);
        var validOtp = new EmailOtp(user.Id, "123456", OtpType.VerifyEmail, 15);

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _emailOtpRepositoryMock.Setup(r => r.GetLatestActiveOtpAsync(user.Id, OtpType.VerifyEmail, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(validOtp);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.True(validOtp.IsUsed); 
        Assert.Equal(UserStatus.Active, user.Status); 
        
        _emailOtpRepositoryMock.Verify(r => r.UpdateAsync(validOtp, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
    }
}

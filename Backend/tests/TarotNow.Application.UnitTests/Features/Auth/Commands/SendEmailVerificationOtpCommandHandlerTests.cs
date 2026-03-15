using Moq;
using TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

public class SendEmailVerificationOtpCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IEmailOtpRepository> _emailOtpRepositoryMock;
    private readonly Mock<IEmailSender> _emailSenderMock;
    private readonly SendEmailVerificationOtpCommandHandler _handler;

    public SendEmailVerificationOtpCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _emailOtpRepositoryMock = new Mock<IEmailOtpRepository>();
        _emailSenderMock = new Mock<IEmailSender>();
        
        _handler = new SendEmailVerificationOtpCommandHandler(
            _userRepositoryMock.Object,
            _emailOtpRepositoryMock.Object,
            _emailSenderMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenUserDoesNotExist()
    {
        // Phải trả về True để tránh lộ lọt email (Email enumeration)
        // Arrange
        var command = new SendEmailVerificationOtpCommand { Email = "notfound@example.com" };
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _emailOtpRepositoryMock.Verify(r => r.AddAsync(It.IsAny<EmailOtp>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenUserIsAlreadyActive()
    {
        // Arrange
        var command = new SendEmailVerificationOtpCommand { Email = "active@example.com" };
        var activeUser = new User("active@example.com", "active", "hash", "DisplayName", new DateTime(2000, 1, 1), true);
        activeUser.Activate();

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(activeUser);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _emailOtpRepositoryMock.Verify(r => r.AddAsync(It.IsAny<EmailOtp>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldGenerateOtpAndSendEmail_WhenUserIsPending()
    {
        // Arrange
        var command = new SendEmailVerificationOtpCommand { Email = "pending@example.com" };
        var pendingUser = new User("pending@example.com", "pending", "hash", "DisplayName", new DateTime(2000, 1, 1), true);

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(pendingUser);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        
        _emailOtpRepositoryMock.Verify(r => r.AddAsync(
            It.Is<EmailOtp>(otp => otp.UserId == pendingUser.Id && otp.Type == OtpType.VerifyEmail), 
            It.IsAny<CancellationToken>()), Times.Once);

        _emailSenderMock.Verify(s => s.SendEmailAsync(
            command.Email, 
            It.IsAny<string>(), 
            It.IsAny<string>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
}

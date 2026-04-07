

using Moq;
using TarotNow.Application.Features.Auth.Commands.ForgotPassword;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

public class ForgotPasswordCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IEmailOtpRepository> _emailOtpRepositoryMock;
    private readonly Mock<IEmailSender> _emailSenderMock;
    private readonly ForgotPasswordCommandHandler _handler;

    public ForgotPasswordCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _emailOtpRepositoryMock = new Mock<IEmailOtpRepository>();
        _emailSenderMock = new Mock<IEmailSender>();
        
        _handler = new ForgotPasswordCommandHandler(
            _userRepositoryMock.Object,
            _emailOtpRepositoryMock.Object,
            _emailSenderMock.Object
        );
    }

        [Fact]
    public async Task Handle_ShouldReturnTrue_WhenUserDoesNotExist()
    {
        var command = new ForgotPasswordCommand { Email = "notfound@example.com" };
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((User?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result); 
        _emailOtpRepositoryMock.Verify(r => r.AddAsync(It.IsAny<EmailOtp>(), It.IsAny<CancellationToken>()), Times.Never);
    }

        [Fact]
    public async Task Handle_ShouldGenerateOtpAndSendEmail_WhenUserExists()
    {
        var command = new ForgotPasswordCommand { Email = "found@example.com" };
        var user = new User("found@example.com", "founduser", "hash", "DisplayName", new DateTime(2000, 1, 1), true);

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        
        
        _emailOtpRepositoryMock.Verify(r => r.AddAsync(
            It.Is<EmailOtp>(otp => otp.UserId == user.Id && otp.Type == OtpType.ResetPassword), 
            It.IsAny<CancellationToken>()), Times.Once);

        
        _emailSenderMock.Verify(s => s.SendEmailAsync(
            command.Email, 
            It.IsAny<string>(), 
            It.IsAny<string>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
}

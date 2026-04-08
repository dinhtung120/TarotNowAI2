

using Moq;
using TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

// Unit test cho handler gửi OTP xác thực email.
public class SendEmailVerificationOtpCommandHandlerTests
{
    // Mock user repo để điều khiển trạng thái user theo email.
    private readonly Mock<IUserRepository> _userRepositoryMock;
    // Mock OTP repo để xác nhận có/không tạo OTP verify email.
    private readonly Mock<IEmailOtpRepository> _emailOtpRepositoryMock;
    // Mock email sender để kiểm tra hành vi gửi mail.
    private readonly Mock<IEmailSender> _emailSenderMock;
    // Handler cần kiểm thử.
    private readonly SendEmailVerificationOtpCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho SendEmailVerificationOtpCommandHandler.
    /// Luồng dùng mock để test logic gửi OTP mà không phụ thuộc SMTP thật.
    /// </summary>
    public SendEmailVerificationOtpCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _emailOtpRepositoryMock = new Mock<IEmailOtpRepository>();
        _emailSenderMock = new Mock<IEmailSender>();

        _handler = new SendEmailVerificationOtpCommandHandler(
            _userRepositoryMock.Object, _emailOtpRepositoryMock.Object, _emailSenderMock.Object
        );
    }

    /// <summary>
    /// Xác nhận email không tồn tại vẫn trả true và không tạo OTP.
    /// Luồng này tránh lộ thông tin tài khoản qua endpoint gửi OTP.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenUserDoesNotExist()
    {
        var command = new SendEmailVerificationOtpCommand { Email = "notfound@example.com" };
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((User?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        _emailOtpRepositoryMock.Verify(r => r.AddAsync(It.IsAny<EmailOtp>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    /// <summary>
    /// Xác nhận user đã active không phát sinh OTP mới.
    /// Luồng này tránh gửi OTP thừa cho tài khoản đã xác thực email.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenUserIsAlreadyActive()
    {
        var command = new SendEmailVerificationOtpCommand { Email = "active@example.com" };
        var activeUser = new User("active@example.com", "active", "hash", "DisplayName", new DateTime(2000, 1, 1), true);
        activeUser.Activate();

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(activeUser);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        _emailOtpRepositoryMock.Verify(r => r.AddAsync(It.IsAny<EmailOtp>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    /// <summary>
    /// Xác nhận user pending sẽ được tạo OTP verify email và gửi mail.
    /// Luồng này đảm bảo side-effect OTP + email thực thi đúng một lần.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldGenerateOtpAndSendEmail_WhenUserIsPending()
    {
        var command = new SendEmailVerificationOtpCommand { Email = "pending@example.com" };
        var pendingUser = new User("pending@example.com", "pending", "hash", "DisplayName", new DateTime(2000, 1, 1), true);

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(pendingUser);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        _emailOtpRepositoryMock.Verify(r => r.AddAsync(
            It.Is<EmailOtp>(otp => otp.UserId == pendingUser.Id && otp.Type == OtpType.VerifyEmail),
            It.IsAny<CancellationToken>()), Times.Once);
        // Gửi email xác thực sau khi tạo OTP để người dùng hoàn tất kích hoạt tài khoản.
        _emailSenderMock.Verify(s => s.SendEmailAsync(
            command.Email, It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}

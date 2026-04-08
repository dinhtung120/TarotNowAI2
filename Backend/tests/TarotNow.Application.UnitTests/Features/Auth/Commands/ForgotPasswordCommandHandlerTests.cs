

using Moq;
using TarotNow.Application.Features.Auth.Commands.ForgotPassword;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

// Unit test cho handler quên mật khẩu.
public class ForgotPasswordCommandHandlerTests
{
    // Mock user repo để điều khiển tồn tại/không tồn tại user.
    private readonly Mock<IUserRepository> _userRepositoryMock;
    // Mock OTP repo để xác nhận có/không tạo OTP reset.
    private readonly Mock<IEmailOtpRepository> _emailOtpRepositoryMock;
    // Mock email sender để kiểm tra gửi mail.
    private readonly Mock<IEmailSender> _emailSenderMock;
    // Handler cần kiểm thử.
    private readonly ForgotPasswordCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho ForgotPasswordCommandHandler.
    /// Luồng dùng mock giúp kiểm thử logic bảo mật mà không gọi hạ tầng ngoài.
    /// </summary>
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

    /// <summary>
    /// Xác nhận khi email không tồn tại, handler vẫn trả true để tránh lộ thông tin tài khoản.
    /// Luồng này kiểm tra không tạo OTP ở nhánh user không tồn tại.
    /// </summary>
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

    /// <summary>
    /// Xác nhận khi user tồn tại, handler tạo OTP reset password và gửi email.
    /// Luồng này đảm bảo side-effect OTP + email xảy ra đúng một lần.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldGenerateOtpAndSendEmail_WhenUserExists()
    {
        var command = new ForgotPasswordCommand { Email = "found@example.com" };
        var user = new User("found@example.com", "founduser", "hash", "DisplayName", new DateTime(2000, 1, 1), true);

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);

        // Xác nhận OTP được tạo đúng loại ResetPassword cho đúng user.
        _emailOtpRepositoryMock.Verify(r => r.AddAsync(
            It.Is<EmailOtp>(otp => otp.UserId == user.Id && otp.Type == OtpType.ResetPassword),
            It.IsAny<CancellationToken>()), Times.Once);

        // Xác nhận handler gọi send email sau khi tạo OTP.
        _emailSenderMock.Verify(s => s.SendEmailAsync(
            command.Email,
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}

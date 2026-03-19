/*
 * FILE: ForgotPasswordCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler quên mật khẩu (Forgot Password).
 *
 *   CÁC TEST CASE:
 *   1. Handle_ShouldReturnTrue_WhenUserDoesNotExist:
 *      → Email không tồn tại → vẫn trả true (CHỐNG DÒ TÌM EMAIL)
 *      → KHÔNG tạo OTP, KHÔNG gửi email → nhưng response giống hệt khi thành công
 *   2. Handle_ShouldGenerateOtpAndSendEmail_WhenUserExists:
 *      → Email tồn tại → tạo OTP (type=ResetPassword) + gửi email
 *
 *   BẢO MẬT QUAN TRỌNG:
 *   → Chống email enumeration: kẻ tấn công thử nhiều email → nếu trả false = email không tồn tại
 *   → Giải pháp: LUÔN trả true bất kể email có hay không → kẻ tấn công không biết
 */

using Moq;
using TarotNow.Application.Features.Auth.Commands.ForgotPassword;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

/// <summary>
/// Test forgot password: chống email enumeration + OTP generation.
/// </summary>
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

    /// <summary>
    /// Email KHÔNG tồn tại → vẫn trả true (chống dò tìm email).
    /// KHÔNG tạo OTP, KHÔNG gửi email → nhưng response giống khi thành công.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenUserDoesNotExist()
    {
        var command = new ForgotPasswordCommand { Email = "notfound@example.com" };
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((User?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result); // Luôn true → chống enumeration
        _emailOtpRepositoryMock.Verify(r => r.AddAsync(It.IsAny<EmailOtp>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    /// <summary>
    /// Email tồn tại → tạo OTP (type=ResetPassword) + gửi email cho User.
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
        
        // Verify: OTP tạo đúng type
        _emailOtpRepositoryMock.Verify(r => r.AddAsync(
            It.Is<EmailOtp>(otp => otp.UserId == user.Id && otp.Type == OtpType.ResetPassword), 
            It.IsAny<CancellationToken>()), Times.Once);

        // Verify: email gửi đúng địa chỉ
        _emailSenderMock.Verify(s => s.SendEmailAsync(
            command.Email, 
            It.IsAny<string>(), 
            It.IsAny<string>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
}

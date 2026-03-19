/*
 * FILE: SendEmailVerificationOtpCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler gửi OTP xác thực email khi đăng ký.
 *
 *   CÁC TEST CASE:
 *   1. Handle_ShouldReturnTrue_WhenUserDoesNotExist:
 *      → Email không tồn tại → vẫn trả true (chống email enumeration)
 *   2. Handle_ShouldReturnTrue_WhenUserIsAlreadyActive:
 *      → User đã Active (đã verify) → trả true, KHÔNG gửi OTP (tránh spam)
 *   3. Handle_ShouldGenerateOtpAndSendEmail_WhenUserIsPending:
 *      → User Pending → tạo OTP (type=VerifyEmail) + gửi email
 *
 *   LOGIC:
 *   → Chỉ gửi OTP khi User tồn tại VÀ status=Pending
 *   → Mọi trường hợp khác đều trả true (chống enumeration + không spam)
 */

using Moq;
using TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

/// <summary>
/// Test send verification OTP: chống enumeration, chỉ gửi khi Pending.
/// </summary>
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
            _userRepositoryMock.Object, _emailOtpRepositoryMock.Object, _emailSenderMock.Object
        );
    }

    /// <summary>Email không tồn tại → trả true (chống dò email).</summary>
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

    /// <summary>User đã Active → trả true, KHÔNG gửi OTP (đã verify rồi).</summary>
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

    /// <summary>User Pending → tạo OTP (VerifyEmail) + gửi email.</summary>
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
        _emailSenderMock.Verify(s => s.SendEmailAsync(
            command.Email, It.IsAny<string>(), It.IsAny<string>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
}

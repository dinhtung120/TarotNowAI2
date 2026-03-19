/*
 * FILE: VerifyEmailCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler xác thực email sau khi đăng ký.
 *
 *   CÁC TEST CASE:
 *   1. Handle_ShouldThrowException_WhenUserDoesNotExist: email sai → INVALID_OTP
 *   2. Handle_ShouldThrowException_WhenUserAlreadyActive: đã verify rồi → EMAIL_ALREADY_VERIFIED
 *   3. Handle_ShouldThrowException_WhenOtpIsInvalidOrExpired: OTP sai/hết hạn → INVALID_OTP
 *   4. Handle_ShouldVerifyEmailAndActivateUser_WhenOtpIsValid:
 *      → Happy path: OTP đúng → User status Pending → Active + OTP IsUsed=true
 *
 *   KIỂM TRA STATE MACHINE:
 *   → Pending → Active (chỉ khi OTP hợp lệ)
 *   → Active → Active (không cho verify lại → EMAIL_ALREADY_VERIFIED)
 */

using Moq;
using TarotNow.Application.Features.Auth.Commands.VerifyEmail;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

/// <summary>
/// Test email verification: OTP check, status transition Pending → Active.
/// </summary>
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

    /// <summary>User không tồn tại → INVALID_OTP.</summary>
    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserDoesNotExist()
    {
        var command = new VerifyEmailCommand { Email = "notfound@example.com", OtpCode = "123456" };
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((User?)null);

        var ex = await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("INVALID_OTP", ex.ErrorCode);
    }

    /// <summary>User đã Active → EMAIL_ALREADY_VERIFIED (không cho verify lại).</summary>
    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserAlreadyActive()
    {
        var command = new VerifyEmailCommand { Email = "active@example.com", OtpCode = "123456" };
        var user = new User("active@example.com", "active", "hash", "DisplayName", new DateTime(2000, 1, 1), true);
        user.Activate(); // Status = Active

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);

        var ex = await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("EMAIL_ALREADY_VERIFIED", ex.ErrorCode);
    }

    /// <summary>OTP sai hoặc hết hạn → INVALID_OTP.</summary>
    [Fact]
    public async Task Handle_ShouldThrowException_WhenOtpIsInvalidOrExpired()
    {
        var command = new VerifyEmailCommand { Email = "pending@example.com", OtpCode = "999999" };
        var user = new User("pending@example.com", "pending", "hash", "DisplayName", new DateTime(2000, 1, 1), true);

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _emailOtpRepositoryMock.Setup(r => r.GetLatestActiveOtpAsync(user.Id, OtpType.VerifyEmail, It.IsAny<CancellationToken>()))
                               .ReturnsAsync((EmailOtp?)null);

        var ex = await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("INVALID_OTP", ex.ErrorCode);
    }

    /// <summary>
    /// Happy path: OTP hợp lệ → User Pending → Active + OTP IsUsed=true.
    /// </summary>
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
        Assert.True(validOtp.IsUsed); // OTP đã dùng
        Assert.Equal(UserStatus.Active, user.Status); // Pending → Active
        
        _emailOtpRepositoryMock.Verify(r => r.UpdateAsync(validOtp, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
    }
}

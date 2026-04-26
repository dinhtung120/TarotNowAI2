

using Moq;
using TarotNow.Application.Features.Auth.Commands.VerifyEmail;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

// Unit test cho handler xác thực email bằng OTP.
public class VerifyEmailCommandExecutorTests
{
    // Mock user repo để điều khiển trạng thái user trước/sau verify.
    private readonly Mock<IUserRepository> _userRepositoryMock;
    // Mock OTP repo để mô phỏng OTP hợp lệ/không hợp lệ.
    private readonly Mock<IEmailOtpRepository> _emailOtpRepositoryMock;
    private readonly Mock<IDomainEventPublisher> _domainEventPublisherMock;
    // Handler cần kiểm thử.
    private readonly VerifyEmailCommandExecutor _handler;

    /// <summary>
    /// Khởi tạo fixture cho VerifyEmailCommandExecutor.
    /// Luồng tiêm mock dependencies giúp kiểm thử độc lập khỏi DB và SMTP.
    /// </summary>
    public VerifyEmailCommandExecutorTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _emailOtpRepositoryMock = new Mock<IEmailOtpRepository>();
        _domainEventPublisherMock = new Mock<IDomainEventPublisher>();

        _handler = new VerifyEmailCommandExecutor(
            _userRepositoryMock.Object,
            _emailOtpRepositoryMock.Object,
            _domainEventPublisherMock.Object
        );
    }

    /// <summary>
    /// Xác nhận user không tồn tại trả lỗi INVALID_OTP.
    /// Luồng này đảm bảo endpoint không lộ thông tin tài khoản.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserDoesNotExist()
    {
        var command = new VerifyEmailCommand { Email = "notfound@example.com", OtpCode = "123456" };
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((User?)null);

        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("INVALID_OTP", ex.ErrorCode);
    }

    /// <summary>
    /// Xác nhận user đã active không thể verify lại email.
    /// Luồng này bảo vệ trạng thái đã xác thực khỏi thao tác dư thừa.
    /// </summary>
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

    /// <summary>
    /// Xác nhận OTP sai/hết hạn trả lỗi INVALID_OTP.
    /// Luồng này kiểm tra nhánh repository không tìm thấy OTP active.
    /// </summary>
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

    /// <summary>
    /// Xác nhận OTP hợp lệ sẽ kích hoạt user và đánh dấu OTP đã dùng.
    /// Luồng này kiểm tra cả kết quả trả về lẫn side-effect cập nhật persistence.
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
        Assert.True(validOtp.IsUsed);
        Assert.Equal(UserStatus.Active, user.Status);

        // Xác nhận OTP và user đều được cập nhật đúng một lần.
        _emailOtpRepositoryMock.Verify(r => r.UpdateAsync(validOtp, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        _domainEventPublisherMock.Verify(
            p => p.PublishAsync(
                It.Is<Domain.Events.MoneyChangedDomainEvent>(e =>
                    e.UserId == user.Id
                    && e.Currency == CurrencyType.Gold
                    && e.ChangeType == TransactionType.RegisterBonus
                    && e.DeltaAmount == 5),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}



using Moq;
using TarotNow.Application.Features.Auth.Commands.ForgotPassword;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

// Unit test cho handler quên mật khẩu.
public class ForgotPasswordCommandHandlerTests
{
    // Mock user repo để điều khiển tồn tại/không tồn tại user.
    private readonly Mock<IUserRepository> _userRepositoryMock;
    // Mock OTP repo để xác nhận có/không tạo OTP reset.
    private readonly Mock<IEmailOtpRepository> _emailOtpRepositoryMock;
    // Mock domain event publisher để kiểm tra enqueue outbox event.
    private readonly Mock<IDomainEventPublisher> _domainEventPublisherMock;
    // Mock transaction coordinator để mô phỏng transaction boundary trong unit test.
    private readonly Mock<ITransactionCoordinator> _transactionCoordinatorMock;
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
        _domainEventPublisherMock = new Mock<IDomainEventPublisher>();
        _transactionCoordinatorMock = new Mock<ITransactionCoordinator>();
        _transactionCoordinatorMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns<Func<CancellationToken, Task>, CancellationToken>((action, ct) => action(ct));

        _handler = new ForgotPasswordCommandHandler(
            _userRepositoryMock.Object,
            _emailOtpRepositoryMock.Object,
            _domainEventPublisherMock.Object,
            _transactionCoordinatorMock.Object
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
        _domainEventPublisherMock.Verify(
            r => r.PublishAsync(It.IsAny<EmailOtpIssuedDomainEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Xác nhận khi user tồn tại, handler tạo OTP reset password và publish domain event gửi email.
    /// Luồng này đảm bảo side-effect OTP + outbox event xảy ra đúng một lần.
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

        // Xác nhận handler publish EmailOtpIssuedDomainEvent sau khi tạo OTP.
        _domainEventPublisherMock.Verify(s => s.PublishAsync(
            It.Is<EmailOtpIssuedDomainEvent>(x => x.Email == command.Email && x.UserId == user.Id),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}

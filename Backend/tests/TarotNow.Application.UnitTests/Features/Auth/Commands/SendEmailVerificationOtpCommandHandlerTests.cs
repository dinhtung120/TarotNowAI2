

using Moq;
using TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.UnitTests.Features.Auth.Commands;

// Unit test cho handler gửi OTP xác thực email.
public class SendEmailVerificationOtpCommandHandlerRequestedDomainEventHandlerTests
{
    // Mock user repo để điều khiển trạng thái user theo email.
    private readonly Mock<IUserRepository> _userRepositoryMock;
    // Mock OTP repo để xác nhận có/không tạo OTP verify email.
    private readonly Mock<IEmailOtpRepository> _emailOtpRepositoryMock;
    // Mock domain event publisher để kiểm tra enqueue outbox event.
    private readonly Mock<IDomainEventPublisher> _domainEventPublisherMock;
    // Mock transaction coordinator để mô phỏng ranh giới transaction.
    private readonly Mock<ITransactionCoordinator> _transactionCoordinatorMock;
    // Handler cần kiểm thử.
    private readonly SendEmailVerificationOtpCommandHandlerRequestedDomainEventHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho SendEmailVerificationOtpCommandHandlerRequestedDomainEventHandler.
    /// Luồng dùng mock để test logic gửi OTP mà không phụ thuộc SMTP thật.
    /// </summary>
    public SendEmailVerificationOtpCommandHandlerRequestedDomainEventHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _emailOtpRepositoryMock = new Mock<IEmailOtpRepository>();
        _domainEventPublisherMock = new Mock<IDomainEventPublisher>();
        _transactionCoordinatorMock = new Mock<ITransactionCoordinator>();
        _transactionCoordinatorMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns<Func<CancellationToken, Task>, CancellationToken>((action, ct) => action(ct));

        _handler = new SendEmailVerificationOtpCommandHandlerRequestedDomainEventHandler(
            _userRepositoryMock.Object,
            _emailOtpRepositoryMock.Object,
            _domainEventPublisherMock.Object,
            _transactionCoordinatorMock.Object,
            Mock.Of<TarotNow.Application.Interfaces.DomainEvents.IEventHandlerIdempotencyService>()
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
        _domainEventPublisherMock.Verify(
            r => r.PublishAsync(It.IsAny<EmailOtpIssuedDomainEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
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
        _domainEventPublisherMock.Verify(
            r => r.PublishAsync(It.IsAny<EmailOtpIssuedDomainEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Xác nhận user pending sẽ được tạo OTP verify email và publish domain event gửi mail.
    /// Luồng này đảm bảo side-effect OTP + outbox event thực thi đúng một lần.
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
        // Publish event email xác thực sau khi tạo OTP để worker xử lý gửi mail.
        _domainEventPublisherMock.Verify(s => s.PublishAsync(
            It.Is<EmailOtpIssuedDomainEvent>(x => x.Email == command.Email && x.UserId == pendingUser.Id),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}

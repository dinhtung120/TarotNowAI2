using MediatR;
using System;
using System.Security.Cryptography;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Auth.Commands.ForgotPassword;

// Handler khởi tạo luồng quên mật khẩu bằng OTP qua email.
public class ForgotPasswordCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ForgotPasswordCommandHandlerRequestedDomainEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailOtpRepository _emailOtpRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly ITransactionCoordinator _transactionCoordinator;

    /// <summary>
    /// Khởi tạo handler forgot password.
    /// Luồng xử lý: nhận user repo, OTP repo, transaction coordinator và domain event publisher để enqueue email OTP.
    /// </summary>
    public ForgotPasswordCommandHandlerRequestedDomainEventHandler(
        IUserRepository userRepository,
        IEmailOtpRepository emailOtpRepository,
        IDomainEventPublisher domainEventPublisher,
        ITransactionCoordinator transactionCoordinator,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _userRepository = userRepository;
        _emailOtpRepository = emailOtpRepository;
        _domainEventPublisher = domainEventPublisher;
        _transactionCoordinator = transactionCoordinator;
    }

    /// <summary>
    /// Xử lý yêu cầu quên mật khẩu.
    /// Luồng xử lý: tìm user theo email, nếu tồn tại thì tạo OTP + lưu DB + gửi email, luôn trả true để tránh lộ thông tin account.
    /// </summary>
    public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            // Trả true ngay cả khi email không tồn tại để tránh lộ thông tin account (email enumeration).
            return true;
        }

        // Sinh OTP 6 chữ số ngẫu nhiên cho luồng reset password.
        var otpCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

        var otpEntity = new TarotNow.Domain.Entities.EmailOtp(
            userId: user.Id,
            otpCode: otpCode,
            type: OtpType.ResetPassword,
            expiryMinutes: 15);

        var subject = "TarotNow - Reset Your Password";
        var body = $"Hello {user.Username},\n\nWe received a request to reset your password. Your reset OTP code is: {otpCode}\n\nThis code will expire in 15 minutes. If you did not request this change, please safely ignore this email.";
        await _transactionCoordinator.ExecuteAsync(
            async transactionCt =>
            {
                // Lưu OTP trước khi gửi email để bảo đảm mã trong email luôn có bản ghi hợp lệ.
                await _emailOtpRepository.AddAsync(otpEntity, transactionCt);

                await _domainEventPublisher.PublishAsync(
                    new TarotNow.Domain.Events.EmailOtpIssuedDomainEvent
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        Subject = subject,
                        Body = body,
                        Purpose = OtpType.ResetPassword
                    },
                    transactionCt);
            },
            cancellationToken);

        return true;
    }

    protected override async Task HandleDomainEventAsync(
        ForgotPasswordCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}

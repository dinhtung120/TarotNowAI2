using MediatR;
using System;
using System.Security.Cryptography;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;

// Handler gửi OTP xác minh email cho tài khoản chưa active.
public class SendEmailVerificationOtpCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<SendEmailVerificationOtpCommandHandlerRequestedDomainEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailOtpRepository _emailOtpRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly ITransactionCoordinator _transactionCoordinator;

    /// <summary>
    /// Khởi tạo handler gửi OTP verify email.
    /// Luồng xử lý: nhận user repo, OTP repo, transaction coordinator và domain event publisher để enqueue email OTP.
    /// </summary>
    public SendEmailVerificationOtpCommandHandlerRequestedDomainEventHandler(
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
    /// Xử lý command gửi OTP verify email.
    /// Luồng xử lý: tìm user theo email, bỏ qua user null/đã active, tạo OTP mới, lưu DB và gửi email.
    /// </summary>
    public async Task<bool> Handle(SendEmailVerificationOtpCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            // Rule bảo mật: luôn trả true để không lộ email có tồn tại hay không.
            return true;
        }

        if (user.Status == UserStatus.Active)
        {
            // Account đã active thì không cần gửi lại OTP xác minh email.
            return true;
        }

        // Sinh OTP 6 chữ số cho mục đích xác minh email.
        var otpCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

        var otpEntity = new EmailOtp(
            userId: user.Id,
            otpCode: otpCode,
            type: OtpType.VerifyEmail,
            expiryMinutes: 15);

        var subject = "TarotNow - Validate your target email address";
        var body = $"Hello {user.Username},\n\nYour Verification Code is: {otpCode}\n\nThis code will expire in 15 minutes. Please do not share this code.";
        await _transactionCoordinator.ExecuteAsync(
            async transactionCt =>
            {
                // Persist OTP trước để đảm bảo mã gửi đi luôn có bản ghi hợp lệ.
                await _emailOtpRepository.AddAsync(otpEntity, transactionCt);

                await _domainEventPublisher.PublishAsync(
                    new EmailOtpIssuedDomainEvent
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        Subject = subject,
                        Body = body,
                        Purpose = OtpType.VerifyEmail
                    },
                    transactionCt);
            },
            cancellationToken);

        return true;
    }

    protected override async Task HandleDomainEventAsync(
        SendEmailVerificationOtpCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}

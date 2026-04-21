using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Gửi email xác nhận sau khi tạo yêu cầu rút tiền.
/// </summary>
public sealed class WithdrawalRequestedEmailDomainEventHandler
    : IdempotentDomainEventNotificationHandler<WithdrawalRequestedDomainEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<WithdrawalRequestedEmailDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo handler email cho withdrawal requested.
    /// </summary>
    public WithdrawalRequestedEmailDomainEventHandler(
        IUserRepository userRepository,
        IEmailSender emailSender,
        ILogger<WithdrawalRequestedEmailDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        WithdrawalRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(domainEvent.UserId, cancellationToken);
        if (user == null || string.IsNullOrWhiteSpace(user.Email))
        {
            _logger.LogWarning(
                "[WithdrawalEmail] Skip send because user/email is missing. UserId={UserId}, RequestId={RequestId}",
                domainEvent.UserId,
                domainEvent.RequestId);
            return;
        }

        var subject = "TarotNow - Yeu cau rut tien da duoc ghi nhan";
        var body = BuildEmailBody(user.DisplayName, domainEvent);
        await _emailSender.SendEmailAsync(user.Email, subject, body, cancellationToken);
    }

    private static string BuildEmailBody(string displayName, WithdrawalRequestedDomainEvent domainEvent)
    {
        var maskedAccountNumber = MaskBankAccountNumber(domainEvent.BankAccountNumber);
        return $$"""
            <h3>Chao {{displayName}},</h3>
            <p>He thong da ghi nhan yeu cau rut tien cua ban.</p>
            <ul>
              <li>Request ID: <b>{{domainEvent.RequestId}}</b></li>
              <li>So diamond: <b>{{domainEvent.AmountDiamond}}</b></li>
              <li>So tien du kien nhan: <b>{{domainEvent.NetAmountVnd:N0}} VND</b></li>
              <li>Ngan hang: <b>{{domainEvent.BankName}}</b></li>
              <li>So tai khoan: <b>{{maskedAccountNumber}}</b></li>
            </ul>
            <p>Trang thai hien tai: <b>Pending</b>.</p>
            <p>TarotNow se thong bao ket qua duyet trong muc Notification.</p>
            """;
    }

    private static string MaskBankAccountNumber(string bankAccountNumber)
    {
        var value = bankAccountNumber.Trim();
        if (value.Length <= 4)
        {
            return value;
        }

        return new string('*', value.Length - 4) + value[^4..];
    }
}

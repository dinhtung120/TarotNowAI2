using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed class EscrowReleasedEmailNotificationHandler : INotificationHandler<EscrowReleasedNotification>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<EscrowReleasedEmailNotificationHandler> _logger;

    public EscrowReleasedEmailNotificationHandler(
        IUserRepository userRepository,
        IEmailSender emailSender,
        ILogger<EscrowReleasedEmailNotificationHandler> logger)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task Handle(EscrowReleasedNotification notification, CancellationToken cancellationToken)
    {
        var ev = notification.DomainEvent;
        var receiver = await _userRepository.GetByIdAsync(ev.ReceiverId, cancellationToken);
        var payer = await _userRepository.GetByIdAsync(ev.PayerId, cancellationToken);

        await SendReceiverEmailAsync(receiver, ev, cancellationToken);
        await SendPayerEmailAsync(payer, ev, cancellationToken);
    }

    private async Task SendReceiverEmailAsync(
        Domain.Entities.User? receiver,
        Domain.Events.EscrowReleasedDomainEvent ev,
        CancellationToken cancellationToken)
    {
        if (receiver == null || string.IsNullOrWhiteSpace(receiver.Email))
        {
            return;
        }

        var subject = "TarotNow - Giải ngân phí dịch vụ Chat";
        var body = $"""
            <h3>Chào {receiver.DisplayName},</h3>
            <p>Một khoản thanh toán vừa được giải ngân tự động vào ví của bạn.</p>
            <ul>
                <li><b>Tổng thu:</b> {ev.GrossAmountDiamond} 💎</li>
                <li><b>Thực nhận:</b> {ev.ReleasedAmountDiamond} 💎</li>
                <li><b>Phí nền tảng (10%):</b> {ev.FeeAmountDiamond} 💎</li>
            </ul>
            <p>Cảm ơn bạn đã đồng hành cùng TarotNow!</p>
            """;

        await SendEmailSafelyAsync(receiver.Email, subject, body, "Reader", cancellationToken);
    }

    private async Task SendPayerEmailAsync(
        Domain.Entities.User? payer,
        Domain.Events.EscrowReleasedDomainEvent ev,
        CancellationToken cancellationToken)
    {
        if (payer == null || string.IsNullOrWhiteSpace(payer.Email))
        {
            return;
        }

        var subject = "TarotNow - Thanh toán phiên Chat thành công";
        var body = $"""
            <h3>Chào {payer.DisplayName},</h3>
            <p>TarotNow đã chuyển <b>{ev.GrossAmountDiamond} 💎</b> từ escrow sang ví Reader để tất toán phiên chat.</p>
            <p>Trân trọng cảm ơn.</p>
            """;

        await SendEmailSafelyAsync(payer.Email, subject, body, "User", cancellationToken);
    }

    private async Task SendEmailSafelyAsync(
        string email,
        string subject,
        string body,
        string audience,
        CancellationToken cancellationToken)
    {
        try
        {
            await _emailSender.SendEmailAsync(email, subject, body, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[EscrowReleasedEmail] Lỗi gửi email {Audience} {Email}", audience, email);
        }
    }
}

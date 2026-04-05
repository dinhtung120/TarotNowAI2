using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed class EscrowRefundedEmailNotificationHandler : INotificationHandler<EscrowRefundedNotification>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<EscrowRefundedEmailNotificationHandler> _logger;

    public EscrowRefundedEmailNotificationHandler(
        IUserRepository userRepository,
        IEmailSender emailSender,
        ILogger<EscrowRefundedEmailNotificationHandler> logger)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task Handle(EscrowRefundedNotification notification, CancellationToken cancellationToken)
    {
        var ev = notification.DomainEvent;
        var user = await _userRepository.GetByIdAsync(ev.UserId, cancellationToken);
        if (user == null || string.IsNullOrWhiteSpace(user.Email))
        {
            _logger.LogWarning("[EscrowRefundedEmail] Không tìm thấy user/email cho UserId = {UserId}", ev.UserId);
            return;
        }

        var subject = "TarotNow - Thông báo hoàn tiền dịch vụ Giao tiếp (Chat)";
        var body = $"""
            <h3>Chào {user.DisplayName},</h3>
            <p>Hệ thống vừa hoàn lại <b>{ev.AmountDiamond} Kim cương (💎)</b> vào ví của bạn.</p>
            <p><strong>Lý do:</strong> {ev.RefundSource}</p>
            <p>Số dư của bạn đã được cập nhật thành công. Cảm ơn bạn đã sử dụng TarotNow!</p>
            """;

        await SendEmailSafelyAsync(user.Email, subject, body, cancellationToken);
    }

    private async Task SendEmailSafelyAsync(string email, string subject, string body, CancellationToken cancellationToken)
    {
        try
        {
            await _emailSender.SendEmailAsync(email, subject, body, cancellationToken);
            _logger.LogInformation("[EscrowRefundedEmail] Đã gửi thông báo hoàn tiền tới {Email}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[EscrowRefundedEmail] Lỗi khi gửi email hoàn tiền tới {Email}", email);
        }
    }
}

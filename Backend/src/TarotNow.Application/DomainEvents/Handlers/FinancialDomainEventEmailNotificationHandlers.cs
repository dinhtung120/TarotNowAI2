/*
 * ===================================================================
 * FILE: FinancialDomainEventEmailNotificationHandlers.cs
 * NAMESPACE: TarotNow.Application.DomainEvents.Handlers
 * ===================================================================
 * MỤC ĐÍCH:
 *   Lắng nghe các sự kiện Tài Chính / Escrow diễn ra trong miền Domain,
 *   Sau đó phát thông báo (Email) tới User / Reader tương ứng, tuân thủ đúng 
 *   Thiết Kế Chat.md về việc "Tự động gửi email khi giải ngân hoặc hoàn tiền".
 * ===================================================================
 */

using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

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
            _logger.LogWarning("[EscrowRefundedEmail] Không tìm thấy user hoặc email cho UserId = {UserId}", ev.UserId);
            return;
        }

        var subject = "TarotNow - Thông báo hoàn tiền dịch vụ Giao tiếp (Chat)";
        var body = $@"
            <h3>Chào {user.DisplayName},</h3>
            <p>Hệ thống vừa tiến hành hoàn lại <b>{ev.AmountDiamond} Kim cương (💎)</b> vào ví của bạn.</p>
            <p><strong>Lý do:</strong> {ev.RefundSource}</p>
            <p>Số dư của bạn đã được cập nhật thành công. Cảm ơn bạn đã sử dụng TarotNow!</p>
        ";

        try
        {
            await _emailSender.SendEmailAsync(user.Email, subject, body, cancellationToken);
            _logger.LogInformation("[EscrowRefundedEmail] Đã gửi thông báo hoàn tiền thành công tới {Email}", user.Email);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "[EscrowRefundedEmail] Lỗi khi gửi email hoàn tiền tới {Email}", user.Email);
        }
    }
}

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

        // 1. Gửi thư cho Reader (người nhận tiền)
        if (receiver != null && !string.IsNullOrWhiteSpace(receiver.Email))
        {
            var rSubject = "TarotNow - Giải ngân phí dịch vụ Chat";
            var rBody = $@"
                <h3>Chào {receiver.DisplayName},</h3>
                <p>Một khoản thanh toán vừa được giải ngân tự động vào ví của bạn (Sau khi khấu trừ phí Nền tảng).</p>
                <ul>
                    <li><b>Tổng thu:</b> {ev.GrossAmountDiamond} 💎</li>
                    <li><b>Thực nhận:</b> {ev.ReleasedAmountDiamond} 💎</li>
                    <li><b>Phí nền tảng (10%):</b> {ev.FeeAmountDiamond} 💎</li>
                </ul>
                <p>Cảm ơn bạn đã đồng hành cùng TarotNow!</p>
            ";

            try
            {
                await _emailSender.SendEmailAsync(receiver.Email, rSubject, rBody, cancellationToken);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "[EscrowReleasedEmail] Lỗi gửi email Reader {Email}", receiver.Email);
            }
        }

        // 2. Gửi thư cho User (người thanh toán)
        if (payer != null && !string.IsNullOrWhiteSpace(payer.Email))
        {
            var pSubject = "TarotNow - Thanh toán phiên Chat thành công";
            var pBody = $@"
                <h3>Chào {payer.DisplayName},</h3>
                <p>Hệ thống TarotNow đã chuyển số tiền <b>{ev.GrossAmountDiamond} 💎</b> từ trạng thái Ký quỹ (Đóng băng) sang ví của Reader để tất toán cho phiên Chat hỗ trợ theo yêu cầu của bạn, hoặc do đã quá hạn Dispute Window.</p>
                <p>Trân trọng cảm ơn.</p>
            ";

            try
            {
                await _emailSender.SendEmailAsync(payer.Email, pSubject, pBody, cancellationToken);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "[EscrowReleasedEmail] Lỗi gửi email User {Email}", payer.Email);
            }
        }
    }
}

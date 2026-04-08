using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.DomainEvents.Handlers;

// Gửi email cho cả payer và receiver khi escrow được giải ngân.
public sealed class EscrowReleasedEmailNotificationHandler : INotificationHandler<EscrowReleasedNotification>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<EscrowReleasedEmailNotificationHandler> _logger;

    /// <summary>
    /// Khởi tạo handler email giải ngân escrow.
    /// Luồng xử lý: nhận repository để tải thông tin người nhận/người trả và email sender để gửi thông báo.
    /// </summary>
    public EscrowReleasedEmailNotificationHandler(
        IUserRepository userRepository,
        IEmailSender emailSender,
        ILogger<EscrowReleasedEmailNotificationHandler> logger)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
        _logger = logger;
    }

    /// <summary>
    /// Xử lý notification giải ngân và gửi email cho hai vai trò liên quan.
    /// Luồng xử lý: tải receiver/payer theo id rồi gửi từng email theo mẫu nội dung riêng.
    /// </summary>
    public async Task Handle(EscrowReleasedNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var receiver = await _userRepository.GetByIdAsync(domainEvent.ReceiverId, cancellationToken);
        var payer = await _userRepository.GetByIdAsync(domainEvent.PayerId, cancellationToken);

        // Tách hai luồng gửi để đảm bảo template phù hợp từng đối tượng nghiệp vụ.
        await SendReceiverEmailAsync(receiver, domainEvent, cancellationToken);
        await SendPayerEmailAsync(payer, domainEvent, cancellationToken);
    }

    /// <summary>
    /// Gửi email cho reader nhận tiền giải ngân.
    /// Luồng xử lý: kiểm tra email hợp lệ, dựng nội dung breakdown khoản tiền, rồi gửi an toàn.
    /// </summary>
    private async Task SendReceiverEmailAsync(
        Domain.Entities.User? receiver,
        Domain.Events.EscrowReleasedDomainEvent ev,
        CancellationToken cancellationToken)
    {
        if (receiver == null || string.IsNullOrWhiteSpace(receiver.Email))
        {
            // Edge case thiếu thông tin người nhận: bỏ qua gửi để tránh lỗi null/email rỗng.
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

    /// <summary>
    /// Gửi email cho user trả phí xác nhận phiên chat đã tất toán.
    /// Luồng xử lý: kiểm tra email hợp lệ, dựng nội dung xác nhận chuyển escrow, rồi gửi an toàn.
    /// </summary>
    private async Task SendPayerEmailAsync(
        Domain.Entities.User? payer,
        Domain.Events.EscrowReleasedDomainEvent ev,
        CancellationToken cancellationToken)
    {
        if (payer == null || string.IsNullOrWhiteSpace(payer.Email))
        {
            // Edge case không có email người trả: dừng nhánh gửi tương ứng.
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

    /// <summary>
    /// Thực hiện gửi email với cơ chế bắt lỗi cục bộ.
    /// Luồng xử lý: thử gửi qua IEmailSender, nếu lỗi thì log để vận hành theo dõi.
    /// </summary>
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
            // Không ném lại exception để tránh làm fail domain event chain vì lỗi kênh thông báo.
            _logger.LogError(ex, "[EscrowReleasedEmail] Lỗi gửi email {Audience} {Email}", audience, email);
        }
    }
}

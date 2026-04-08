using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.DomainEvents.Handlers;

// Gửi email thông báo cho user khi escrow được hoàn tiền.
public sealed class EscrowRefundedEmailNotificationHandler : INotificationHandler<EscrowRefundedNotification>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<EscrowRefundedEmailNotificationHandler> _logger;

    /// <summary>
    /// Khởi tạo handler gửi email hoàn tiền escrow.
    /// Luồng xử lý: nhận repository user để lấy email đích và email sender để phát thư thông báo.
    /// </summary>
    public EscrowRefundedEmailNotificationHandler(
        IUserRepository userRepository,
        IEmailSender emailSender,
        ILogger<EscrowRefundedEmailNotificationHandler> logger)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
        _logger = logger;
    }

    /// <summary>
    /// Xử lý notification hoàn tiền và gửi email cho user liên quan.
    /// Luồng xử lý: tải user theo UserId, kiểm tra email hợp lệ, dựng nội dung thư và gửi qua hàm an toàn.
    /// </summary>
    public async Task Handle(EscrowRefundedNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var user = await _userRepository.GetByIdAsync(domainEvent.UserId, cancellationToken);
        if (user == null || string.IsNullOrWhiteSpace(user.Email))
        {
            // Edge case không có user hoặc email rỗng: ghi cảnh báo và bỏ qua gửi thư.
            _logger.LogWarning("[EscrowRefundedEmail] Không tìm thấy user/email cho UserId = {UserId}", domainEvent.UserId);
            return;
        }

        var subject = "TarotNow - Thông báo hoàn tiền dịch vụ Giao tiếp (Chat)";
        var body = $"""
            <h3>Chào {user.DisplayName},</h3>
            <p>Hệ thống vừa hoàn lại <b>{domainEvent.AmountDiamond} Kim cương (💎)</b> vào ví của bạn.</p>
            <p><strong>Lý do:</strong> {domainEvent.RefundSource}</p>
            <p>Số dư của bạn đã được cập nhật thành công. Cảm ơn bạn đã sử dụng TarotNow!</p>
            """;

        await SendEmailSafelyAsync(user.Email, subject, body, cancellationToken);
    }

    /// <summary>
    /// Gửi email trong khối bảo vệ lỗi để không làm fail luồng domain event chính.
    /// Luồng xử lý: thử gửi email, log thông tin khi thành công và log lỗi khi thất bại.
    /// </summary>
    private async Task SendEmailSafelyAsync(string email, string subject, string body, CancellationToken cancellationToken)
    {
        try
        {
            await _emailSender.SendEmailAsync(email, subject, body, cancellationToken);
            _logger.LogInformation("[EscrowRefundedEmail] Đã gửi thông báo hoàn tiền tới {Email}", email);
        }
        catch (Exception ex)
        {
            // Nhánh lỗi gửi thư chỉ được log để tránh rollback nghiệp vụ tài chính đã hoàn tất.
            _logger.LogError(ex, "[EscrowRefundedEmail] Lỗi khi gửi email hoàn tiền tới {Email}", email);
        }
    }
}

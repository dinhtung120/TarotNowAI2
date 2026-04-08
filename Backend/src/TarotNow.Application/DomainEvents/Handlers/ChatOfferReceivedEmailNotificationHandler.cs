using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Gửi email cho reader khi có yêu cầu tư vấn mới cần duyệt.
/// </summary>
public sealed class ChatOfferReceivedEmailNotificationHandler : INotificationHandler<ChatOfferReceivedNotification>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<ChatOfferReceivedEmailNotificationHandler> _logger;

    /// <summary>
    /// Khởi tạo handler gửi email duyệt yêu cầu chat.
    /// Luồng xử lý: nhận repository User để lấy email Reader và email sender để phát thư thông báo.
    /// </summary>
    public ChatOfferReceivedEmailNotificationHandler(
        IUserRepository userRepository,
        IEmailSender emailSender,
        ILogger<ChatOfferReceivedEmailNotificationHandler> logger)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
        _logger = logger;
    }

    /// <summary>
    /// Xử lý sự kiện nhận thông điệp đầu tiên từ User và gửi email cho Reader tương ứng.
    /// Luồng xử lý: tải dữ liệu User và Reader; dựng nội dung nhắc nhở thời hạn trả lời (SLA) và gửi qua kênh email.
    /// </summary>
    public async Task Handle(ChatOfferReceivedNotification notification, CancellationToken cancellationToken)
    {
        var ev = notification.DomainEvent;
        var reader = await _userRepository.GetByIdAsync(ev.ReaderId, cancellationToken);
        var user = await _userRepository.GetByIdAsync(ev.UserId, cancellationToken);

        if (reader == null || string.IsNullOrWhiteSpace(reader.Email) || user == null)
        {
            // Tránh gửi mail lỗi nếu dữ liệu không tồn tại
            _logger.LogWarning("[ChatOfferReceivedEmail] Không lấy được Email hợp lệ cho Reader {ReaderId} hoặc thiếu User {UserId}", ev.ReaderId, ev.UserId);
            return;
        }

        var subject = "TarotNow - Có yêu cầu tư vấn mới chờ bạn phê duyệt";
        var body = $"""
            <h3>Chào {reader.DisplayName},</h3>
            <p>Khách hàng <b>{user.DisplayName}</b> vừa gửi yêu cầu bắt đầu phiên tư vấn / câu hỏi mới.</p>
            <p>Vui lòng đăng nhập vào hệ thống và <b>Phê duyệt</b> hoặc Từ chối yêu cầu này.</p>
            <p>Lưu ý: Thời hạn tối đa để bạn phản hồi là tới <b>{ev.OfferExpiresAtUtc:dd/MM/yyyy HH:mm} (giờ chuẩn hệ thống - UTC)</b>.</p>
            <p>Trân trọng cảm ơn,</p>
            """;

        try
        {
            await _emailSender.SendEmailAsync(reader.Email, subject, body, cancellationToken);
            _logger.LogInformation("[ChatOfferReceivedEmail] Đã gửi thông báo cho Reader {Email}", reader.Email);
        }
        catch (Exception ex)
        {
            // Bắt và log lỗi để khoanh vùng sự cố email mà không làm đổ vỡ Flow Event của MediatR
            _logger.LogError(ex, "[ChatOfferReceivedEmail] Lỗi khi báo cho Reader {Email}", reader.Email);
        }
    }
}

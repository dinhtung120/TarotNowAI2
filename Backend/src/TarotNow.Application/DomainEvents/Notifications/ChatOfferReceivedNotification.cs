using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Notifications;

/// <summary>
/// Notification bọc domain event khởi tạo yêu cầu tư vấn mới dành cho MediatR handler.
/// </summary>
public sealed class ChatOfferReceivedNotification : INotification
{
    /// <summary>
    /// Khởi tạo notification đón sự kiện gửi yêu cầu tư vấn đầu tiên.
    /// Luồng xử lý: lưu domain event để cung cấp ngữ cảnh gửi email/push cho reader.
    /// </summary>
    public ChatOfferReceivedNotification(ChatOfferReceivedDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    // Domain event gốc chứa dữ liệu khởi tạo yêu cầu
    public ChatOfferReceivedDomainEvent DomainEvent { get; }
}

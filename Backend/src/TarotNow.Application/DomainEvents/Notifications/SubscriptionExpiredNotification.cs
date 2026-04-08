using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Notifications;

// Notification bao domain event hết hạn subscription.
public sealed class SubscriptionExpiredNotification : INotification
{
    /// <summary>
    /// Khởi tạo notification cho sự kiện subscription expired.
    /// Luồng xử lý: lưu domain event để handler có dữ liệu dọn cache/quản lý hậu kỳ.
    /// </summary>
    public SubscriptionExpiredNotification(SubscriptionExpiredDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    // Domain event gốc chứa thông tin hết hạn gói.
    public SubscriptionExpiredDomainEvent DomainEvent { get; }
}

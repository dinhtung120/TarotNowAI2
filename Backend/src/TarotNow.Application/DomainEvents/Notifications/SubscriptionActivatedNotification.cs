using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Notifications;

// Notification bao domain event kích hoạt subscription.
public sealed class SubscriptionActivatedNotification : INotification
{
    /// <summary>
    /// Khởi tạo notification cho sự kiện subscription activated.
    /// Luồng xử lý: giữ domain event gốc để handler cache hoặc thông báo sử dụng.
    /// </summary>
    public SubscriptionActivatedNotification(SubscriptionActivatedDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    // Domain event gốc chứa thông tin kích hoạt gói.
    public SubscriptionActivatedDomainEvent DomainEvent { get; }
}

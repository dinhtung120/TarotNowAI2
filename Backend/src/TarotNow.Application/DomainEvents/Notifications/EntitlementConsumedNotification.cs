using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Notifications;

// Notification bao domain event entitlement đã bị tiêu thụ.
public sealed class EntitlementConsumedNotification : INotification
{
    /// <summary>
    /// Khởi tạo notification cho sự kiện entitlement consumed.
    /// Luồng xử lý: lưu domain event gốc để các handler downstream truy cập dữ liệu.
    /// </summary>
    public EntitlementConsumedNotification(EntitlementConsumedDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    // Domain event gốc chứa thông tin người dùng và lượng entitlement đã tiêu thụ.
    public EntitlementConsumedDomainEvent DomainEvent { get; }
}

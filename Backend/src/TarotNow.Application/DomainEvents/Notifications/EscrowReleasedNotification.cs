using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Notifications;

// Notification bao domain event giải ngân escrow.
public sealed class EscrowReleasedNotification : INotification
{
    /// <summary>
    /// Khởi tạo notification cho sự kiện escrow released.
    /// Luồng xử lý: lưu domain event để các handler xử lý email/push/log dùng chung.
    /// </summary>
    public EscrowReleasedNotification(EscrowReleasedDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    // Domain event gốc chứa thông tin giải ngân escrow.
    public EscrowReleasedDomainEvent DomainEvent { get; }
}

using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Notifications;

// Notification bao domain event hoàn tiền escrow.
public sealed class EscrowRefundedNotification : INotification
{
    /// <summary>
    /// Khởi tạo notification cho sự kiện escrow refunded.
    /// Luồng xử lý: lưu domain event để handler gửi thông báo/log có đầy đủ ngữ cảnh.
    /// </summary>
    public EscrowRefundedNotification(EscrowRefundedDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    // Domain event gốc chứa dữ liệu hoàn tiền escrow.
    public EscrowRefundedDomainEvent DomainEvent { get; }
}

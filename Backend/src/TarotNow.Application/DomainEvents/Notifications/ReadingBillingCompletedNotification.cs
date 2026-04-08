using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Notifications;

// Notification bao domain event hoàn tất billing cho phiên đọc bài AI.
public sealed class ReadingBillingCompletedNotification : INotification
{
    /// <summary>
    /// Khởi tạo notification cho sự kiện reading billing completed.
    /// Luồng xử lý: lưu domain event để handler downstream thực hiện logging hoặc side-effect.
    /// </summary>
    public ReadingBillingCompletedNotification(ReadingBillingCompletedDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    // Domain event gốc chứa trạng thái cuối cùng của phiên billing.
    public ReadingBillingCompletedDomainEvent DomainEvent { get; }
}

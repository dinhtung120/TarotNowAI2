
using System;

namespace TarotNow.Domain.Events;

// Domain event phát sinh khi subscription hết hạn.
public sealed class SubscriptionExpiredDomainEvent : IDomainEvent
{
    // Người dùng có subscription hết hạn.
    public Guid UserId { get; init; }

    // Subscription đã hết hạn.
    public Guid SubscriptionId { get; init; }

    // Thời điểm phát sinh sự kiện (UTC).
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Khởi tạo sự kiện subscription expired để kích hoạt luồng thu hồi entitlement.
    /// Luồng xử lý: gán user/subscription và sử dụng thời điểm UTC hiện tại làm mốc phát sinh.
    /// </summary>
    public SubscriptionExpiredDomainEvent(Guid userId, Guid subscriptionId)
    {
        UserId = userId;
        SubscriptionId = subscriptionId;
    }
}

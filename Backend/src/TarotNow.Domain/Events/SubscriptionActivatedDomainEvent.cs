
using System;

namespace TarotNow.Domain.Events;

// Domain event phát sinh khi subscription của người dùng được kích hoạt.
public sealed class SubscriptionActivatedDomainEvent : IDomainEvent
{
    // Người dùng được kích hoạt gói.
    public Guid UserId { get; init; }

    // Subscription vừa kích hoạt.
    public Guid SubscriptionId { get; init; }

    // Plan được kích hoạt.
    public Guid PlanId { get; init; }

    // Thời điểm phát sinh sự kiện (UTC).
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Khởi tạo sự kiện kích hoạt subscription để publish tới các luồng cấp quyền lợi.
    /// Luồng xử lý: gán user/subscription/plan; thời gian phát sinh dùng mặc định UTC hiện tại.
    /// </summary>
    public SubscriptionActivatedDomainEvent(Guid userId, Guid subscriptionId, Guid planId)
    {
        UserId = userId;
        SubscriptionId = subscriptionId;
        PlanId = planId;
    }
}

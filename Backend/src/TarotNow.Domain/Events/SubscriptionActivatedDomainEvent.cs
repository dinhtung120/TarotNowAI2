/*
 * ===================================================================
 * FILE: SubscriptionActivatedDomainEvent.cs
 * NAMESPACE: TarotNow.Domain.Events
 * ===================================================================
 * MỤC ĐÍCH:
 *   Domain Event được bắn ra (publish) khi người dùng vừa thanh toán và kích hoạt thành công một gói Subscription.
 *   Lý do sử dụng Domain Event: Giúp tách rời (decouple) logic chính (Ghi Database) ra khỏi các side-effects (như Xóa Cache, Đẩy Notification Real-time).
 *   Sử dụng C# Records để đảm bảo tính bất biến (Immutable), một event khi đã sinh ra thì không bị thay đổi.
 * ===================================================================
 */

using System;

namespace TarotNow.Domain.Events;

/// <summary>
/// Sự kiện: Gói Subscription vừa được kích hoạt thành công.
/// Được lắng nghe bởi các Handlers để cập nhật Cache và báo Notification cho Frontend.
/// </summary>
/// <param name="UserId">ID của người dùng sở hữu gói</param>
/// <param name="SubscriptionId">ID của hồ sơ đăng ký (UserSubscription)</param>
/// <param name="PlanId">ID của loại gói (SubscriptionPlan)</param>
public sealed class SubscriptionActivatedDomainEvent : IDomainEvent
{
    public Guid UserId { get; init; }
    public Guid SubscriptionId { get; init; }
    public Guid PlanId { get; init; }
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    public SubscriptionActivatedDomainEvent(Guid userId, Guid subscriptionId, Guid planId)
    {
        UserId = userId;
        SubscriptionId = subscriptionId;
        PlanId = planId;
    }
}

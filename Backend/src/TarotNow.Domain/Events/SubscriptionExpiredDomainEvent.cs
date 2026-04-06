/*
 * ===================================================================
 * FILE: SubscriptionExpiredDomainEvent.cs
 * NAMESPACE: TarotNow.Domain.Events
 * ===================================================================
 * MỤC ĐÍCH:
 *   Domain Event được System Background Job kích hoạt khi quét thấy một gói Subscription đã quá hạn (EndDate < Hiện tại).
 *   Lý do: Cần xóa cache entitlements hiện tại của user để họ không thể lạm dụng consume tiếp, đồng thời gửi thông báo khuyến nghị gia hạn.
 * ===================================================================
 */

using System;

namespace TarotNow.Domain.Events;

/// <summary>
/// Sự kiện: Gói Subscription đã hết hạn.
/// </summary>
/// <param name="UserId">ID của người dùng sở hữu</param>
/// <param name="SubscriptionId">ID của hồ sơ đăng ký đã hết hạn</param>
public sealed class SubscriptionExpiredDomainEvent : IDomainEvent
{
    public Guid UserId { get; init; }
    public Guid SubscriptionId { get; init; }
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    public SubscriptionExpiredDomainEvent(Guid userId, Guid subscriptionId)
    {
        UserId = userId;
        SubscriptionId = subscriptionId;
    }
}

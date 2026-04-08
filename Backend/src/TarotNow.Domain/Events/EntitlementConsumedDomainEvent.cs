
using System;

namespace TarotNow.Domain.Events;

// Domain event phát sinh khi quota entitlement bị tiêu thụ thành công.
public sealed class EntitlementConsumedDomainEvent : IDomainEvent
{
    // Người dùng tiêu thụ entitlement.
    public Guid UserId { get; init; }

    // Khóa entitlement đã tiêu thụ.
    public string EntitlementKey { get; init; }

    // Bucket quota bị trừ.
    public Guid BucketId { get; init; }

    // Thời điểm phát sinh sự kiện (UTC).
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Khởi tạo sự kiện tiêu thụ entitlement để trigger các xử lý hậu kỳ.
    /// Luồng xử lý: gán dữ liệu user/key/bucket; OccurredAtUtc dùng mặc định thời điểm tạo.
    /// </summary>
    public EntitlementConsumedDomainEvent(Guid userId, string entitlementKey, Guid bucketId)
    {
        UserId = userId;
        EntitlementKey = entitlementKey;
        BucketId = bucketId;
    }
}

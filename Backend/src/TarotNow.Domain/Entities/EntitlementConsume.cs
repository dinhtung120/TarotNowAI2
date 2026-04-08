

using System;

namespace TarotNow.Domain.Entities;

// Entity log tiêu thụ entitlement để audit quota đã sử dụng và hỗ trợ idempotency.
public class EntitlementConsume
{
    // Định danh log tiêu thụ.
    public Guid Id { get; private set; }

    // Người dùng tiêu thụ quyền lợi.
    public Guid UserId { get; private set; }

    // Bucket quota đã bị trừ.
    public Guid BucketId { get; private set; }

    // Khóa entitlement đã tiêu thụ.
    public string EntitlementKey { get; private set; } = string.Empty;

    // Thời điểm tiêu thụ.
    public DateTime ConsumedAt { get; private set; }

    // Nguồn nghiệp vụ phát sinh tiêu thụ.
    public string? ReferenceSource { get; private set; }

    // Tham chiếu bản ghi nguồn nghiệp vụ.
    public string? ReferenceId { get; private set; }

    // Khóa idempotency của thao tác consume.
    public string IdempotencyKey { get; private set; } = string.Empty;

    // Navigation tới bucket entitlement.
    public SubscriptionEntitlementBucket Bucket { get; private set; } = null!;

    // Navigation tới user.
    public User User { get; private set; } = null!;

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: để EF khởi tạo entity từ dữ liệu lưu trữ.
    /// </summary>
    protected EntitlementConsume() { }

    /// <summary>
    /// Khởi tạo log tiêu thụ entitlement mới cho một lần trừ quota.
    /// Luồng xử lý: sinh id, gán liên kết user/bucket, lưu tham chiếu nghiệp vụ và mốc thời gian tiêu thụ.
    /// </summary>
    public EntitlementConsume(
        Guid userId,
        Guid bucketId,
        string entitlementKey,
        string? referenceSource,
        string? referenceId,
        string idempotencyKey)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        BucketId = bucketId;
        EntitlementKey = entitlementKey;
        ConsumedAt = DateTime.UtcNow;
        ReferenceSource = referenceSource;
        ReferenceId = referenceId;
        IdempotencyKey = idempotencyKey;
    }
}

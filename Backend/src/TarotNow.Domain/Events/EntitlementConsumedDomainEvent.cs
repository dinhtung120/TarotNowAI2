/*
 * ===================================================================
 * FILE: EntitlementConsumedDomainEvent.cs
 * NAMESPACE: TarotNow.Domain.Events
 * ===================================================================
 * MỤC ĐÍCH:
 *   Domain Event được kích hoạt sau khi User sử dụng thành công một số lượng quyền lợi (Consume Entitlement).
 *   Lý do: Để kích hoạt luồng dọn dẹp Cache số dư quyền lợi ngay lập tức, đảm bảo UI được Sync đồng bộ số lần còn lại để hiển thị cho User.
 * ===================================================================
 */

using System;

namespace TarotNow.Domain.Events;

/// <summary>
/// Sự kiện: Quyền lợi đã bị tiêu thụ để sử dụng một dịch vụ nào đó (ví dụ trải bài, chat AI).
/// </summary>
/// <param name="UserId">ID người dùng</param>
/// <param name="EntitlementKey">Loại quyền lợi đã dùng (Ví dụ: free_spread_3_daily)</param>
/// <param name="BucketId">ID của rổ chứa quota bị trừ (Để track trace)</param>
public sealed class EntitlementConsumedDomainEvent : IDomainEvent
{
    public Guid UserId { get; init; }
    public string EntitlementKey { get; init; }
    public Guid BucketId { get; init; }
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    public EntitlementConsumedDomainEvent(Guid userId, string entitlementKey, Guid bucketId)
    {
        UserId = userId;
        EntitlementKey = entitlementKey;
        BucketId = bucketId;
    }
}

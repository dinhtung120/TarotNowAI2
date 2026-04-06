/*
 * ===================================================================
 * FILE: EntitlementConsume.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Sổ ghi chép lịch sử (Usage Log) cho mỗi lần một Quyền Lợi (Entitlement) thực nhận lệnh bị đốt tróc rụng khỏi Bucket rổ.
 *   Lý do thiết kế: Dùng để làm Bằng Chứng Móc Kéo (Audit/Ledger) nếu User rên rỉ khiếu nại "Ủa Sao Rút Lá Số Thần Thiêng Không Định Ăn Thưởng Của Tao?".
 *   Bảng này sử dụng thiết kế "Append-only" y hệt hệ ví WalletTransaction. Có Mã Chống Bấm 2 Lần (Idempotency Key) Dán Gắn Cuối Cùng.
 * ===================================================================
 */

using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Biên Niên Sử Lưu Vết Đốt Quyền Lợi (Consume Log). 
/// Chỉ Ghi Vào Nhét Không Cho Update Thay Đổi.
/// </summary>
public class EntitlementConsume
{
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Thằng Khách Bị Ép Trừ
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Bị Trừ Tiền Thóc Tới Rương Nào Cụ Thể (Bucket Liên Quan) Để Admin Biết Ngọn Vết Gốc Từ Gói Mua Nào Cho.
    /// </summary>
    public Guid BucketId { get; private set; }

    /// <summary>
    /// Key Nào Bị Cào? (Ví dụ: free_spread_3_daily)
    /// </summary>
    public string EntitlementKey { get; private set; } = string.Empty;

    public DateTime ConsumedAt { get; private set; }

    /// <summary>
    /// Nguồn Trừ Phát Sinh Gây Cháy.
    /// Ví dụ: "reading_session" / "ai_chat"
    /// </summary>
    public string? ReferenceSource { get; private set; }

    /// <summary>
    /// Dây Móc Link Với Thẻ Dịch Vụ Bên Khóa Ngoại Ngoài
    /// Ví dụ ID Trải Bài (Session Id)
    /// </summary>
    public string? ReferenceId { get; private set; }

    /// <summary>
    /// Khớp Mã Nhận Trừ Chống Liên Hoàn Click Lỗi (Idempotency Key Cực Quan Trọng Mảng Tài Chính Web 2).
    /// </summary>
    public string IdempotencyKey { get; private set; } = string.Empty;

    // EF Core Thu Thuật.
    public SubscriptionEntitlementBucket Bucket { get; private set; } = null!;
    public User User { get; private set; } = null!;

    protected EntitlementConsume() { }

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

/*
 * ===================================================================
 * FILE: SubscriptionEntitlementBucket.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   "Cái rổ" (Bucket) chứa số lượt sử dụng đặc quyền DÀNH SAU TỪNG NGÀY riêng biệt của một gói Subscription.
 *   Lý do thiết kế: Nếu một User mua 2 gói khác nhau, họ sẽ có 2 rổ tách biệt.
 *   Dùng mô hình Row-lock trên rổ này để tránh chạy đua (Concurrent Request) khi trừ lượt (Consume) không bị trừ âm.
 * ===================================================================
 */

using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Rổ đặc quyền (Entitlement Bucket). 
/// Mỗi quyền lợi của 1 gói đều chẻ ra 1 dòng ở đây để SQL quản lý khóa (Row Level Lock).
/// </summary>
public class SubscriptionEntitlementBucket
{
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Thuộc về Gói Mua nào?
    /// </summary>
    public Guid UserSubscriptionId { get; private set; }

    /// <summary>
    /// Bản sao dư thừa (Denormalization) của User ID để Search cực nhanh (Tránh Join Bảng Chéo Nặng Nề).
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Khóa Quyền Lợi (Ví Dụ: free_spread_3_daily) ánh xạ với EntitlementKey.
    /// </summary>
    public string EntitlementKey { get; private set; } = string.Empty;

    /// <summary>
    /// Giới hạn hạn ngạch TỐI ĐA trong 1 ngày (Reset mỗi ngày UTC).
    /// </summary>
    public int DailyQuota { get; private set; }

    /// <summary>
    /// Số lần đã bị xé / xài lấy ra trong Ngày Hôm Nay. Cột này luôn biến động.
    /// </summary>
    public int UsedToday { get; private set; }

    /// <summary>
    /// Cột chốt theo dõi Cửa sổ Ngày Giờ (Business Date - UTC/Midnight).
    /// Bọn Batch Job dùng Mốc này Nhìn nếu Thấy Đã Qua Ngày Cũ Quá Hạn Là Reset Giỏ Đắp Đầy Lại Liền.
    /// </summary>
    public DateOnly BusinessDate { get; private set; }

    /// <summary>
    /// Bản sao Dư thừa (Denormalization) Của Ngày Đáo Hạn Từ UserSubscription.
    /// Bắt buộc thiết kế như này để có thể dùng hàm SQL OrderBy(EndDate) mà không Gây Chậm DB (Không Cần Nhúng Thằng Khác Vào Tranh Chấp Trói Trùng).
    /// Đây chính là Mấu Chốt của Thuật Toán Consume "Earliest-Expiry-First" (Earliest-Ex).
    /// </summary>
    public DateTime SubscriptionEndDate { get; private set; }

    // Navigation Cấu Trúc Bụng EF Core
    public UserSubscription UserSubscription { get; private set; } = null!;

    protected SubscriptionEntitlementBucket() { }

    public SubscriptionEntitlementBucket(
        Guid userSubscriptionId, 
        Guid userId, 
        string entitlementKey, 
        int dailyQuota, 
        DateOnly currentDate, 
        DateTime subscriptionEndDate)
    {
        Id = Guid.NewGuid();
        UserSubscriptionId = userSubscriptionId;
        UserId = userId;
        EntitlementKey = entitlementKey;
        DailyQuota = dailyQuota;
        UsedToday = 0;
        BusinessDate = currentDate;
        SubscriptionEndDate = subscriptionEndDate;
    }

    /// <summary>
    /// Kiểm tra xem giỏ rổ hàng có Lấy Được Hạt Nào Không? 
    /// (Lưu Ý: Ngày ở đây phải Khớp Nhá Ngày Hệ Thống Cung. Phải Gọi Lệnh Reset Ngày Trước Nhanh Cả Đã).
    /// </summary>
    public bool CanConsume(DateOnly todayUtc)
    {
        return BusinessDate == todayUtc && UsedToday < DailyQuota;
    }

    /// <summary>
    /// Đốt (Consume) 1 phần Rổ. 
    /// Phải đi Kèm Lệnh Database SQL Row Lock Khác Gọi Khóa Móm.
    /// </summary>
    public void Consume(DateOnly todayUtc)
    {
        if (!CanConsume(todayUtc))
            throw new InvalidOperationException($"Không thể consume entitlement {EntitlementKey}. Quota: {UsedToday}/{DailyQuota}. Date: {BusinessDate} vs {todayUtc}");

        UsedToday++;
    }

    /// <summary>
    /// Giao Nhiệm Vụ Của Nồi Nấu Quét Batch Job Quét Tới Đâu Phủ Quét Set Phẳng Khát Số Về 0 Nếu Đã Mới Ngày Sáng Tinh Sương UTC.
    /// </summary>
    public void ResetForNewDay(DateOnly newDate)
    {
        if (newDate <= BusinessDate)
        {
            // Tránh Reset Cập Nhật lùi Thời Gian Phá Vỡ Ledger
            return;
        }

        UsedToday = 0;
        BusinessDate = newDate;
    }
}

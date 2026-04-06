/*
 * ===================================================================
 * FILE: UserSubscription.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Biên lai (Instance) đại diện cho việc một User đã băm tiền (Diamond) ra để mua một SubscriptionPlan nào đó.
 *   Lý do thiết kế: Cho phép 1 User có ĐỒNG THỜI nhiều gói (Stacking). 
 *   Do đó mỗi biên lai phải track Ngày Mua Cụ Thể, Hạn Mở, và Snapshot Giá Trị Cũ để đối chiếu Lịch sử / Audit.
 * ===================================================================
 */

using System;
using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Gói đăng ký riêng biệt mà người dùng đang sở hữu.
/// </summary>
public class UserSubscription
{
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Khách hàng mua gói.
    /// </summary>
    public Guid UserId { get; private set; }
    
    /// <summary>
    /// Mã nguồn của gói (Để liên kết UI lấy tên gói).
    /// </summary>
    public Guid PlanId { get; private set; }

    /// <summary>
    /// Trạng thái gói: active, expired, cancelled.
    /// Định hình qua Enum SubscriptionStatus.
    /// </summary>
    public string Status { get; private set; } = string.Empty;

    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    /// <summary>
    /// Ghi vết số Kim Cương đã bốc trừ ví hôm mua mốc đó 
    /// (Đề phòng sau này Admin lên giá gói Plan gốc xuống SQL, biên lai này là bằng chứng giá cũ).
    /// </summary>
    public long PricePaidDiamond { get; private set; }

    /// <summary>
    /// Mã khối độc nhất ngăn lừa mạng nhấp nhấn 2 lần một nút "Mua".
    /// </summary>
    public string IdempotencyKey { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    // Navigation Properties EF Core.
    public SubscriptionPlan Plan { get; private set; } = null!;
    public User User { get; private set; } = null!;

    protected UserSubscription() { } // Dành cho EF Core

    public UserSubscription(
        Guid userId, 
        Guid planId, 
        DateTime startDate, 
        DateTime endDate, 
        long pricePaidDiamond, 
        string idempotencyKey)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        PlanId = planId;
        StartDate = startDate;
        EndDate = endDate;
        PricePaidDiamond = pricePaidDiamond;
        IdempotencyKey = idempotencyKey;
        Status = SubscriptionStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Computed logic: Gói có đang kích hoạt xài được không.
    /// Kết hợp cờ Active và Thời gian vượt qua.
    /// </summary>
    public bool IsActive => Status == SubscriptionStatus.Active && EndDate > DateTime.UtcNow;

    /// <summary>
    /// Hết Lúa! Dòng mã bị đóng khi tới hạn kỳ dọn dẹp quét.
    /// </summary>
    public void Expire()
    {
        if (Status != SubscriptionStatus.Active)
            throw new InvalidOperationException($"Không thể hết hạn một gói đang trong trạng thái: {Status}");

        Status = SubscriptionStatus.Expired;
    }

    /// <summary>
    /// Hủy ngang Gói từ Admin Tool dẹp bỏ Dispute/Refund.
    /// </summary>
    public void Cancel()
    {
        Status = SubscriptionStatus.Cancelled;
    }
}

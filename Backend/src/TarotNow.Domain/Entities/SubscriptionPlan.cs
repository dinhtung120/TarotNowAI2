/*
 * ===================================================================
 * FILE: SubscriptionPlan.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Domain Entity đại diện cho một Gói Đăng Ký (Subscription Plan) do Admin cấu hình bán trên cửa hàng.
 *   Ví dụ: "Gói Lời Tiên Tri Cơ Bản - 30 Ngày - Giá 50 Kim Cương".
 *   Lý do thiết kế: Tách rời Definition (Plan) ra khỏi Instance (UserSubscription) để Admin thoải mái đổi giá, 
 *   sửa quyền lợi cho các lượt mua tương lai mà không ảnh hưởng tới những ai đã lỡ mua gói của quá khứ.
 * ===================================================================
 */

using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Gói Đăng Ký (Master Data). Chứa cấu hình tiền, thời hạn, và rổ đặc quyền.
/// </summary>
public class SubscriptionPlan
{
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Tên gói (Ví dụ: "Hội Viên Vàng").
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Giới thiệu mô tả ngắn gọn về đặc quyền.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Giá tiền của gói mua bằng đồng tiền riêng của App (Diamond).
    /// </summary>
    public long PriceDiamond { get; private set; }

    /// <summary>
    /// Thời hạn của gói kể từ ngày mua (Số ngày).
    /// </summary>
    public int DurationDays { get; private set; }

    /// <summary>
    /// Cục chuỗi JSON chứa các mảnh đặc quyền. 
    /// Ví dụ: [{"key": "free_spread_3_daily", "dailyQuota": 2}]
    /// Dùng JSON vì số lượng key có thể rải rộng linh hoạt. EF Core hỗ trợ parse cột này rất mượt.
    /// </summary>
    public string EntitlementsJson { get; private set; } = "[]";

    /// <summary>
    /// Cờ đánh dấu Gói có đang mở để bán không. (Soft Delete).
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Cột sắp xếp để hiện trên giao diện User (Số nhỏ lên trước).
    /// </summary>
    public int DisplayOrder { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected SubscriptionPlan() { } // Dành cho EF Core Reflection

    public SubscriptionPlan(
        string name, 
        string? description, 
        long priceDiamond, 
        int durationDays, 
        string entitlementsJson, 
        int displayOrder)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        PriceDiamond = priceDiamond;
        DurationDays = durationDays;
        EntitlementsJson = entitlementsJson;
        DisplayOrder = displayOrder;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Các tác vụ bảo trì gói do Admin băm sửa.
    /// </summary>
    public void Update(
        string name, 
        string? description, 
        long priceDiamond, 
        int durationDays, 
        string entitlementsJson, 
        int displayOrder, 
        bool isActive)
    {
        Name = name;
        Description = description;
        PriceDiamond = priceDiamond;
        DurationDays = durationDays;
        EntitlementsJson = entitlementsJson;
        DisplayOrder = displayOrder;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }
}

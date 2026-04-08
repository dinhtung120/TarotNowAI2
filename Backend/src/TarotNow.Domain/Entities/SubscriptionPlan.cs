
using System;

namespace TarotNow.Domain.Entities;

// Entity gói subscription để định nghĩa giá, thời hạn và entitlement đi kèm.
public class SubscriptionPlan
{
    // Định danh gói.
    public Guid Id { get; private set; }

    // Tên gói hiển thị.
    public string Name { get; private set; } = string.Empty;

    // Mô tả gói.
    public string? Description { get; private set; }

    // Giá gói theo Diamond.
    public long PriceDiamond { get; private set; }

    // Thời lượng gói theo ngày.
    public int DurationDays { get; private set; }

    // Entitlement của gói ở dạng JSON.
    public string EntitlementsJson { get; private set; } = "[]";

    // Cờ bật/tắt gói.
    public bool IsActive { get; private set; }

    // Thứ tự hiển thị trên UI.
    public int DisplayOrder { get; private set; }

    // Thời điểm tạo gói.
    public DateTime CreatedAt { get; private set; }

    // Thời điểm cập nhật gần nhất.
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: để EF khởi tạo entity từ dữ liệu lưu trữ.
    /// </summary>
    protected SubscriptionPlan() { }

    /// <summary>
    /// Khởi tạo gói subscription mới với trạng thái active mặc định.
    /// Luồng xử lý: sinh id, gán toàn bộ cấu hình đầu vào và chốt CreatedAt.
    /// </summary>
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
    /// Cập nhật cấu hình gói theo dữ liệu quản trị mới nhất.
    /// Luồng xử lý: ghi đè các trường cấu hình và cập nhật mốc UpdatedAt để audit.
    /// </summary>
    public void Update(SubscriptionPlanUpdateDetails details)
    {
        Name = details.Name;
        Description = details.Description;
        PriceDiamond = details.PriceDiamond;
        DurationDays = details.DurationDays;
        EntitlementsJson = details.EntitlementsJson;
        DisplayOrder = details.DisplayOrder;
        IsActive = details.IsActive;
        UpdatedAt = DateTime.UtcNow;
        // Chốt mốc cập nhật để truy vết thay đổi cấu hình gói theo thời gian.
    }
}

// DTO cập nhật gói subscription để gom các trường thay đổi trong một payload.
public sealed record SubscriptionPlanUpdateDetails(
    string Name,
    string? Description,
    long PriceDiamond,
    int DurationDays,
    string EntitlementsJson,
    int DisplayOrder,
    bool IsActive);

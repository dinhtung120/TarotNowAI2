/*
 * ===================================================================
 * FILE: EntitlementMappingRule.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bảng Ma Trận Quy Đổi Quyền Lợi Chéo Nhau (Cross-Key Mapping).
 *   Ví dụ (Business Rule): Khách mua Gói có free_spread_5_daily nhưng lại xài bói Spread_3_Cards (thường tốn free_spread_3_daily). 
 *   Thay vì ép Khách trừ Diamond, ta có thể cho phép ĐỐT 1 LƯỢT 5 LÁ cho 1 TRẢI 3 LÁ (Ratio 1:1).
 *   
 *   Lý do thiết kế TẮT MẶC ĐỊNH (default OFF): Ngăn hệ thống ĂN TRỘM quyền lợi xịn của Khách nếu Hút Sai Cấp Không Báo Trước Gây Cãi Vã.
 * ===================================================================
 */

using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Luật Quét Ánh Xạ Đường Lỗi Cho Phép Đốt Quyền Rơi (Downgrade/Cross-grade).
/// </summary>
public class EntitlementMappingRule
{
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Chìa Rút Kho Ngồn Ban Đầu Đem Đốt Cầm Đi Cất.
    /// VD: free_spread_5_daily
    /// </summary>
    public string SourceKey { get; private set; } = string.Empty;

    /// <summary>
    /// Chìa Đi Rẽ Hướng Mục Tiêu Chuyên Môn Mong Chờ Có Đích.
    /// VD: free_spread_3_daily
    /// </summary>
    public string TargetKey { get; private set; } = string.Empty;

    /// <summary>
    /// Hệ Số Tỉ Lệ Tiêu Hao Đốt Xuống Nổ Lắp Ráp Nếu Tiêu Trừ (Thường là 1.0 Tín Đơn Giản Lẹ).
    /// </summary>
    public decimal Ratio { get; private set; }

    /// <summary>
    /// Công Tắc An Toàn: Có Được Áp Dụng Không Mặc Định LUÔN TẮT Khác Hẳn Ngành Gốc Găm.
    /// </summary>
    public bool IsEnabled { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected EntitlementMappingRule() { }

    public EntitlementMappingRule(
        string sourceKey, 
        string targetKey, 
        decimal ratio, 
        bool isEnabled = false)
    {
        Id = Guid.NewGuid();
        SourceKey = sourceKey;
        TargetKey = targetKey;
        Ratio = ratio;
        IsEnabled = isEnabled;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gạt đòn bẩy cấu hình Admin Đón Trả Giảm Sửa Sổ
    /// </summary>
    public void UpdateRatioOrStatus(decimal ratio, bool isEnabled)
    {
        Ratio = ratio;
        IsEnabled = isEnabled;
        UpdatedAt = DateTime.UtcNow;
    }
}

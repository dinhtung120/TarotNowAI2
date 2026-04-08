
using System;

namespace TarotNow.Domain.Entities;

// Entity rule ánh xạ entitlement để quy đổi quyền lợi giữa các nguồn nghiệp vụ.
public class EntitlementMappingRule
{
    // Định danh rule ánh xạ.
    public Guid Id { get; private set; }

    // Khóa entitlement nguồn.
    public string SourceKey { get; private set; } = string.Empty;

    // Khóa entitlement đích.
    public string TargetKey { get; private set; } = string.Empty;

    // Tỷ lệ quy đổi từ source sang target.
    public decimal Ratio { get; private set; }

    // Cờ bật/tắt rule ánh xạ.
    public bool IsEnabled { get; private set; }

    // Thời điểm tạo rule.
    public DateTime CreatedAt { get; private set; }

    // Thời điểm cập nhật gần nhất.
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: để EF khởi tạo entity từ dữ liệu database.
    /// </summary>
    protected EntitlementMappingRule() { }

    /// <summary>
    /// Khởi tạo rule ánh xạ entitlement mới với tỷ lệ và trạng thái ban đầu.
    /// Luồng xử lý: sinh id, gán source/target/ratio, set cờ enable và mốc tạo.
    /// </summary>
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
    /// Cập nhật tỷ lệ hoặc trạng thái enable của rule khi thay đổi chính sách ánh xạ.
    /// Luồng xử lý: ghi đè ratio/isEnabled và cập nhật mốc UpdatedAt.
    /// </summary>
    public void UpdateRatioOrStatus(decimal ratio, bool isEnabled)
    {
        Ratio = ratio;
        IsEnabled = isEnabled;
        UpdatedAt = DateTime.UtcNow;
        // Lưu dấu thời điểm thay đổi để tiện audit cấu hình quy đổi.
    }
}

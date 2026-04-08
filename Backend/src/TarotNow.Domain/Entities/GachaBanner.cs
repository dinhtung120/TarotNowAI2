

using System;
using System.Collections.Generic;
using System.Linq;

namespace TarotNow.Domain.Entities;

// Entity banner gacha để cấu hình thời gian hiệu lực, chi phí quay và danh sách vật phẩm.
public class GachaBanner
{
    // Định danh banner.
    public Guid Id { get; private set; }

    // Mã banner duy nhất.
    public string Code { get; private set; } = string.Empty;

    // Tên banner tiếng Việt.
    public string NameVi { get; private set; } = string.Empty;

    // Tên banner tiếng Anh.
    public string NameEn { get; private set; } = string.Empty;

    // Mô tả banner tiếng Việt.
    public string? DescriptionVi { get; private set; }

    // Mô tả banner tiếng Anh.
    public string? DescriptionEn { get; private set; }

    // Chi phí Diamond cho mỗi lượt quay.
    public long CostDiamond { get; private set; } = 5;

    // Phiên bản cấu hình odds để truy vết theo từng đợt.
    public string OddsVersion { get; private set; } = string.Empty;

    // Thời điểm bắt đầu hiệu lực của banner.
    public DateTime EffectiveFrom { get; private set; }

    // Thời điểm kết thúc hiệu lực; null nếu banner mở vô thời hạn.
    public DateTime? EffectiveTo { get; private set; }

    // Cờ bật cơ chế pity.
    public bool PityEnabled { get; private set; } = true;

    // Số lượt hard pity tối đa.
    public int HardPityCount { get; private set; } = 90;

    // Cờ bật/tắt banner.
    public bool IsActive { get; private set; } = true;

    // Thời điểm tạo banner.
    public DateTime CreatedAt { get; private set; }

    // Thời điểm cập nhật banner.
    public DateTime UpdatedAt { get; private set; }

    // Danh sách item nội bộ có thể chỉnh sửa trong domain.
    private readonly List<GachaBannerItem> _items = new();

    // Danh sách item chỉ đọc công khai cho bên ngoài.
    public IReadOnlyCollection<GachaBannerItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: để EF khởi tạo entity từ dữ liệu tồn tại.
    /// </summary>
    protected GachaBanner() { }

    /// <summary>
    /// Khởi tạo banner gacha mới với thông tin cấu hình và thời gian hiệu lực.
    /// Luồng xử lý: sinh id, gán metadata banner, cấu hình pity và đặt mốc thời gian tạo/cập nhật.
    /// </summary>
    public GachaBanner(string code, string nameVi, string nameEn, long costDiamond, string oddsVersion, DateTime effectiveFrom, DateTime? effectiveTo = null, bool pityEnabled = true, int hardPityCount = 90)
    {
        Id = Guid.NewGuid();
        Code = code;
        NameVi = nameVi;
        NameEn = nameEn;
        CostDiamond = costDiamond;
        OddsVersion = oddsVersion;
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
        PityEnabled = pityEnabled;
        HardPityCount = hardPityCount;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Kiểm tra banner có đang khả dụng tại thời điểm hiện tại hay không.
    /// Luồng xử lý: kiểm tra cờ IsActive trước, sau đó đối chiếu khoảng thời gian hiệu lực UTC.
    /// </summary>
    public bool IsCurrentlyActive()
    {
        if (!IsActive)
        {
            // Nhánh tắt thủ công luôn ưu tiên để chặn hiển thị/quay ngay lập tức.
            return false;
        }

        var now = DateTime.UtcNow;

        if (now < EffectiveFrom)
        {
            // Edge case: banner chưa đến thời điểm mở, không cho phép active sớm.
            return false;
        }

        if (EffectiveTo.HasValue && now > EffectiveTo.Value)
        {
            // Banner đã quá hạn kết thúc nên không còn hợp lệ để quay.
            return false;
        }

        // Chỉ active khi bật cờ và đang nằm trong khung thời gian hiệu lực.
        return true;
    }

    /// <summary>
    /// Kiểm tra tổng trọng số của item có đúng 10000 basis points hay không.
    /// Luồng xử lý: cộng WeightBasisPoints toàn bộ item và so khớp giá trị chuẩn hệ thống.
    /// </summary>
    public bool ValidateOddsSum(IEnumerable<GachaBannerItem> itemsToValidate)
    {
        // Rule bắt buộc 10000 bps giúp biểu diễn 100% xác suất một cách chính xác.
        return itemsToValidate.Sum(x => x.WeightBasisPoints) == 10000;
    }
}

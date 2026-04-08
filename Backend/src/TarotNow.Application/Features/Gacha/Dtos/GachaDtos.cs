namespace TarotNow.Application.Features.Gacha.Dtos;

// DTO thông tin banner gacha hiển thị cho client.
public class GachaBannerDto
{
    // Mã banner.
    public string Code { get; set; } = string.Empty;

    // Tên banner tiếng Việt.
    public string NameVi { get; set; } = string.Empty;

    // Tên banner tiếng Anh.
    public string NameEn { get; set; } = string.Empty;

    // Mô tả banner tiếng Việt.
    public string? DescriptionVi { get; set; }

    // Mô tả banner tiếng Anh.
    public string? DescriptionEn { get; set; }

    // Chi phí mỗi lượt quay theo kim cương.
    public long CostDiamond { get; set; }

    // Phiên bản cấu hình odds hiện tại của banner.
    public string OddsVersion { get; set; } = string.Empty;

    // Pity count hiện tại của user với banner này.
    public int UserCurrentPity { get; set; }
}

// DTO thông tin odds của một banner.
public class GachaBannerOddsDto
{
    // Mã banner.
    public string BannerCode { get; set; } = string.Empty;

    // Phiên bản odds đang áp dụng.
    public string OddsVersion { get; set; } = string.Empty;

    // Danh sách item trong bảng odds.
    public System.Collections.Generic.List<GachaBannerItemDto> Items { get; set; } = new();
}

// DTO mô tả một item trong bảng odds banner.
public class GachaBannerItemDto
{
    // Độ hiếm item.
    public string Rarity { get; set; } = string.Empty;

    // Loại phần thưởng.
    public string RewardType { get; set; } = string.Empty;

    // Giá trị phần thưởng.
    public string RewardValue { get; set; } = string.Empty;

    // Trọng số xác suất theo đơn vị basis points.
    public int WeightBasisPoints { get; set; }

    // Tên hiển thị tiếng Việt.
    public string DisplayNameVi { get; set; } = string.Empty;

    // Tên hiển thị tiếng Anh.
    public string DisplayNameEn { get; set; } = string.Empty;

    // Icon hiển thị item (nếu có).
    public string? DisplayIcon { get; set; }

    // Xác suất phần trăm quy đổi từ basis points để hiển thị UI.
    public double ProbabilityPercent => WeightBasisPoints / 100.0;
}

// DTO một bản ghi lịch sử quay gacha.
public class GachaHistoryItemDto
{
    // Mã banner đã quay.
    public string BannerCode { get; set; } = string.Empty;

    // Độ hiếm item nhận được.
    public string Rarity { get; set; } = string.Empty;

    // Loại phần thưởng nhận được.
    public string RewardType { get; set; } = string.Empty;

    // Giá trị phần thưởng nhận được.
    public string RewardValue { get; set; } = string.Empty;

    // Kim cương đã tiêu cho lượt quay.
    public long SpentDiamond { get; set; }

    // Cờ cho biết lượt quay có kích hoạt pity hay không.
    public bool WasPityTriggered { get; set; }

    // Thời điểm tạo bản ghi lịch sử.
    public System.DateTime CreatedAt { get; set; }
}

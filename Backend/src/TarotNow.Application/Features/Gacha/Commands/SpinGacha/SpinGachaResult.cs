namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

// DTO kết quả tổng hợp của một batch quay gacha.
public class SpinGachaResult
{
    // Cờ cho biết request quay thành công hay không.
    public bool Success { get; set; }

    // Cờ cho biết kết quả lấy từ idempotent replay.
    public bool IsIdempotentReplay { get; set; }

    // Pity count hiện tại sau batch quay.
    public int CurrentPityCount { get; set; }

    // Ngưỡng hard pity của banner.
    public int HardPityThreshold { get; set; }

    // Cờ cho biết có lượt nào trigger hard pity hay không.
    public bool WasPityTriggered { get; set; }

    // Danh sách item nhận được sau các lượt quay.
    public List<SpinGachaItemResult> Items { get; set; } = new();
}

// DTO kết quả cho từng item trong batch quay.
public class SpinGachaItemResult
{
    // Độ hiếm của item nhận được.
    public string Rarity { get; set; } = string.Empty;

    // Loại phần thưởng (gold/diamond/title...).
    public string RewardType { get; set; } = string.Empty;

    // Giá trị phần thưởng (amount/code).
    public string RewardValue { get; set; } = string.Empty;

    // Tên hiển thị tiếng Việt.
    public string DisplayNameVi { get; set; } = string.Empty;

    // Tên hiển thị tiếng Anh.
    public string DisplayNameEn { get; set; } = string.Empty;

    // Icon hiển thị phần thưởng (nếu có).
    public string? DisplayIcon { get; set; }
}

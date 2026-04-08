

using System;

namespace TarotNow.Domain.Entities;

// Entity item phần thưởng trong banner gacha với xác suất và metadata hiển thị.
public class GachaBannerItem
{
    // Định danh item.
    public Guid Id { get; private set; }

    // Định danh banner cha.
    public Guid BannerId { get; private set; }

    // Navigation tới banner chứa item.
    public GachaBanner? Banner { get; private set; }

    // Cấp độ hiếm của phần thưởng.
    public string Rarity { get; private set; } = string.Empty;

    // Loại phần thưởng (card/resource/...).
    public string RewardType { get; private set; } = string.Empty;

    // Giá trị phần thưởng theo RewardType.
    public string RewardValue { get; private set; } = string.Empty;

    // Trọng số chọn thưởng theo basis points.
    public int WeightBasisPoints { get; private set; }

    // Tên hiển thị tiếng Việt.
    public string DisplayNameVi { get; private set; } = string.Empty;

    // Tên hiển thị tiếng Anh.
    public string DisplayNameEn { get; private set; } = string.Empty;

    // Icon hiển thị của item.
    public string? DisplayIcon { get; private set; }

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: để EF khởi tạo entity từ dữ liệu lưu trữ.
    /// </summary>
    protected GachaBannerItem() { }

    /// <summary>
    /// Khởi tạo item phần thưởng mới cho banner gacha.
    /// Luồng xử lý: sinh id, gán liên kết banner và toàn bộ metadata cần cho quay + hiển thị.
    /// </summary>
    public GachaBannerItem(Guid bannerId, string rarity, string rewardType, string rewardValue, int weightBasisPoints, string displayNameVi, string displayNameEn, string? displayIcon = null)
    {
        Id = Guid.NewGuid();
        BannerId = bannerId;
        Rarity = rarity;
        RewardType = rewardType;
        RewardValue = rewardValue;
        WeightBasisPoints = weightBasisPoints;
        DisplayNameVi = displayNameVi;
        DisplayNameEn = displayNameEn;
        DisplayIcon = displayIcon;
    }
}

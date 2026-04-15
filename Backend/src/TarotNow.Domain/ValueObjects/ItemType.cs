namespace TarotNow.Domain.ValueObjects;

/// <summary>
/// Tập mã loại item trong kho đồ tarot.
/// </summary>
public static class ItemType
{
    /// <summary>
    /// Item tăng chỉ số/thăng cấp cho một lá bài cụ thể.
    /// </summary>
    public const string CardEnhancer = "card_enhancer";

    /// <summary>
    /// Item tác dụng cho một phiên trải bài.
    /// </summary>
    public const string ReadingBooster = "reading_booster";

    /// <summary>
    /// Item tiêu hao hoặc mở gói đặc biệt.
    /// </summary>
    public const string ConsumableSpecial = "consumable_special";

    /// <summary>
    /// Item danh hiệu hiếm.
    /// </summary>
    public const string RareTitle = "rare_title";
}

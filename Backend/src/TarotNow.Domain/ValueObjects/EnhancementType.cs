namespace TarotNow.Domain.ValueObjects;

/// <summary>
/// Tập mã hiệu ứng nâng cấp áp lên lá bài tarot.
/// </summary>
public static class EnhancementType
{
    /// <summary>
    /// Tăng EXP cho lá bài.
    /// </summary>
    public const string Exp = "exp";

    /// <summary>
    /// Tăng chỉ số tấn công.
    /// </summary>
    public const string Power = "power";

    /// <summary>
    /// Tăng chỉ số phòng thủ.
    /// </summary>
    public const string Defense = "defense";

    /// <summary>
    /// Nâng cấp cấp độ theo tỉ lệ thành công.
    /// </summary>
    public const string LevelUpgrade = "level_upgrade";

    /// <summary>
    /// Cấp thêm lượt trải bài miễn phí.
    /// </summary>
    public const string FreeDraw = "free_draw";

    /// <summary>
    /// Áp dụng chỉ số may mắn.
    /// </summary>
    public const string Luck = "luck";
}

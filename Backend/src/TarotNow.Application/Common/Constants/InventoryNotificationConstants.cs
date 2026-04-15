namespace TarotNow.Application.Common.Constants;

/// <summary>
/// Tập mã type notification cho module inventory.
/// </summary>
public static class InventoryNotificationTypes
{
    /// <summary>
    /// Notification khi user nhận free draw từ item.
    /// </summary>
    public const string FreeDrawGranted = "inventory.free_draw_granted";

    /// <summary>
    /// Notification khi user nhận hiệu ứng may mắn.
    /// </summary>
    public const string LuckApplied = "inventory.luck_applied";
}

/// <summary>
/// Tập template nội dung notification cho inventory.
/// </summary>
public static class InventoryNotificationTemplates
{
    /// <summary>
    /// Tiêu đề tiếng Việt cho free draw.
    /// </summary>
    public const string FreeDrawTitleVi = "Bạn nhận được lượt xem bài miễn phí";

    /// <summary>
    /// Tiêu đề tiếng Anh cho free draw.
    /// </summary>
    public const string FreeDrawTitleEn = "You received a free tarot draw";

    /// <summary>
    /// Tiêu đề tiếng Trung cho free draw.
    /// </summary>
    public const string FreeDrawTitleZh = "你获得了一次免费占卜";

    /// <summary>
    /// Template body tiếng Việt cho free draw.
    /// </summary>
    public const string FreeDrawBodyVi = "Đã nhận {0} lượt miễn phí từ vật phẩm {1}.";

    /// <summary>
    /// Template body tiếng Anh cho free draw.
    /// </summary>
    public const string FreeDrawBodyEn = "Granted {0} free draw(s) from item {1}.";

    /// <summary>
    /// Template body tiếng Trung cho free draw.
    /// </summary>
    public const string FreeDrawBodyZh = "已从道具 {1} 获得 {0} 次免费占卜。";

    /// <summary>
    /// Tiêu đề tiếng Việt cho hiệu ứng may mắn.
    /// </summary>
    public const string LuckAppliedTitleVi = "May mắn đã được kích hoạt";

    /// <summary>
    /// Tiêu đề tiếng Anh cho hiệu ứng may mắn.
    /// </summary>
    public const string LuckAppliedTitleEn = "Luck effect activated";

    /// <summary>
    /// Tiêu đề tiếng Trung cho hiệu ứng may mắn.
    /// </summary>
    public const string LuckAppliedTitleZh = "幸运效果已激活";

    /// <summary>
    /// Template body tiếng Việt cho hiệu ứng may mắn.
    /// </summary>
    public const string LuckAppliedBodyVi = "Bạn đã dùng {0} và nhận {1} điểm may mắn.";

    /// <summary>
    /// Template body tiếng Anh cho hiệu ứng may mắn.
    /// </summary>
    public const string LuckAppliedBodyEn = "You used {0} and gained {1} luck points.";

    /// <summary>
    /// Template body tiếng Trung cho hiệu ứng may mắn.
    /// </summary>
    public const string LuckAppliedBodyZh = "你使用了 {0} 并获得了 {1} 点幸运值。";
}

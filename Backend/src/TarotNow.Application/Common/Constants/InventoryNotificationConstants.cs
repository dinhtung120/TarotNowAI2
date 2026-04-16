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

    /// <summary>
    /// Notification khi user mở mystery pack và nhận reward.
    /// </summary>
    public const string MysteryPackOpened = "inventory.mystery_pack_opened";

    /// <summary>
    /// Notification khi card được tăng chỉ số từ item enhancer.
    /// </summary>
    public const string CardEnhanced = "inventory.card_enhanced";
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
    public const string FreeDrawBodyVi = "Đã nhận {0} lượt miễn phí cho spread {2} lá từ vật phẩm {1}.";

    /// <summary>
    /// Template body tiếng Anh cho free draw.
    /// </summary>
    public const string FreeDrawBodyEn = "Granted {0} free draw(s) for {2}-card spread from item {1}.";

    /// <summary>
    /// Template body tiếng Trung cho free draw.
    /// </summary>
    public const string FreeDrawBodyZh = "已从道具 {1} 获得 {0} 次 {2} 张牌免费占卜。";

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

    /// <summary>
    /// Tiêu đề tiếng Việt cho mystery pack.
    /// </summary>
    public const string MysteryPackOpenedTitleVi = "Bạn đã mở Gói Bài Bí Ẩn";

    /// <summary>
    /// Tiêu đề tiếng Anh cho mystery pack.
    /// </summary>
    public const string MysteryPackOpenedTitleEn = "Mystery pack opened";

    /// <summary>
    /// Tiêu đề tiếng Trung cho mystery pack.
    /// </summary>
    public const string MysteryPackOpenedTitleZh = "神秘卡包已开启";

    /// <summary>
    /// Template body tiếng Việt cho mystery pack.
    /// </summary>
    public const string MysteryPackOpenedBodyVi = "Bạn nhận được vật phẩm {0} x{1} từ {2}.";

    /// <summary>
    /// Template body tiếng Anh cho mystery pack.
    /// </summary>
    public const string MysteryPackOpenedBodyEn = "You received {0} x{1} from {2}.";

    /// <summary>
    /// Template body tiếng Trung cho mystery pack.
    /// </summary>
    public const string MysteryPackOpenedBodyZh = "你从 {2} 获得了 {0} x{1}。";

    /// <summary>
    /// Tiêu đề tiếng Việt cho card enhancement.
    /// </summary>
    public const string CardEnhancedTitleVi = "Lá bài đã được cường hóa";

    /// <summary>
    /// Tiêu đề tiếng Anh cho card enhancement.
    /// </summary>
    public const string CardEnhancedTitleEn = "Card enhancement applied";

    /// <summary>
    /// Tiêu đề tiếng Trung cho card enhancement.
    /// </summary>
    public const string CardEnhancedTitleZh = "卡牌强化已生效";

    /// <summary>
    /// Template body tiếng Việt cho card enhancement.
    /// </summary>
    public const string CardEnhancedBodyVi = "Card {0}: +EXP {1}, +ATK {2}, +DEF {3}, nâng cấp cấp {4}.";

    /// <summary>
    /// Template body tiếng Anh cho card enhancement.
    /// </summary>
    public const string CardEnhancedBodyEn = "Card {0}: +EXP {1}, +ATK {2}, +DEF {3}, level up {4}.";

    /// <summary>
    /// Template body tiếng Trung cho card enhancement.
    /// </summary>
    public const string CardEnhancedBodyZh = "卡牌 {0}: 经验 +{1}, 攻击 +{2}, 防御 +{3}, 升级 {4}。";
}

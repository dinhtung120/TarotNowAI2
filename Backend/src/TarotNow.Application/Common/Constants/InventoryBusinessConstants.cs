using TarotNow.Domain.Enums;

namespace TarotNow.Application.Common.Constants;

/// <summary>
/// Tập hằng số nghiệp vụ cho module inventory.
/// </summary>
public static class InventoryBusinessConstants
{
    /// <summary>
    /// Spread 3 lá.
    /// </summary>
    public const int SpreadCardCount3 = 3;

    /// <summary>
    /// Spread 5 lá.
    /// </summary>
    public const int SpreadCardCount5 = 5;

    /// <summary>
    /// Spread 10 lá.
    /// </summary>
    public const int SpreadCardCount10 = 10;

    /// <summary>
    /// Số vàng thưởng khi dùng Lucky Star đã sở hữu title.
    /// </summary>
    public const long LuckyStarOwnedTitleGoldReward = 500;

    /// <summary>
    /// Mã title thực tế được grant từ item Lucky Star.
    /// </summary>
    public const string LuckyStarTitleCode = "title_lucky_star";

    /// <summary>
    /// Chuẩn hóa spread type sang bucket free draw card count.
    /// </summary>
    public static int? ResolveFreeDrawSpreadCardCount(string spreadType)
    {
        return spreadType switch
        {
            SpreadType.Spread3Cards => SpreadCardCount3,
            SpreadType.Spread5Cards => SpreadCardCount5,
            SpreadType.Spread10Cards => SpreadCardCount10,
            _ => null,
        };
    }

    /// <summary>
    /// Chuẩn hóa item code vé free draw sang bucket spread card count.
    /// </summary>
    public static int? ResolveTicketSpreadCardCount(string itemCode)
    {
        return itemCode switch
        {
            InventoryItemCodes.FreeDrawTicket3 => SpreadCardCount3,
            InventoryItemCodes.FreeDrawTicket5 => SpreadCardCount5,
            InventoryItemCodes.FreeDrawTicket10 => SpreadCardCount10,
            _ => null,
        };
    }
}

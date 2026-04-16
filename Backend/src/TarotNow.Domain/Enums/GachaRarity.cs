
namespace TarotNow.Domain.Enums;

// Tập hằng cấp độ hiếm của phần thưởng gacha.
public static class GachaRarity
{
    // Độ hiếm phổ thông.
    public const string Common = "Common";

    // Độ hiếm hiếm.
    public const string Rare = "Rare";

    // Độ hiếm sử thi.
    public const string Epic = "Epic";

    // Độ hiếm huyền thoại.
    public const string Legendary = "Legendary";

    // Độ hiếm thần thoại.
    public const string Mythic = "Mythic";

    /// <summary>
    /// Kiểm tra rarity có thuộc nhóm hiếm từ Epic trở lên hay không.
    /// </summary>
    public static bool IsAtLeastEpic(string rarity)
    {
        if (string.IsNullOrWhiteSpace(rarity))
        {
            return false;
        }

        return string.Equals(rarity, Epic, StringComparison.OrdinalIgnoreCase)
               || string.Equals(rarity, Legendary, StringComparison.OrdinalIgnoreCase)
               || string.Equals(rarity, Mythic, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Kiểm tra rarity có thuộc nhóm cực hiếm (Legendary/Mythic) hay không.
    /// Sử dụng cho logic Hard Pity mới.
    /// </summary>
    public static bool IsAtLeastLegendary(string rarity)
    {
        if (string.IsNullOrWhiteSpace(rarity))
        {
            return false;
        }

        return string.Equals(rarity, Legendary, StringComparison.OrdinalIgnoreCase)
               || string.Equals(rarity, Mythic, StringComparison.OrdinalIgnoreCase);
    }
}

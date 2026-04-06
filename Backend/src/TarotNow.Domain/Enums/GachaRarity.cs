/*
 * ===================================================================
 * FILE: GachaRarity.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Độ hiếm của các vật phẩm trong hệ thống Gacha (Vòng quay).
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Các cấp độ hiếm của Gacha.
/// Dùng để xác định pity system (ví dụ: Legendary reset pity).
/// </summary>
public static class GachaRarity
{
    public const string Common = "Common";
    public const string Rare = "Rare";
    public const string Epic = "Epic";
    public const string Legendary = "Legendary";
}

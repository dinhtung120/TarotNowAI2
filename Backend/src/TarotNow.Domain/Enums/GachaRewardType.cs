/*
 * ===================================================================
 * FILE: GachaRewardType.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Các loại phần thưởng có thể xóc ra từ hệ thống Gacha.
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Phân loại phần thưởng để Handler biết đường map vào ví hoặc kho đồ (Titile, Card Skin).
/// </summary>
public static class GachaRewardType
{
    public const string Gold = "gold";
    public const string Diamond = "diamond";
    public const string Title = "title";
    public const string CardSkin = "card_skin";
    public const string ExpBoost = "exp_boost";
}

/*
 * FILE: QuestRewardType.cs
 * MỤC ĐÍCH: Enum định nghĩa loại phần thưởng mà user có thể nhận khi hoàn thành nhiệm vụ.
 *   - Gold/Diamond: Nạp vào ví (Wallet)
 *   - Exp: Tăng EXP của tài khoản
 *   - Title: Cấp danh hiệu
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Loại phần thưởng của nhiệm vụ.
/// </summary>
public static class QuestRewardType
{
    public const string Gold = "gold";
    public const string Diamond = "diamond";
    public const string Exp = "exp";
    public const string Title = "title";
}

/*
 * ===================================================================
 * FILE: EntitlementKey.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Định danh các chìa khóa (keys) quyền lợi mà một Subscription cung cấp.
 *   Lý do thiết kế: Entitlement key là khóa nối nối (magic string) giữa việc mua gói (Subscription) và việc tiêu thụ dịch vụ (Consume).
 *   Khai báo ở đây để tránh hard-code rải rác khắp hệ thống, giảm thiểu rủi ro gõ sai tên key.
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Chìa khóa định danh quyền lợi (Entitlement Key). 
/// Khi User thực hiện một hành động (như xem bói), hệ thống sẽ check key này trong giỏ (Bucket) của họ thay vì trừ Diamond.
/// </summary>
public static class EntitlementKey
{
    /// <summary>
    /// Quyền trải bài 3 lá miễn phí.
    /// Áp dụng cho loại SpreadType.Spread3Cards.
    /// </summary>
    public const string FreeSpread3Daily = "free_spread_3_daily";

    /// <summary>
    /// Quyền trải bài 5 lá miễn phí.
    /// Áp dụng cho loại SpreadType.Spread5Cards (Cao cấp hơn, tốn nhiều Diamond hơn nếu mua lẻ).
    /// </summary>
    public const string FreeSpread5Daily = "free_spread_5_daily";

    /// <summary>
    /// Quyền stream AI trò chuyện tiếp diễn (Follow-up) miễn phí.
    /// Tránh việc User bị trừ nhỏ lẻ Diamond liên tục khi hỏi tiếp các câu hỏi nhỏ với AI.
    /// </summary>
    public const string FreeAiStreamDaily = "free_ai_stream_daily";

    /// <summary>
    /// Hệ số nhân điểm kinh nghiệm (EXP).
    /// Không phải entitlement tiêu hao (consumable) thông thường mà là buff thụ động (Passive Buff).
    /// </summary>
    public const string BonusExpMultiplier = "bonus_exp_multiplier";
}

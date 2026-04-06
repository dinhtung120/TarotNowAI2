/*
 * FILE: QuestType.cs
 * MỤC ĐÍCH: Enum định nghĩa các loại nhiệm vụ trong hệ thống Gamification.
 *   - Các loại quest khác nhau sẽ có chu kỳ (period) reset khác nhau
 *   - Daily (Hàng ngày), Weekly (Hàng tuần), Monthly (Hàng tháng), Seasonal (Theo mùa giải)
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Loại nhiệm vụ (Quest) phân loại dựa trên chu kỳ lặp lại.
/// </summary>
public static class QuestType
{
    public const string Daily = "daily";
    public const string Weekly = "weekly";
    public const string Monthly = "monthly";
    public const string Seasonal = "seasonal";
}

using System;
using System.Globalization;

namespace TarotNow.Application.Helpers;

/// <summary>
/// Thần Chú Định Nghĩa Chu Kỳ Thời Gian (Period Key Helper).
/// Giúp thống nhất cách tính chu kỳ (Ngày/Tuần/Tháng/Mùa) trên toàn hệ thống Gamification.
/// Đảm bảo tính nhất quán giữa Service ghi điểm và Query đọc điểm.
/// </summary>
public static class PeriodKeyHelper
{
    /// <summary>
    /// Trả về mã chu kỳ dựa trên loại nhiệm vụ và thời điểm hiện tại.
    /// Dùng ISO 8601 Week Number để đảm bảo tính chính xác toàn cầu.
    /// </summary>
    public static string GetPeriodKey(string questType)
    {
        var now = DateTime.UtcNow;
        return questType.ToLower() switch
        {
            "daily" => now.ToString("yyyy-MM-dd"),
            // Sử dụng ISO Week để tránh lỗi chia dư DayOfYear / 7 của code cũ:
            "weekly" => $"{ISOWeek.GetYear(now)}-W{ISOWeek.GetWeekOfYear(now):D2}",
            "monthly" => now.ToString("yyyy-MM"),
            "seasonal" => $"{now:yyyy}-Q{(now.Month - 1) / 3 + 1}",
            _ => "all"
        };
    }
}

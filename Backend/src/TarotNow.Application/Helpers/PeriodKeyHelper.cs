using System;
using System.Globalization;

namespace TarotNow.Application.Helpers;

// Quy đổi loại kỳ nhiệm vụ sang khóa chuẩn để đồng bộ truy vấn tiến độ và thống kê.
public static class PeriodKeyHelper
{
    /// <summary>
    /// Sinh khóa kỳ dựa trên loại nhiệm vụ để toàn bộ hệ thống dùng chung một chuẩn lưu trữ.
    /// Luồng xử lý: đọc thời gian UTC hiện tại, ánh xạ theo loại kỳ, và trả về khóa mặc định khi loại không hợp lệ.
    /// </summary>
    /// <param name="questType">Loại nhiệm vụ (daily, weekly, monthly, seasonal).</param>
    /// <returns>Khóa kỳ theo định dạng chuẩn của từng loại.</returns>
    public static string GetPeriodKey(string questType)
    {
        var now = DateTime.UtcNow;

        // Dùng UTC để tránh lệch kỳ khi máy chủ hoặc người dùng ở múi giờ khác nhau.
        return questType.ToLower() switch
        {
            "daily" => now.ToString("yyyy-MM-dd"),
            "weekly" => $"{ISOWeek.GetYear(now)}-W{ISOWeek.GetWeekOfYear(now):D2}",
            "monthly" => now.ToString("yyyy-MM"),
            "seasonal" => $"{now:yyyy}-Q{(now.Month - 1) / 3 + 1}",

            // Trả về bucket mặc định để hệ thống vẫn xử lý được loại kỳ ngoài danh sách hỗ trợ.
            _ => "all"
        };
    }
}

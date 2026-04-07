using System;
using System.Globalization;

namespace TarotNow.Application.Helpers;

public static class PeriodKeyHelper
{
        public static string GetPeriodKey(string questType)
    {
        var now = DateTime.UtcNow;
        return questType.ToLower() switch
        {
            "daily" => now.ToString("yyyy-MM-dd"),
            
            "weekly" => $"{ISOWeek.GetYear(now)}-W{ISOWeek.GetWeekOfYear(now):D2}",
            "monthly" => now.ToString("yyyy-MM"),
            "seasonal" => $"{now:yyyy}-Q{(now.Month - 1) / 3 + 1}",
            _ => "all"
        };
    }
}

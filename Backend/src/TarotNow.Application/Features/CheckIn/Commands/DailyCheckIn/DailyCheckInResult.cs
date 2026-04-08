using System;

namespace TarotNow.Application.Features.CheckIn.Commands.DailyCheckIn;

// DTO kết quả của luồng daily check-in.
public class DailyCheckInResult
{
    // Số vàng được cộng trong lần xử lý hiện tại.
    public long GoldRewarded { get; set; }

    // Cờ cho biết user đã check-in trước đó trong ngày hay chưa.
    public bool IsAlreadyCheckedIn { get; set; }

    // Business date UTC của lần check-in.
    public string BusinessDate { get; set; } = string.Empty;

    // Streak hiện tại sau khi xử lý check-in.
    public int CurrentStreak { get; set; }
}

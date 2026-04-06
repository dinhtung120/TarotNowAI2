using System;

namespace TarotNow.Application.Features.CheckIn.Commands.DailyCheckIn;

/// <summary>
/// Hộp thư trả về UX để app báo "Chúc mừng chúa tể đã lượm Vàng".
/// </summary>
public class DailyCheckInResult
{
    // Bao nhiêu Vàng rơi vào túi?
    public long GoldRewarded { get; set; }
    
    // Đã bấm lượn rồi bớt tham spam đi!
    public bool IsAlreadyCheckedIn { get; set; }
    
    // Ngày hệ thống lưu sổ Ghi nhận là ngày nào
    public string BusinessDate { get; set; } = string.Empty;
    
    // Streak hiện tại (dù điểm danh chưa tăng streak rút bài, nhưng cứ trả cho FE hiển thị).
    public int CurrentStreak { get; set; }
}

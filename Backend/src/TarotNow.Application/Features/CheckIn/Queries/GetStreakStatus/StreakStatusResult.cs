using System;

namespace TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;

// DTO trả về trạng thái streak hiện tại và thông tin freeze liên quan.
public class StreakStatusResult
{
    // Streak hiện tại của user.
    public int CurrentStreak { get; set; }

    // Ngày streak gần nhất theo định dạng yyyy-MM-dd (nếu có).
    public string? LastStreakDate { get; set; }

    // Giá trị streak trước khi bị gãy.
    public int PreBreakStreak { get; set; }

    // Cờ cho biết streak hiện đang ở trạng thái bị gãy.
    public bool IsStreakBroken { get; set; }

    // Chi phí kim cương cần trả để mua phục hồi streak.
    public long FreezePrice { get; set; }

    // Số giây còn lại của cửa sổ mua freeze.
    public long FreezeWindowRemainingSeconds { get; set; }

    // Cờ cho biết hiện tại có thể mua freeze hay không.
    public bool CanBuyFreeze { get; set; }

    // Cờ cho biết user đã check-in hôm nay hay chưa.
    public bool TodayCheckedIn { get; set; }

    // Hệ số EXP theo streak hiện tại.
    public double ExpMultiplier { get; set; }
}

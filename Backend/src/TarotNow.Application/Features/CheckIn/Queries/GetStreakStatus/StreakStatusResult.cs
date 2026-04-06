using System;

namespace TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;

/// <summary>
/// Bao tải chở thông tin Streak hiện diện về cho UX tô vẽ ngọn lửa.
/// </summary>
public class StreakStatusResult
{
    // Số ngày liên tục truy cập giữ sóng Wifi
    public int CurrentStreak { get; set; }
    
    // Ngày Gần Nhất Góp Củi
    public string? LastStreakDate { get; set; }
    
    // Ngày điểm streak trước khi đứt dòng máu
    public int PreBreakStreak { get; set; }
    
    // UX nhấp nháy đèn đỏ báo "Streak của mài vỡ dồi kìa"
    public bool IsStreakBroken { get; set; }
    
    // Kim Cương đòi thu nạp để cấp lại mốc cũ
    public long FreezePrice { get; set; }
    
    // Đồng hồ 24h đếm ngược còn bao nhiu giây đứt thở hết được cứu.
    public long FreezeWindowRemainingSeconds { get; set; }
    
    // Có đủ điều kiện nhét đá đắp đập không?
    public bool CanBuyFreeze { get; set; }
    
    // Chỉa cờ điểm danh hôm nay chưa?
    public bool TodayCheckedIn { get; set; }
    
    // Hệ số cày cuốc cấp thẻ (Base = 1.0)
    public double ExpMultiplier { get; set; }
}

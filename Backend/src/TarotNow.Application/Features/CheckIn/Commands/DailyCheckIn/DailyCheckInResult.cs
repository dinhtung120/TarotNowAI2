using System;

namespace TarotNow.Application.Features.CheckIn.Commands.DailyCheckIn;

public class DailyCheckInResult
{
    
    public long GoldRewarded { get; set; }
    
    
    public bool IsAlreadyCheckedIn { get; set; }
    
    
    public string BusinessDate { get; set; } = string.Empty;
    
    
    public int CurrentStreak { get; set; }
}

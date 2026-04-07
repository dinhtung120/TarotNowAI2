using System;

namespace TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;

public class StreakStatusResult
{
    
    public int CurrentStreak { get; set; }
    
    
    public string? LastStreakDate { get; set; }
    
    
    public int PreBreakStreak { get; set; }
    
    
    public bool IsStreakBroken { get; set; }
    
    
    public long FreezePrice { get; set; }
    
    
    public long FreezeWindowRemainingSeconds { get; set; }
    
    
    public bool CanBuyFreeze { get; set; }
    
    
    public bool TodayCheckedIn { get; set; }
    
    
    public double ExpMultiplier { get; set; }
}

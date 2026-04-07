namespace TarotNow.Domain.Entities;

public partial class User
{
        public void IncrementStreak(DateOnly businessDate)
    {
        
        
        if (LastStreakDate.HasValue && LastStreakDate.Value == businessDate)
        {
            return;
        }

        
        CurrentStreak++;
        LastStreakDate = businessDate;
        
        
        UpdatedAt = DateTime.UtcNow;
    }

        public void BreakStreak()
    {
        
        if (CurrentStreak == 0) return;

        
        PreBreakStreak = CurrentStreak;
        
        
        CurrentStreak = 0;

        UpdatedAt = DateTime.UtcNow;
    }

        public double GetStreakExpMultiplier()
    {
        
        return 1.0 + (CurrentStreak / 100.0);
    }

        public void RestoreStreak()
    {
        if (PreBreakStreak <= 0)
        {
            throw new InvalidOperationException("Không có Streak cũ để đóng băng/phục hồi.");
        }

        
        CurrentStreak = PreBreakStreak;
        
        
        PreBreakStreak = 0;

        
        
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        LastStreakDate = today.AddDays(-1);

        UpdatedAt = DateTime.UtcNow;
    }

        public long CalculateFreezePrice()
    {
        if (PreBreakStreak == 0) return 0;
        
        
        return (long)Math.Ceiling(PreBreakStreak / 10.0);
    }
}

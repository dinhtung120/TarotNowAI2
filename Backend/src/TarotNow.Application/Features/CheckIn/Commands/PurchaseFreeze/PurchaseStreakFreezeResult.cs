namespace TarotNow.Application.Features.CheckIn.Commands.PurchaseFreeze;

public class PurchaseStreakFreezeResult
{
    // Đã phục hồi thành công?
    public bool Success { get; set; }
    
    // Streak được xách dậy là mốc bao nhiu?
    public int RestoredStreak { get; set; }
    
    // Bị xẻo bao nhiêu Kim Cương?
    public long DiamondCost { get; set; }
}

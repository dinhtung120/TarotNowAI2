namespace TarotNow.Application.Features.CheckIn.Commands.PurchaseFreeze;

// DTO kết quả của luồng mua phục hồi streak.
public class PurchaseStreakFreezeResult
{
    // Cờ cho biết giao dịch mua freeze có thành công hay không.
    public bool Success { get; set; }

    // Streak được phục hồi sau khi mua thành công.
    public int RestoredStreak { get; set; }

    // Số kim cương đã trừ cho giao dịch.
    public long DiamondCost { get; set; }
}

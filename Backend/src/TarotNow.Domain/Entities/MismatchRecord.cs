namespace TarotNow.Domain.Entities;

public class MismatchRecord
{
    public Guid UserId { get; set; }
    public long UserGoldBalance { get; set; }
    public long LedgerGoldBalance { get; set; }
    public long UserDiamondBalance { get; set; }
    public long LedgerDiamondBalance { get; set; }
}

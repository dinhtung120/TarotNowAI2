namespace TarotNow.Domain.Entities;

public class UserCollection
{
    public Guid UserId { get; private set; }
    public int CardId { get; private set; } // 0 đến 77
    public int Level { get; private set; }
    public int Copies { get; private set; }
    public long ExpGained { get; private set; }
    public DateTime LastDrawnAt { get; private set; }

    protected UserCollection() { }

    public UserCollection(Guid userId, int cardId)
    {
        UserId = userId;
        CardId = cardId;
        Level = 1;
        Copies = 1;
        ExpGained = 0;
        LastDrawnAt = DateTime.UtcNow;
    }
    
    // Hàm Logic Upsert
    public void AddCopy(long expToGain)
    {
        Copies += 1;
        ExpGained += expToGain;
        LastDrawnAt = DateTime.UtcNow;
        // Logic Level up đơn giản: Mỗi 5 copies = 1 level
        // (Sẽ tùy biến theo Document Gacha sau nếu có yêu cầu)
        if (Copies % 5 == 0)
        {
            Level += 1;
        }
    }
}

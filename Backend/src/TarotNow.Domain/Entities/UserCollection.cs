

using System;

namespace TarotNow.Domain.Entities;

public class UserCollection
{
    
    public Guid UserId { get; private set; }
    
    public int CardId { get; private set; } 
    
    
    public int Level { get; private set; }
    
    public int Copies { get; private set; }
    
    public long ExpGained { get; private set; }
    
    public DateTime LastDrawnAt { get; private set; }

    
    public int Atk { get; private set; }
    
    public int Def { get; private set; }

    protected UserCollection() { } 

        public UserCollection(Guid userId, int cardId)
    {
        UserId = userId;
        CardId = cardId;
        Level = 1;
        Copies = 1;
        ExpGained = 0;
        LastDrawnAt = DateTime.UtcNow;
        Atk = 10;
        Def = 10;
    }

        public static UserCollection Rehydrate(UserCollectionSnapshot snapshot)
    {
        return new UserCollection(snapshot.UserId, snapshot.CardId)
        {
            Level = snapshot.Level,
            Copies = snapshot.Copies,
            ExpGained = snapshot.ExpGained,
            LastDrawnAt = snapshot.LastDrawnAt,
            Atk = snapshot.Atk,
            Def = snapshot.Def
        };
    }
    
        public void AddCopy(long expToGain)
    {
        Copies += 1;
        ExpGained += expToGain;
        LastDrawnAt = DateTime.UtcNow;

        
        if (Copies % 5 == 0)
        {
            Level += 1;
        }
    }

        public void ApplyLevelUpStats(int atkBonus, int defBonus)
    {
        Atk += atkBonus;
        Def += defBonus;
    }

        public static (int min, int max) GetStatBonusRange(int newLevel)
    {
        return (10, newLevel * 10);
    }
}

public sealed class UserCollectionSnapshot
{
    public Guid UserId { get; init; }
    public int CardId { get; init; }
    public int Level { get; init; }
    public int Copies { get; init; }
    public long ExpGained { get; init; }
    public DateTime LastDrawnAt { get; init; }
    public int Atk { get; init; }
    public int Def { get; init; }
}

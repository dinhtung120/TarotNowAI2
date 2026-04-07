

using System;

namespace TarotNow.Domain.Entities;

public class DepositPromotion
{
    public Guid Id { get; private set; }
    
    
    public long MinAmountVnd { get; private set; }
    
    
    
    public long BonusDiamond { get; private set; }
    
    
    public bool IsActive { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    protected DepositPromotion() { }

        public DepositPromotion(long minAmountVnd, long bonusDiamond, bool isActive)
    {
        Id = Guid.NewGuid();
        MinAmountVnd = minAmountVnd;
        BonusDiamond = bonusDiamond;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
    }

        public void Update(long minAmountVnd, long bonusDiamond, bool isActive)
    {
        MinAmountVnd = minAmountVnd;
        BonusDiamond = bonusDiamond;
        IsActive = isActive;
    }

        public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}

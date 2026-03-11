using System;

namespace TarotNow.Domain.Entities;

public class DepositPromotion
{
    public Guid Id { get; private set; }
    
    // Số tiền VND tối thiểu để áp dụng khuyến mãi
    public long MinAmountVnd { get; private set; }
    
    // Tỉ lệ thưởng phần trăm (ví dụ: 10 = +10% Diamond) hoặc số Diamond cố định
    // Giả sử ta dùng số Diamond Bonus cố định cho đơn giản, hoặc % 
    public long BonusDiamond { get; private set; }
    
    // Ưu tiên: Nếu có nhiều promotion gối nhau, lấy cái có RequiredAmount cao nhất.
    
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

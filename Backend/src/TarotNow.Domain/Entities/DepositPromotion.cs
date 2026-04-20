using System;

namespace TarotNow.Domain.Entities;

// Entity khuyến mãi nạp tiền để cấu hình ngưỡng nạp và bonus Gold tương ứng.
public class DepositPromotion
{
    // Định danh chương trình khuyến mãi.
    public Guid Id { get; private set; }

    // Mức nạp tối thiểu (VND) để được hưởng khuyến mãi.
    public long MinAmountVnd { get; private set; }

    // Số Gold thưởng thêm khi đạt ngưỡng.
    public long BonusGold { get; private set; }

    // Cờ bật/tắt chương trình khuyến mãi.
    public bool IsActive { get; private set; }

    // Thời điểm tạo chương trình.
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// </summary>
    protected DepositPromotion() { }

    /// <summary>
    /// Khởi tạo chương trình khuyến mãi mới với cấu hình ngưỡng nạp và phần thưởng.
    /// </summary>
    public DepositPromotion(long minAmountVnd, long bonusGold, bool isActive)
    {
        Id = Guid.NewGuid();
        MinAmountVnd = minAmountVnd;
        BonusGold = bonusGold;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cập nhật cấu hình khuyến mãi khi cần điều chỉnh ngưỡng và giá trị thưởng.
    /// </summary>
    public void Update(long minAmountVnd, long bonusGold, bool isActive)
    {
        MinAmountVnd = minAmountVnd;
        BonusGold = bonusGold;
        IsActive = isActive;
    }

    /// <summary>
    /// Bật hoặc tắt nhanh trạng thái hoạt động của chương trình khuyến mãi.
    /// </summary>
    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}



using System;

namespace TarotNow.Domain.Entities;

// Entity khuyến mãi nạp tiền để cấu hình ngưỡng nạp và bonus Diamond tương ứng.
public class DepositPromotion
{
    // Định danh chương trình khuyến mãi.
    public Guid Id { get; private set; }

    // Mức nạp tối thiểu (VND) để được hưởng khuyến mãi.
    public long MinAmountVnd { get; private set; }

    // Số Diamond thưởng thêm khi đạt ngưỡng.
    public long BonusDiamond { get; private set; }

    // Cờ bật/tắt chương trình khuyến mãi.
    public bool IsActive { get; private set; }

    // Thời điểm tạo chương trình.
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: để EF khởi tạo entity từ dữ liệu lưu trữ.
    /// </summary>
    protected DepositPromotion() { }

    /// <summary>
    /// Khởi tạo chương trình khuyến mãi mới với cấu hình ngưỡng nạp và phần thưởng.
    /// Luồng xử lý: sinh id, gán tham số đầu vào và đặt mốc CreatedAt hiện tại.
    /// </summary>
    public DepositPromotion(long minAmountVnd, long bonusDiamond, bool isActive)
    {
        Id = Guid.NewGuid();
        MinAmountVnd = minAmountVnd;
        BonusDiamond = bonusDiamond;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cập nhật cấu hình khuyến mãi khi cần điều chỉnh ngưỡng và giá trị thưởng.
    /// Luồng xử lý: ghi đè các trường cấu hình chính của chương trình hiện tại.
    /// </summary>
    public void Update(long minAmountVnd, long bonusDiamond, bool isActive)
    {
        MinAmountVnd = minAmountVnd;
        BonusDiamond = bonusDiamond;
        IsActive = isActive;
        // Thay đổi cấu hình áp dụng cho các giao dịch nạp phát sinh sau cập nhật.
    }

    /// <summary>
    /// Bật hoặc tắt nhanh trạng thái hoạt động của chương trình khuyến mãi.
    /// Luồng xử lý: cập nhật trực tiếp cờ IsActive theo giá trị đầu vào.
    /// </summary>
    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        // Đổi trạng thái hiệu lực để hệ thống lọc áp dụng đúng chương trình.
    }
}

/*
 * ===================================================================
 * FILE: DepositPromotion.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Entity Mô Tả Chính Sách Bơm Thêm Kim Cương (Khuyến Mãi/Bonus).
 *   Sử Dụng Cho Chiến Dịch Tặng Điểm Thưởng Khuyến Khích Khách Nạp Tiền Đậm.
 * ===================================================================
 */

using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Mẫu Trạm Dữ Liệu Lập Trình Admin Tặng Khuyến Mãi SQL `deposit_promotions`.
/// </summary>
public class DepositPromotion
{
    public Guid Id { get; private set; }
    
    // Khách Phải Cúng Tối Thiểu Vào Cổng App Số Tiền Lớn Bằng Này (VD Trên 2 Lít Mới Hưởng).
    public long MinAmountVnd { get; private set; }
    
    // Tỉ lệ thưởng phần trăm (ví dụ: 10 = +10% Diamond) hoặc số Diamond cố định Tặng.
    // Giả sử ta dùng số Diamond Bonus cố định cho Tặng Lõi Gói.
    public long BonusDiamond { get; private set; }
    
    // Cờ Bật Tắt Cho Admin Khi Chương Trình Tới Hạn (Bật Nó Lên Mới Áp Dụng Cho Đơn Rớt Hôm Nay).
    public bool IsActive { get; private set; }
    // Khoảnh Định Ra Cột Tuyên Truyền Cho Tính Công Bằng Mới Nhất.
    public DateTime CreatedAt { get; private set; }

    protected DepositPromotion() { }

    /// <summary>Sinh Sự Kiện Sập Trần Vàng Khuyến Mãi Mới (Do Sếp Admin Chạy).</summary>
    public DepositPromotion(long minAmountVnd, long bonusDiamond, bool isActive)
    {
        Id = Guid.NewGuid();
        MinAmountVnd = minAmountVnd;
        BonusDiamond = bonusDiamond;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>Lão Quản Trị Thấy Thiếu Thu Nhập Kéo Giảm Bonus Hoặc Tăng Ngưỡng Nạp Xuống.</summary>
    public void Update(long minAmountVnd, long bonusDiamond, bool isActive)
    {
        MinAmountVnd = minAmountVnd;
        BonusDiamond = bonusDiamond;
        IsActive = isActive;
    }

    /// <summary>Nút Tức Cấp Dập Cầu Đóng Nhanh Tự Động Sự Kiện Này Khách Ko Ăn Được Nữa Bằng Toggle Mềm.</summary>
    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}

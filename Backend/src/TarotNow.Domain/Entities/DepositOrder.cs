/*
 * ===================================================================
 * FILE: DepositOrder.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Entity Đại Diện Biên Lai Chờ Thanh Toán Tiền Mặt (VND sang Diamond).
 *   Đúc Mẫu Hoá Đơn Tạm Thời Trực Tiếp Theo Dõi Lệnh Đã Hoàn Thành Của Thằng Cổng Stripe/VNPay Gửi Sang.
 * ===================================================================
 */

using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Tổ Chức Cột Dữ Liệu SQL `deposit_orders` Chuyên Theo Dõi Phiếu Trả Tiền Của Khách.
/// </summary>
public class DepositOrder
{
    // Cột Mã Số Hóa Đơn Duy Nhất (Order Id) - Đem Mã Này Tạo Request Sang VNPay.
    public Guid Id { get; private set; }
    // Khách Hàng Nào Đang Nộp Tiền.
    public Guid UserId { get; private set; }
    
    // Tổng Tiền VNĐ Khách Phải Trả Thật Cho Cổng Thanh Toán.
    public long AmountVnd { get; private set; }
    // Điểm Diamond Quy Đổi Sẽ Phải Nhả Cho Khách (Kèm Thưởng Nếu Có).
    public long DiamondAmount { get; private set; }
    
    // Status Tình Trạng Hóa Đơn: Pending (Chờ Bank Báo), Success (Lúa Về), Failed (Bị Thẻ Lỗi/Khách Rút Chui).
    public string Status { get; private set; } = string.Empty;

    // Idempotency: Mã Bank Khớp Duy Nhất Chống Lừa Đảo Nạp Nhồi 2 Lần.
    public string? TransactionId { get; private set; }

    // Dấu Vết Tỉ Giá Thưởng (FX) Của Khuyến Mãi Gốc Được Áp Dụng Chặn Bank Kiện.
    public string? FxSnapshot { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    // Lúc Tiền Đã Nhẩy Vô Ví Hay Hủy Trắng Đo Xem Mất Xử Lý Bao Lâu.
    public DateTime? ProcessedAt { get; private set; }

    // Rãnh Nối Liên Thông Cho SQL Truy Ra Tên Tuổi Khách Giao Dịch Dễ Ở Bảng User (EF Core Navigation).
    public User User { get; private set; } = null!;

    protected DepositOrder() { } // Constructor Bó Tay Cho Riêng Thằng Bắn SQL EF Core Nó Xài Vô Tự Gọi.

    /// <summary>
    /// Vòi Đẻ Hoá Đơn (Khi Khách Bấm Nút "Tao Mua 5 Kim Cương Bằng 5000Đ").
    /// </summary>
    public DepositOrder(Guid userId, long amountVnd, long diamondAmount)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        AmountVnd = amountVnd;
        DiamondAmount = diamondAmount;
        Status = "Pending";
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Chốt Chặn Xác Thực Thành Công Bơm Lúa Vào Order Khi Webhook Ngân Hàng Kêu Về 'OK'.
    /// </summary>
    public void MarkAsSuccess(string transactionId, string? fxSnapshot = null)
    {
        if (Status == "Success")
            throw new InvalidOperationException("This order is already marked as success.");

        Status = "Success";
        TransactionId = transactionId;
        FxSnapshot = fxSnapshot;
        ProcessedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Ngân Hàng Lắc Đầu "Thẻ Ngèo Lắm Đéo Có Tiền" - Phiếu Bị Hủy Khẩn Cấp.
    /// </summary>
    public void MarkAsFailed(string transactionId)
    {
        if (Status == "Success")
            throw new InvalidOperationException("Cannot fail a successful order.");

        Status = "Failed";
        TransactionId = transactionId;
        ProcessedAt = DateTime.UtcNow;
    }
}

/*
 * ===================================================================
 * FILE: WithdrawalRequest.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Entity Đòi Kim Cương Về Lại Tài Khoản Ngân Hàng Thành Cơm Gạo (Rút Tiền Của Thầy Bói Cho Reader).
 *   Kéo Đơn Cắt Nằm Trực Tiếp Bên Cảnh Sát Chờ Phê Duyệt Để Ngân Hàng Kế Toán Xác Thực Tránh Fake Data.
 * ===================================================================
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity SQL ánh xạ bảng xin xỏ rót Của cải Nhận Trút Vàng Tươi Rút Két App `withdrawal_requests`.
///
/// Các Quy Tắc Sắt Gắn Kèm:
/// - Ít Nhất 50 Viên Kim Cương Thầy Reader Mới Đáng Nhát Xoạc Rút, Đọc Quá Ít Cày Cuốc App Có Ép Tặng (1 Lần Dịch Vụ Ngày Tính Theo Múi Tiêu Chuẩn Nộp Date UTC Chống Spam Đơn Rác Bủa Lưới Thức Liên Tục Rút Cò Con Máy Tắc Kẹt DB).
/// - Khách Ngang Ăn 2 Giọt Phí Giao Dịch Rút Thuế Tiền Phí Sân (Thu Trọng Sắc Nôm Nữa 10% Vạch Phí Cho Mái Quản Trị Hệ Thống Ăn Từ Việc Cho Đi Thuê).
/// - Chui Đổi Biến Kim Cương Sang VNĐ Bọc Rác Cửa Cố Định Tới (1 Cục Bằng 1000 Tiền Trắng Việt).
/// - Nhẫn Cờ Chờ Admin Cốt Giải Ngân Đã Bank Chưa Bank Paid Hay Cấm Giật Hoàn Gấp Quát (Refund Reject).
/// </summary>
[Table("withdrawal_requests")]
public class WithdrawalRequest
{
    // Cột Đinh Định Nghĩa Mẫu Phiếu Trút.
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>Là Lão Nào Phán Khát Quần Giảng Gào Cần Tiền Mưu Nộp Ở Nhà Gõ Lá (Reader ID Đòi Rút Nằm Chung Giây Dây Gọi Nằm UserId Từ Base Chính).</summary>
    [Column("user_id")]
    public Guid UserId { get; set; }

    /// <summary>Gậy Chống Cấm Rút Máy Mọc 2 Lần Trên Ngay Cùng Ngày Múi Chuẩn Admin Đảo UTC (DateOnly Sát Phạt Rút Súng Cạnh).</summary>
    [Column("business_date_utc")]
    public DateOnly BusinessDateUtc { get; set; }

    /// <summary>Số Cục Đá Lửa Diamond Ló Rút Lấy Khỏi Bụng App Đem Sang Phế Áp Kim (Luật Bắt Đầu Từ 50 Cục Cứng Khởi Lên).</summary>
    [Column("amount_diamond")]
    public long AmountDiamond { get; set; }

    /// <summary>Đôn Ngang Tỉ Điện Tỷ Giá VND Nhân Thành Đống Cục = Tức 50 Diamond Đem Chia Ăn Xong Nhác Rút Nhân Cứng Nắm Lấy 1000 Ghi Lòng Tiền Mặt (Amount Cục 50000 vnđ gross gốc).</summary>
    [Column("amount_vnd")]
    public long AmountVnd { get; set; }

    /// <summary>Tính Đống Rác Thả Hóa Bảng Kế Mõm Túm Rẻ App Thu Phí Ngang Tiện Dữ 10% Tổng Tệ Cắn Vô Trước Khi Phát Chặt Tay Giáng (Fee Cho Company).</summary>
    [Column("fee_vnd")]
    public long FeeVnd { get; set; }

    /// <summary>Số Lửa Ngân Vẫn Túi Gửi Thực Về Tới Cây App Bank Tài Khoản Khách Reader Chốt Thẳng Lấy 50k Từ Vnd Gross Đem Trừ Cho Thằng Phí Vnd Fee Đi Cục Căn Rỗng Giả.</summary>
    [Column("net_amount_vnd")]
    public long NetAmountVnd { get; set; }

    // Kho Tên Ngân Hàng Muốn Đưa Đi Xa (MBBank. Agribank Đổ Vào).
    [Column("bank_name")]
    [Required]
    public string BankName { get; set; } = string.Empty;

    // Tạp Ghi Đống Dữ Cứng Thẻ Rút Ghi Thẻ Nhựa Cho Thầy Bói Viết Đăng Lên Cẩn Thận Chuyển Theo (Tên Thật Mõm: TRAN THE B).
    [Column("bank_account_name")]
    [Required]
    public string BankAccountName { get; set; } = string.Empty;

    // Cụt Rẻ Mãng Gọi Kẹt Lỗ Đầu Tạp STK Ghi Ở Trên Giầy: Chống Nạp Nhanh Trót Rỗng.
    [Column("bank_account_number")]
    [Required]
    public string BankAccountNumber { get; set; } = string.Empty;

    /// <summary>Quầng Thở Mệnh Lễ Đòi Rút Máy Đi: Đang Kẹt Cố Xin Chờ (pending) / Giấy Duyệt Của Mẹ Admin Chịu Nhưng Bỏ Đói Bank Dịch Ngân Lắc (approved) / Bank Tiên Đưa Xong Xóa Nhá Sách Ghi Nhảy Đỏ 'DONE Paid Rút' (paid) / Chửi Nhám Giải Tiền Kẹt DB Chối Chặn Kém Bank Lộ Cọc Trả Túi Admin Nhảy Hoàn Reject Bơm Vi (rejected).</summary>
    [Column("status")]
    [Required]
    public string Status { get; set; } = "pending";

    /// <summary>Kẻ Nào Thay Tên Ở Admin Vòng Thượng Nắm Gõ Lỗi Phán Rút Vụ Rút Nhận Rạng Kéo Khóa Bằng Chút Mũi.</summary>
    [Column("admin_id")]
    public Guid? AdminId { get; set; }

    // Dấu Tường Ghi Nhập Mực Sổ Xin Báo Cho Giám Đốc Coi Cáu Vụn Đợi Admin Chửi Góp Thầy Lếu Lá Hack Gian (Lý Do Bị Rớt Trả Refund Rejct Kín Lỗi Có Trống Gì).</summary>
    [Column("admin_note")]
    public string? AdminNote { get; set; }

    // Tiếng Tắt Xóa Giải Trình Đã Gửi Admin Process Đổ Hoàn Mỹ Cắm Lúc Nào Rẽ Bóng Xong Trực.
    [Column("processed_at")]
    public DateTime? ProcessedAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

/*
 * ===================================================================
 * FILE: ConversationStatus.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gắn Cờ Lớn Của Cái Phiên Phòng Chat 1-1 Cho Cả Cuộc (Sống/Chết).
 *   Đồng Bộ Ngôn Ngữ Tới Mongo DB Cấm Lạc Type Enum.
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Thẻ Gắn Mác Nhãn Dán Trạng Thái Sinh Mệnh Sống Của Cái Bản Hội Thoại Phòng Zalo Mọc Ở MongoDB Giữa Reader Lầu Xanh Có User Vô Khán.
///
/// Lifecycle Kiếp Luân Hồi: Chờ Đầu Này Ok → Cho Vô Chat Mở Vàng Cạch → Đóng Sập Hoàn Thành Rút Củi. (pending → active → completed/cancelled/disputed).
/// </summary>
public static class ConversationStatus
{
    /// <summary>Đèn Vàng: Thằng Khách Vừa Bốc Lệnh Thầy Bói Đang Ngủ Chờ Thao Tác Chấp Nhận Khớp 2 Cú (Pending).</summary>
    public const string Pending = "pending";

    /// <summary>Đèn Xanh: Máy Đẩy Text Bật Sáng Mạng Chat Bắt Sóng Hai Chữ Sống Bấm Thả Tin 1-1 Tụt Tốc Ra.</summary>
    public const string Active = "active";

    /// <summary>Chuông Ren Re: Nén Giao Dịch Done, Tính Tiền Xong Mọi Chuyện Hút Đáy Quét Ra File Chốt Sổ Chat Xong Ko Rep Đc Nữa.</summary>
    public const string Completed = "completed";

    /// <summary>Khẩu Gãy Giữa Chừng Do Đứa Khách Đi Ngủ Cúp Ngang Bỏ Lệnh Chat Gọi Hoặc Thầy Dis Quá Lâu Chết.</summary>
    public const string Cancelled = "cancelled";

    /// <summary>Bao Cát Ăn Đập UI Do Lôi Admin Kiện Lên Kiện Xuống Cấm Giết Trẻ Đổi Code Án 2 Tội Khóa Đèn Đỏ Đo Tiền Ở Kiện.</summary>
    public const string Disputed = "disputed";

    /// <summary>Cơ Phép Kiểm Soát Coi Thằng Post Bắn DB Lỗi Enum Bậy Vô Đâm Kẹt Gián (Bắt Guards Clause Check Chữ Đẩy Mất Tiền).</summary>
    public static bool IsValid(string status) =>
        status is Pending or Active or Completed or Cancelled or Disputed;
}

/*
 * ===================================================================
 * FILE: ChatMessageType.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bộ Enum Nhãn Dán Để Chỉ Mặt Gọi Tên Đây Là Cái Tin Chat Text Gì Ở Khung Nhắn.
 *   (Admin Có Gửi System Thông Báo Nổi? Cửa Sổ Bấm Mua Hàng Thanh Toán Gọi Ra Rác UI Nào?)
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Loại Ném Tin Nhắn Ra Khung Cửa Sổ Bubble Chat Giữa Client Khách Và Thầy Bói (UI Rendering Chốt Frontend Vẽ Layout Mới Cho Tiện).
/// Gói Text Chữ Sẽ Dễ Ném Thẳng Ra JSON Bắn Qua HTTP Tới Mongo Tự Link Đọc (Schema Mongo).
/// </summary>
public static class ChatMessageType
{
    /// <summary>Tin Thỏ Thẻ Chat Chữ Trắng Buông Lời Chào Reader (Bong Bóng Bình Thường).</summary>
    public const string Text = "text";

    /// <summary>Loa Phát Thanh Từ Tổng Đài Tự Động Admin Chèn Ngang Khung Giao Diện Cho Nổi Múi Text Màu Xanh Đỏ Sợ Nhận Chữ Nhỏ.</summary>
    public const string System = "system";

    /// <summary>Thẻ Rút Bài Quăng Đè Tấm Lên Face Giữa Đoạn Màn Hình Hai Đứa Cùng Chờ (Giao Hiện Lên Bọc Trái Đất Bóng Cửa Chắn Nhấn Nút Chữ Tiếng Chữ "CardShare").</summary>
    public const string CardShare = "card_share";

    /// <summary>Thạch Bảng Báo Giá Thầy Ném Tờ Đơn Ra "50k Xem Câu Này Em Có Chơi Không Nhấn Accept Phát" (UI Payment).</summary>
    public const string PaymentOffer = "payment_offer";

    /// <summary>Nút Tích Dấu Chốt Tay Xuống Tiền Accept Giam Cục Đá 2 Bên Phía UI Lọc Bọc Thành Câu Text Đồng Ý Nép Trong Góc.</summary>
    public const string PaymentAccept = "payment_accept";

    /// <summary>Thằng Khách Kêu Mắc Đéo Trả Đâu Từ Chối Thầy 1 Vố Câu Chối Bắn Tịt Gắn Khung Từ Chối Offer.</summary>
    public const string PaymentReject = "payment_reject";

    /// <summary>Tấm Bảng Quỷ System Auto Trả Lời Viết Lại Tiền Lên Box Chat "Mày Đã Bị Bom Hẹn Giờ 24H Trả Ví Đây Lời Khẩn Cảnh Báo Lần 1".</summary>
    public const string SystemRefund = "system_refund";

    /// <summary>Bản Di Cáo Cho Khách Quá Nghèo Thầy Đớp Hết Thạch Xanh Vì Cãi Bướng Còi Báo Đạt.</summary>
    public const string SystemRelease = "system_release";

    /// <summary>Màn Khóc To Tòa Án Admin Mở Kẹp Dispute Thùng Lều Án Chặt Phạt Mở Cửa Sổ Call.</summary>
    public const string SystemDispute = "system_dispute";

    /// <summary>Cầm Cái Kiếm Đâm Lấy Mã Type Để Kiểm Tranh Hacker Mò Rác Thêm Rác Kiểu Tin Text Gì Bậy Bạ Hủy Nát UI.</summary>
    public static bool IsValid(string type) =>
        type is Text or System or CardShare or PaymentOffer or PaymentAccept
            or PaymentReject or SystemRefund or SystemRelease or SystemDispute;
}

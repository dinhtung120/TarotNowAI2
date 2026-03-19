/*
 * ===================================================================
 * FILE: QuestionItemStatus.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Enum Mô Tả Trạng Thái Của Từng Câu Text Treo Tiền Cho Escrow Ở Chat (Quá Khứ Mua Từng Con Cá Ở Cốc).
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Tích Trạng Ghi Chữ Rõ Sứ Mạng Từng Sợi Dây Tiền `chat_question_items` Buộc Chân Reader Lại Khách SQL (Thằng Phải Có Tiền Lâu Mới Mòn Cọc Hẹn Bom 24H Trổ Khóa).
/// </summary>
public static class QuestionItemStatus
{
    /// <summary>Đèn Chớp Gọi 1 Mớ Thầy Ném Tờ Rớt Nhắm Gái Chưa Bấm Nhấn Chắn (Mới Gửi Request Xin Đồng Ý Nạp Tiền Từ Ví Dài).</summary>
    public const string Pending = "pending";

    /// <summary>Đã Đút 3 Cục Kim Cương Vô Lòng Khẩu Súng Đóng Băng Khung Thật DB Wallet Trừ Bớt Đá Từ Balance Về Cửa Khóa Lại Đợi Bài.</summary>
    public const string Accepted = "accepted";

    /// <summary>Viết Lách Bói Text Ngoan Xong Nhả Cục Tiền Bắn Thẳng Mồm Reader Không Ai Tiếc Đói Ví Từ Dịch Vụ.</summary>
    public const string Released = "released";

    /// <summary>Bom Đồng Hồ Mọc Rêu Thầy Án Ngang Lì Không Rep Hay Admin Quát Cắt Cơm Trả Refund Về Cho Bố Khách.</summary>
    public const string Refunded = "refunded";

    /// <summary>Cắm Mắt Cầm Rào Chặn Toà Án Khách Lao Vô Tiện Ùa Náo Chữ Cục Đá Máu Đôi Đường.</summary>
    public const string Disputed = "disputed";

    /// <summary>Dò Lệnh Enum Cấm Bị Tiêm Mẽ Hacker Từ Thức Hạt Sạn Khủng Hoảng Hóa Ngang JSON Bắn Phá Status DB.</summary>
    public static bool IsValid(string s) =>
        s is Pending or Accepted or Released or Refunded or Disputed;
}

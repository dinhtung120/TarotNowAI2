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
    /// <summary>Phòng đã tạo nhưng chưa có tương tác thanh toán/messaging.</summary>
    public const string Pending = "pending";

    /// <summary>Đã có tin đầu tiên, đang chờ Reader chấp nhận xử lý phiên.</summary>
    public const string AwaitingAcceptance = "awaiting_acceptance";

    /// <summary>Phiên chat đang hoạt động.</summary>
    public const string Ongoing = "ongoing";

    /// <summary>Hoàn tất và đã settlement.</summary>
    public const string Completed = "completed";

    /// <summary>Hủy bởi người dùng/reader/system.</summary>
    public const string Cancelled = "cancelled";

    /// <summary>Hết hạn SLA hoặc accept window.</summary>
    public const string Expired = "expired";

    /// <summary>Đang tranh chấp, chat chuyển read-only.</summary>
    public const string Disputed = "disputed";

    public static bool IsTerminal(string status)
        => status is Completed or Cancelled or Expired;

    public static bool IsValid(string status) =>
        status is Pending or AwaitingAcceptance or Ongoing or Completed or Cancelled or Expired or Disputed;
}

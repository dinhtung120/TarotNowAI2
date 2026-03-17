namespace TarotNow.Domain.Enums;

/// <summary>
/// Trạng thái của conversation chat 1-1 giữa User và Reader.
///
/// Lifecycle: pending → active → completed/cancelled/disputed
///
/// Tại sao dùng static class + string constants thay vì enum?
/// → Đồng bộ với MongoDB storage (string field).
/// → Tránh mapping enum ↔ string khi read/write.
/// → Nhất quán với pattern ReaderApprovalStatus, ReaderOnlineStatus.
/// </summary>
public static class ConversationStatus
{
    /// <summary>Đang chờ reader xác nhận — conversation vừa được tạo.</summary>
    public const string Pending = "pending";

    /// <summary>Đang hoạt động — cả 2 bên đã join, chat đang diễn ra.</summary>
    public const string Active = "active";

    /// <summary>Hoàn thành — settlement đã xong.</summary>
    public const string Completed = "completed";

    /// <summary>Đã hủy — 1 bên hủy trước khi hoàn thành.</summary>
    public const string Cancelled = "cancelled";

    /// <summary>Đang tranh chấp — admin đang xử lý.</summary>
    public const string Disputed = "disputed";

    /// <summary>
    /// Kiểm tra giá trị status có hợp lệ hay không.
    /// Dùng trong validator/handler để guard clause.
    /// </summary>
    public static bool IsValid(string status) =>
        status is Pending or Active or Completed or Cancelled or Disputed;
}

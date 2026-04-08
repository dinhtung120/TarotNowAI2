
namespace TarotNow.Domain.Enums;

// Tập hằng trạng thái cuộc hội thoại từ lúc khởi tạo tới khi kết thúc/tranh chấp.
public static class ConversationStatus
{
    // Hội thoại mới tạo, chưa xử lý bước tiếp theo.
    public const string Pending = "pending";

    // Đang chờ bên nhận chấp thuận.
    public const string AwaitingAcceptance = "awaiting_acceptance";

    // Hội thoại đang diễn ra.
    public const string Ongoing = "ongoing";

    // Hội thoại đã hoàn tất.
    public const string Completed = "completed";

    // Hội thoại đã bị hủy.
    public const string Cancelled = "cancelled";

    // Hội thoại hết hạn xử lý.
    public const string Expired = "expired";

    // Hội thoại đang ở trạng thái tranh chấp.
    public const string Disputed = "disputed";

    /// <summary>
    /// Kiểm tra trạng thái có phải trạng thái kết thúc hay không.
    /// Luồng xử lý: so khớp status với nhóm terminal status đã định nghĩa.
    /// </summary>
    public static bool IsTerminal(string status)
        => status is Completed or Cancelled or Expired;

    /// <summary>
    /// Kiểm tra trạng thái hội thoại có hợp lệ theo danh sách hệ thống hỗ trợ.
    /// Luồng xử lý: đối chiếu status với toàn bộ hằng trạng thái hợp lệ.
    /// </summary>
    public static bool IsValid(string status) =>
        status is Pending or AwaitingAcceptance or Ongoing or Completed or Cancelled or Expired or Disputed;
}

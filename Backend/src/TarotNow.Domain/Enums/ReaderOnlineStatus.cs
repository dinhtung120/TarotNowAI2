namespace TarotNow.Domain.Enums;

/// <summary>
/// Trạng thái trực tuyến của Reader trong hệ thống.
///
/// Tại sao dùng static class + const string thay vì C# enum?
/// → Giữ nhất quán với pattern hiện tại (UserStatus, UserRole, ReaderApprovalStatus).
/// → Lưu trữ trong DB dưới dạng VARCHAR, dễ đọc và debug hơn số nguyên.
/// → Tránh phải viết converter khi map EF Core hoặc MongoDB.
///
/// Ý nghĩa từng trạng thái:
/// - Online: Reader đang trực tuyến nhưng chưa nhận câu hỏi mới.
/// - Offline: Reader không hoạt động (không hiển thị trong directory).
/// - AcceptingQuestions: Reader đang sẵn sàng nhận câu hỏi — chỉ trạng thái này
///   mới cho phép user gửi câu hỏi chat. Đây là gate check quan trọng (P2-READER-QA-1.2).
/// </summary>
public static class ReaderOnlineStatus
{
    /// <summary>Đang trực tuyến nhưng không nhận câu hỏi mới.</summary>
    public const string Online = "online";

    /// <summary>Không hoạt động — ẩn khỏi directory listing.</summary>
    public const string Offline = "offline";

    /// <summary>Sẵn sàng nhận câu hỏi — trạng thái duy nhất cho phép user gửi chat.</summary>
    public const string AcceptingQuestions = "accepting_questions";
}

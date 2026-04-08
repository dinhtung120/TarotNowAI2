
namespace TarotNow.Domain.Enums;

// Tập hằng trạng thái xử lý report bài viết.
public static class PostReportStatus
{
    // Mới tạo, chưa xử lý.
    public const string Pending = "pending";

    // Đang được đội moderation xử lý.
    public const string Processing = "processing";

    // Đã xử lý xong.
    public const string Resolved = "resolved";

    // Bị từ chối report.
    public const string Rejected = "rejected";
}

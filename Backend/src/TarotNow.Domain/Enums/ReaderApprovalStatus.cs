
namespace TarotNow.Domain.Enums;

// Tập hằng trạng thái duyệt hồ sơ Reader.
public static class ReaderApprovalStatus
{
    // Hồ sơ đang chờ duyệt.
    public const string Pending = "pending";

    // Hồ sơ đã được duyệt.
    public const string Approved = "approved";

    // Hồ sơ bị từ chối.
    public const string Rejected = "rejected";
}

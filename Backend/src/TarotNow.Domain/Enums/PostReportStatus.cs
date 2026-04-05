/*
 * ===================================================================
 * FILE: PostReportStatus.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Trạng thái xử lý của một đơn tố cáo bài viết vi phạm (Report).
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Trạng thái của báo cáo vi phạm cộng đồng.
/// </summary>
public static class PostReportStatus
{
    /// <summary>Đang chờ Admin xử lý.</summary>
    public const string Pending = "pending";

    /// <summary>Admin đang xem xét/điều tra.</summary>
    public const string Processing = "processing";

    /// <summary>Đã xử lý xong (có vi phạm).</summary>
    public const string Resolved = "resolved";

    /// <summary>Đã từ chối (báo cáo sai/không vi phạm).</summary>
    public const string Rejected = "rejected";
}

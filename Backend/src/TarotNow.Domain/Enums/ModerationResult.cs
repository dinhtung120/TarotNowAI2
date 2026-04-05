/*
 * ===================================================================
 * FILE: ModerationResult.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Kết quả phán quyết của Admin khi xử lý báo cáo bài viết vi phạm.
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Hành động kiểm duyệt của Admin.
/// </summary>
public static class ModerationResult
{
    /// <summary>Gửi cảnh cáo đến tác giả.</summary>
    public const string Warn = "warn";

    /// <summary>Xóa bài viết (Soft-delete).</summary>
    public const string RemovePost = "remove_post";

    /// <summary>Xóa bài viết và khóa tài khoản người vi phạm.</summary>
    public const string FreezeAccount = "freeze";

    /// <summary>Không có hành động (Dành cho các case báo cáo sai lệch).</summary>
    public const string NoAction = "no_action";
}

/*
 * ===================================================================
 * FILE: PostVisibility.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Định nghĩa trạng thái hiển thị của một bài viết trên cộng đồng.
 *   - Public: Ai cũng có thể thấy (mặc định)
 *   - FriendsOnly: Chỉ bạn bè mới thấy (dự phòng cho phase sau)
 *   - Private: Chỉ người viết mới thấy
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Quyền riêng tư của bài viết cộng đồng (Community Post).
/// </summary>
public static class PostVisibility
{
    /// <summary>Hiển thị công khai trên feed.</summary>
    public const string Public = "public";

    /// <summary>Chỉ những ai là bạn bè/follower mới thấy (Dự phòng cho tính năng sau này).</summary>
    public const string FriendsOnly = "friends_only";

    /// <summary>Chỉ tác giả mới thấy (Nhật ký cá nhân).</summary>
    public const string Private = "private";
}

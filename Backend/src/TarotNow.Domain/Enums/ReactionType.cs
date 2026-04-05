/*
 * ===================================================================
 * FILE: ReactionType.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Các loại biểu cảm (reaction) mà người dùng có thể thả vào bài viết 
 *   trên cộng đồng.
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Các trạng thái thả cảm xúc cho bài viết.
/// </summary>
public static class ReactionType
{
    /// <summary>Thích.</summary>
    public const string Like = "like";

    /// <summary>Tiếc thương/Buồn.</summary>
    public const string Sad = "sad";

    /// <summary>Yêu thích.</summary>
    public const string Love = "love";

    /// <summary>Sâu sắc/Đáng ngẫm.</summary>
    public const string Insightful = "insightful";

    /// <summary>Cười/Hài hước.</summary>
    public const string Haha = "haha";
}

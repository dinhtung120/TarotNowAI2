/*
 * ===================================================================
 * FILE: CommunityDtos.cs
 * NAMESPACE: TarotNow.Application.Common
 * ===================================================================
 * MỤC ĐÍCH:
 *   Định nghĩa các Data Transfer Objects (DTO) cho Community context.
 * ===================================================================
 */

namespace TarotNow.Application.Common;

/// <summary>
/// Gói dữ liệu chuyển đổi (DTO) cho 1 bài viết trên Feed.
/// </summary>
public class CommunityPostDto
{
    public string Id { get; set; } = string.Empty;
    public string AuthorId { get; set; } = string.Empty;
    public string AuthorDisplayName { get; set; } = string.Empty;
    public string? AuthorAvatarUrl { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Visibility { get; set; } = string.Empty;
    
    // Thống kê dictionary ("like": 10, "love": 5...)
    public Dictionary<string, int> ReactionsCount { get; set; } = new();
    public int TotalReactions { get; set; }
    public int CommentsCount { get; set; }
    
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Gói dữ liệu cho Feed - Kèm theo thông tin Reaction của Viewer hiện tại.
/// </summary>
public class CommunityPostFeedItemDto : CommunityPostDto
{
    /// <summary>Reaction của Viewer hiện tại cho bài viết này (nếu có, ví dụ: "like", "love", hoặc null).</summary>
    public string? ViewerReaction { get; set; }
}

/// <summary>
/// Gói dữ liệu Reaction (Biểu cảm)
/// </summary>
public class CommunityReactionDto
{
    public string Id { get; set; } = string.Empty;
    public string PostId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Gói dữ liệu Bình luận
/// </summary>
public class CommunityCommentDto
{
    public string Id { get; set; } = string.Empty;
    public string PostId { get; set; } = string.Empty;
    public string AuthorId { get; set; } = string.Empty;
    public string AuthorDisplayName { get; set; } = string.Empty;
    public string? AuthorAvatarUrl { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

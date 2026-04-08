namespace TarotNow.Application.Common;

// DTO bài đăng cộng đồng cơ bản.
public class CommunityPostDto
{
    // Định danh bài đăng.
    public string Id { get; set; } = string.Empty;

    // Định danh tác giả bài đăng.
    public string AuthorId { get; set; } = string.Empty;

    // Tên hiển thị tác giả.
    public string AuthorDisplayName { get; set; } = string.Empty;

    // URL avatar tác giả.
    public string? AuthorAvatarUrl { get; set; }

    // Nội dung bài đăng.
    public string Content { get; set; } = string.Empty;

    // Mức hiển thị bài đăng (public/private...).
    public string Visibility { get; set; } = string.Empty;

    // Số lượng reaction theo từng loại.
    public Dictionary<string, int> ReactionsCount { get; set; } = new();

    // Tổng số reaction của bài đăng.
    public int TotalReactions { get; set; }

    // Tổng số bình luận của bài đăng.
    public int CommentsCount { get; set; }

    // Cờ đánh dấu bài đã xóa mềm.
    public bool IsDeleted { get; set; }

    // Thời điểm tạo bài đăng.
    public DateTime CreatedAt { get; set; }

    // Thời điểm cập nhật bài đăng gần nhất.
    public DateTime? UpdatedAt { get; set; }
}

// DTO item feed cho người xem hiện tại, kế thừa thông tin bài đăng cơ bản.
public class CommunityPostFeedItemDto : CommunityPostDto
{
    // Loại reaction hiện tại của người xem trên bài đăng.
    public string? ViewerReaction { get; set; }
}

// DTO một reaction trên bài đăng cộng đồng.
public class CommunityReactionDto
{
    // Định danh reaction.
    public string Id { get; set; } = string.Empty;

    // Định danh bài đăng được reaction.
    public string PostId { get; set; } = string.Empty;

    // Định danh người dùng thực hiện reaction.
    public string UserId { get; set; } = string.Empty;

    // Loại reaction (like/love/...).
    public string Type { get; set; } = string.Empty;

    // Thời điểm tạo reaction.
    public DateTime CreatedAt { get; set; }
}

// DTO bình luận trên bài đăng cộng đồng.
public class CommunityCommentDto
{
    // Định danh bình luận.
    public string Id { get; set; } = string.Empty;

    // Định danh bài đăng chứa bình luận.
    public string PostId { get; set; } = string.Empty;

    // Định danh tác giả bình luận.
    public string AuthorId { get; set; } = string.Empty;

    // Tên hiển thị tác giả bình luận.
    public string AuthorDisplayName { get; set; } = string.Empty;

    // URL avatar tác giả bình luận.
    public string? AuthorAvatarUrl { get; set; }

    // Nội dung bình luận.
    public string Content { get; set; } = string.Empty;

    // Thời điểm tạo bình luận.
    public DateTime CreatedAt { get; set; }
}

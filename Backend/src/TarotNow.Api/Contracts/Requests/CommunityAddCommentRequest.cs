namespace TarotNow.Api.Contracts.Requests;

// Payload thêm bình luận mới cho bài viết cộng đồng.
public class CommunityAddCommentRequest
{
    // Nội dung bình luận người dùng gửi lên.
    public required string Content { get; set; }
}

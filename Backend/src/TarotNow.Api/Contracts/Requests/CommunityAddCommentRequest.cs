namespace TarotNow.Api.Contracts.Requests;

// Payload thêm bình luận mới cho bài viết cộng đồng.
public class CommunityAddCommentRequest
{
    // Nội dung bình luận người dùng gửi lên.
    public required string Content { get; set; }

    // Draft id để map asset upload trước khi comment thật được tạo.
    public string? ContextDraftId { get; set; }
}

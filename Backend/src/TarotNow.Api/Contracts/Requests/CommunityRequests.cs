

using System.ComponentModel.DataAnnotations;

namespace TarotNow.Api.Contracts.Requests;

// Payload tạo bài viết cộng đồng mới.
public class CreatePostBody
{
    // Nội dung chính của bài viết, giới hạn để kiểm soát kích thước lưu trữ và kiểm duyệt.
    [Required]
    [MaxLength(5000)]
    public string Content { get; set; } = string.Empty;

    // Mức hiển thị bài viết để áp dụng đúng rule quyền xem.
    [Required]
    public string Visibility { get; set; } = string.Empty;

    // Draft id để map asset upload trước khi post thật được tạo.
    [MaxLength(128)]
    public string? ContextDraftId { get; set; }
}

// Payload cập nhật nội dung bài viết.
public class UpdatePostBody
{
    // Nội dung mới của bài viết sau khi chỉnh sửa.
    [Required]
    [MaxLength(5000)]
    public string Content { get; set; } = string.Empty;
}

// Payload bật/tắt reaction cho bài viết hoặc bình luận.
public class ToggleReactionBody
{
    // Loại reaction client gửi lên để map về enum nghiệp vụ.
    [Required]
    public string Type { get; set; } = string.Empty;
}

// Payload báo cáo vi phạm bài viết cộng đồng.
public class ReportPostBody
{
    // Mã lý do báo cáo để backend phân loại và ưu tiên xử lý.
    [Required]
    public string ReasonCode { get; set; } = string.Empty;

    // Mô tả chi tiết để moderator có đủ ngữ cảnh ra quyết định.
    [Required]
    [MinLength(10)]
    public string Description { get; set; } = string.Empty;
}

// Payload xử lý kết quả moderation cho báo cáo.
public class ResolveReportBody
{
    // Kết quả xử lý cuối cùng của moderator/admin.
    [Required]
    public string Result { get; set; } = string.Empty;

    // Ghi chú nội bộ cho mục đích audit và đối soát quyết định.
    public string? AdminNote { get; set; }
}

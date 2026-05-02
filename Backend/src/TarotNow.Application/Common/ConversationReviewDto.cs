namespace TarotNow.Application.Common;

/// <summary>
/// DTO đánh giá reader theo từng conversation hoàn tất.
/// </summary>
public class ConversationReviewDto
{
    /// <summary>
    /// Định danh bản ghi review.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Định danh conversation đã completed.
    /// </summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>
    /// Định danh user gửi đánh giá.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Định danh reader được đánh giá.
    /// </summary>
    public string ReaderId { get; set; } = string.Empty;

    /// <summary>
    /// Số sao đánh giá (1..5).
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Nhận xét tùy chọn của user.
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Thời điểm tạo review.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Thời điểm cập nhật cuối (nếu có).
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}

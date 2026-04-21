namespace TarotNow.Application.Common;

/// <summary>
/// DTO đơn yêu cầu trở thành Reader.
/// </summary>
public class ReaderRequestDto
{
    /// <summary>
    /// Định danh đơn yêu cầu.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Định danh người dùng gửi yêu cầu.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái xử lý yêu cầu.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Lời giới thiệu Reader.
    /// </summary>
    public string Bio { get; set; } = string.Empty;

    /// <summary>
    /// Danh sách chuyên môn Reader đăng ký.
    /// </summary>
    public List<string> Specialties { get; set; } = [];

    /// <summary>
    /// Số năm kinh nghiệm.
    /// </summary>
    public int YearsOfExperience { get; set; }

    /// <summary>
    /// Link Facebook.
    /// </summary>
    public string? FacebookUrl { get; set; }

    /// <summary>
    /// Link Instagram.
    /// </summary>
    public string? InstagramUrl { get; set; }

    /// <summary>
    /// Link TikTok.
    /// </summary>
    public string? TikTokUrl { get; set; }

    /// <summary>
    /// Giá Diamond mỗi câu hỏi.
    /// </summary>
    public long DiamondPerQuestion { get; set; }

    /// <summary>
    /// Danh sách tài liệu minh chứng đi kèm.
    /// </summary>
    public List<string> ProofDocuments { get; set; } = [];

    /// <summary>
    /// Ghi chú phản hồi từ quản trị viên.
    /// </summary>
    public string? AdminNote { get; set; }

    /// <summary>
    /// Định danh quản trị viên đã review.
    /// </summary>
    public string? ReviewedBy { get; set; }

    /// <summary>
    /// Thời điểm yêu cầu được review.
    /// </summary>
    public DateTime? ReviewedAt { get; set; }

    /// <summary>
    /// Thời điểm tạo yêu cầu.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Thời điểm cập nhật yêu cầu gần nhất.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Lịch sử duyệt yêu cầu theo từng quyết định của admin.
    /// </summary>
    public List<ReaderRequestReviewHistoryEntryDto> ReviewHistory { get; set; } = [];
}

/// <summary>
/// Bản ghi lịch sử duyệt đơn Reader.
/// </summary>
public class ReaderRequestReviewHistoryEntryDto
{
    /// <summary>
    /// Hành động admin đã thực hiện (approve/reject).
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái đơn sau khi xử lý.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Định danh admin xử lý.
    /// </summary>
    public string ReviewedBy { get; set; } = string.Empty;

    /// <summary>
    /// Ghi chú quyết định của admin.
    /// </summary>
    public string? AdminNote { get; set; }

    /// <summary>
    /// Mốc thời gian admin xử lý.
    /// </summary>
    public DateTime ReviewedAt { get; set; }
}

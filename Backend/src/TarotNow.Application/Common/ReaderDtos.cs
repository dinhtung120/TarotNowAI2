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
}

/// <summary>
/// DTO hồ sơ Reader phục vụ hiển thị danh bạ và profile.
/// </summary>
public class ReaderProfileDto
{
    /// <summary>
    /// Định danh hồ sơ reader.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Định danh user sở hữu hồ sơ.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái hoạt động của reader.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Đơn giá kim cương cho mỗi câu hỏi.
    /// </summary>
    public long DiamondPerQuestion { get; set; }

    /// <summary>
    /// Tiểu sử tiếng Việt.
    /// </summary>
    public string BioVi { get; set; } = string.Empty;

    /// <summary>
    /// Tiểu sử tiếng Anh.
    /// </summary>
    public string BioEn { get; set; } = string.Empty;

    /// <summary>
    /// Tiểu sử tiếng Trung.
    /// </summary>
    public string BioZh { get; set; } = string.Empty;

    /// <summary>
    /// Danh sách chuyên môn/năng lực của reader.
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
    /// Điểm đánh giá trung bình.
    /// </summary>
    public double AvgRating { get; set; }

    /// <summary>
    /// Tổng số lượt đánh giá.
    /// </summary>
    public int TotalReviews { get; set; }

    /// <summary>
    /// Tên hiển thị reader.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// URL ảnh đại diện reader.
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// Thời điểm tạo hồ sơ reader.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Thời điểm cập nhật hồ sơ reader gần nhất.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}

namespace TarotNow.Application.Common;

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

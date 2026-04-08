namespace TarotNow.Application.Common;

// DTO đơn yêu cầu trở thành reader.
public class ReaderRequestDto
{
    // Định danh đơn yêu cầu.
    public string Id { get; set; } = string.Empty;

    // Định danh người dùng gửi yêu cầu.
    public string UserId { get; set; } = string.Empty;

    // Trạng thái xử lý yêu cầu.
    public string Status { get; set; } = string.Empty;

    // Nội dung giới thiệu năng lực của ứng viên.
    public string IntroText { get; set; } = string.Empty;

    // Danh sách tài liệu minh chứng đi kèm.
    public List<string> ProofDocuments { get; set; } = new();

    // Ghi chú phản hồi từ quản trị viên.
    public string? AdminNote { get; set; }

    // Định danh quản trị viên đã review.
    public string? ReviewedBy { get; set; }

    // Thời điểm yêu cầu được review.
    public DateTime? ReviewedAt { get; set; }

    // Thời điểm tạo yêu cầu.
    public DateTime CreatedAt { get; set; }

    // Thời điểm cập nhật yêu cầu gần nhất.
    public DateTime? UpdatedAt { get; set; }
}

// DTO hồ sơ reader phục vụ hiển thị danh bạ và profile.
public class ReaderProfileDto
{
    // Định danh hồ sơ reader.
    public string Id { get; set; } = string.Empty;

    // Định danh user sở hữu hồ sơ.
    public string UserId { get; set; } = string.Empty;

    // Trạng thái hoạt động của reader.
    public string Status { get; set; } = string.Empty;

    // Đơn giá kim cương cho mỗi câu hỏi.
    public long DiamondPerQuestion { get; set; }

    // Tiểu sử tiếng Việt.
    public string BioVi { get; set; } = string.Empty;

    // Tiểu sử tiếng Anh.
    public string BioEn { get; set; } = string.Empty;

    // Tiểu sử tiếng Trung.
    public string BioZh { get; set; } = string.Empty;

    // Danh sách chuyên môn/năng lực của reader.
    public List<string> Specialties { get; set; } = new();

    // Điểm đánh giá trung bình.
    public double AvgRating { get; set; }

    // Tổng số lượt đánh giá.
    public int TotalReviews { get; set; }

    // Tên hiển thị reader.
    public string DisplayName { get; set; } = string.Empty;

    // URL ảnh đại diện reader.
    public string? AvatarUrl { get; set; }

    // Thời điểm tạo hồ sơ reader.
    public DateTime CreatedAt { get; set; }

    // Thời điểm cập nhật hồ sơ reader gần nhất.
    public DateTime? UpdatedAt { get; set; }
}

namespace TarotNow.Application.Common;

/// <summary>
/// DTO đại diện cho đơn xin trở thành Reader — dùng ở Application layer.
///
/// Tại sao dùng DTO thay vì tham chiếu MongoDocument trực tiếp?
/// → Clean Architecture: Application layer KHÔNG được biết về Infrastructure.
/// → Application.csproj chỉ reference Domain.csproj (xem csproj).
/// → Repository sẽ map MongoDocument ↔ DTO nội bộ.
///
/// Lưu ý: Đây là POCO đơn giản, không có business logic.
/// Business logic nằm trong Domain layer (User entity).
/// </summary>
public class ReaderRequestDto
{
    /// <summary>MongoDB ObjectId string — dùng để identify khi approve/reject.</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>UUID user gửi đơn — mapping với users.id (PostgreSQL).</summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>Trạng thái: pending | approved | rejected.</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Lời giới thiệu bản thân.</summary>
    public string IntroText { get; set; } = string.Empty;

    /// <summary>Danh sách URL tài liệu chứng minh.</summary>
    public List<string> ProofDocuments { get; set; } = new();

    /// <summary>Ghi chú admin (nếu đã xử lý).</summary>
    public string? AdminNote { get; set; }

    /// <summary>UUID admin đã xử lý.</summary>
    public string? ReviewedBy { get; set; }

    /// <summary>Thời điểm xử lý.</summary>
    public DateTime? ReviewedAt { get; set; }

    /// <summary>Thời điểm gửi đơn.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Thời điểm cập nhật cuối.</summary>
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO đại diện cho hồ sơ công khai Reader — dùng ở Application layer.
/// </summary>
public class ReaderProfileDto
{
    /// <summary>MongoDB ObjectId string.</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>UUID user — 1:1 với users table.</summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>Trạng thái online: online | offline | accepting_questions.</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Giá Diamond mỗi câu hỏi.</summary>
    public long DiamondPerQuestion { get; set; }

    /// <summary>Bio tiếng Việt.</summary>
    public string BioVi { get; set; } = string.Empty;

    /// <summary>Bio tiếng Anh.</summary>
    public string BioEn { get; set; } = string.Empty;

    /// <summary>Bio tiếng Trung.</summary>
    public string BioZh { get; set; } = string.Empty;

    /// <summary>Danh sách chuyên môn.</summary>
    public List<string> Specialties { get; set; } = new();

    /// <summary>Điểm đánh giá trung bình.</summary>
    public double AvgRating { get; set; }

    /// <summary>Tổng số lượt đánh giá.</summary>
    public int TotalReviews { get; set; }

    /// <summary>Tên hiển thị — denormalized.</summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>URL ảnh đại diện.</summary>
    public string? AvatarUrl { get; set; }

    /// <summary>Thời điểm tạo profile.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Thời điểm cập nhật cuối.</summary>
    public DateTime? UpdatedAt { get; set; }
}

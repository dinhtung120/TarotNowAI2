using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// MongoDB Document cho collection "reader_profiles" — Hồ sơ công khai Reader.
///
/// Mục đích:
/// → Hiển thị thông tin Reader trên directory listing (trang danh sách Reader).
/// → Hỗ trợ tìm kiếm, lọc theo giá/đánh giá/chuyên môn/trạng thái online.
/// → Trạng thái online/offline cập nhật realtime khi Reader toggle.
///
/// Lifecycle:
/// → Tạo tự động khi admin approve reader request.
/// → Reader tự cập nhật bio, pricing, specialties, status.
/// → Soft delete khi reader bị revoke (Phase tương lai).
///
/// Gate check quan trọng (P2-READER-QA-1.2):
/// → Chỉ readers có status = "accepting_questions" mới hiển thị trong directory.
/// → User chỉ có thể gửi chat đến reader đang accepting_questions.
///
/// Tham chiếu: schema.md ## 5. reader_profiles
/// </summary>
public class ReaderProfileDocument
{
    /// <summary>MongoDB ObjectId — primary key tự động.</summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>
    /// UUID user — 1:1 với users table (PostgreSQL).
    /// Mỗi user chỉ có tối đa 1 reader_profile. Unique index đảm bảo.
    /// </summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái online: online | offline | accepting_questions.
    /// Mặc định "offline" khi mới tạo — Reader phải chủ động chuyển.
    /// </summary>
    [BsonElement("status")]
    public string Status { get; set; } = "offline";

    /// <summary>
    /// Giá dịch vụ — số Diamond cho mỗi câu hỏi.
    /// Embedded object để dễ mở rộng thêm pricing tiers trong tương lai.
    /// </summary>
    [BsonElement("pricing")]
    public ReaderPricing Pricing { get; set; } = new();

    /// <summary>
    /// Mô tả bản thân đa ngôn ngữ (vi/en/zh).
    /// Dùng LocalizedText class chung với NotificationDocument.
    /// </summary>
    [BsonElement("bio")]
    public LocalizedText Bio { get; set; } = new();

    /// <summary>
    /// Danh sách chuyên môn của Reader: love, career, general, health, finance...
    /// Dùng cho filter trên directory listing.
    /// </summary>
    [BsonElement("specialties")]
    public List<string> Specialties { get; set; } = new();

    /// <summary>
    /// Thống kê đánh giá — denormalized cho fast read trên listing.
    /// Cập nhật bằng aggregation pipeline khi có review mới.
    /// </summary>
    [BsonElement("stats")]
    public ReaderStats Stats { get; set; } = new();

    /// <summary>Danh sách badge codes (Phase tương lai).</summary>
    [BsonElement("badges")]
    public List<string> Badges { get; set; } = new();

    /// <summary>ObjectId tham chiếu titles._id — danh hiệu đang hiển thị.</summary>
    [BsonElement("title_ref")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? TitleRef { get; set; }

    /// <summary>Tên hiển thị của Reader — denormalized từ users.display_name.</summary>
    [BsonElement("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>URL ảnh đại diện — denormalized từ users.avatar_url.</summary>
    [BsonElement("avatar_url")]
    [BsonIgnoreIfNull]
    public string? AvatarUrl { get; set; }

    /// <summary>Soft delete flag.</summary>
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    /// <summary>Thời điểm soft delete.</summary>
    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    /// <summary>Thời điểm tạo profile.</summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Thời điểm cập nhật cuối.</summary>
    [BsonElement("updated_at")]
    [BsonIgnoreIfNull]
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Thông tin giá dịch vụ Reader.
/// Tách thành class riêng vì tương lai có thể mở rộng thêm:
/// - DiamondPerQuickReading (đọc nhanh)
/// - DiamondPerDetailedReading (đọc chi tiết)
/// - DiscountPercentage (giảm giá cho subscriber)
/// </summary>
public class ReaderPricing
{
    /// <summary>Số Diamond mỗi câu hỏi — giá cơ bản.</summary>
    [BsonElement("diamond_per_question")]
    public long DiamondPerQuestion { get; set; } = 5;
}

/// <summary>
/// Thống kê đánh giá Reader — denormalized cho fast read.
/// Tại sao denormalize thay vì aggregate mỗi lần query?
/// → Trang directory listing cần load nhanh (<200ms).
/// → Tính toán aggregate trên reviews collection mỗi request quá tốn.
/// → Cập nhật khi có review mới: stats.total_reviews++, stats.avg_rating recalc.
/// </summary>
public class ReaderStats
{
    /// <summary>Điểm đánh giá trung bình (1.0 – 5.0).</summary>
    [BsonElement("avg_rating")]
    public double AvgRating { get; set; } = 0;

    /// <summary>Tổng số lượt đánh giá.</summary>
    [BsonElement("total_reviews")]
    public int TotalReviews { get; set; } = 0;
}

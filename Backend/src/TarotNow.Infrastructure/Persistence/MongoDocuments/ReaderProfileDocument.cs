/*
 * FILE: ReaderProfileDocument.cs
 * MỤC ĐÍCH: Schema cho collection "reader_profiles" (MongoDB).
 *   Hồ sơ công khai của Reader (thầy bói) — hiển thị trên trang danh sách Reader.
 *   Chứa: bio, pricing, specialties, stats, trạng thái online/offline.
 *
 *   VÒNG ĐỜI:
 *   → Tạo tự động khi Admin duyệt (approve) đơn xin làm Reader.
 *   → Reader tự cập nhật bio, pricing, specialties, status.
 *   → Unique index (user_id): mỗi user chỉ có tối đa 1 reader profile.
 *
 *   GATE CHECK (P2-READER-QA-1.2):
 *   → Chỉ Reader có status = "accepting_questions" mới hiện trên directory.
 *   → User chỉ gửi chat được tới Reader đang accepting_questions.
 *
 *   Tham chiếu: schema.md §5 (reader_profiles)
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// 1 hồ sơ Reader trong collection "reader_profiles".
/// </summary>
public class ReaderProfileDocument
{
    /// <summary>MongoDB ObjectId — tự sinh.</summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>
    /// UUID user (1:1 với PostgreSQL users table). Unique index đảm bảo 1 user = 1 profile.
    /// </summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái hoạt động: "online" | "offline" | "busy".
    /// Mặc định "offline" khi mới tạo — Reader phải chủ động toggle sang trạng thái khác.
    /// Khác biệt với user thường: Reader có thêm trạng thái "busy".
    /// </summary>
    [BsonElement("status")]
    public string Status { get; set; } = "offline";

    /// <summary>
    /// Giá dịch vụ — hiện tại chỉ có DiamondPerQuestion.
    /// Tách thành class con ReaderPricing để dễ mở rộng thêm tier giá trong tương lai
    /// (ví dụ: giá cho đọc nhanh, đọc chi tiết, giảm giá cho subscriber).
    /// </summary>
    [BsonElement("pricing")]
    public ReaderPricing Pricing { get; set; } = new();

    /// <summary>
    /// Mô tả bản thân (bio) đa ngôn ngữ (vi/en/zh).
    /// Reader viết giới thiệu: kinh nghiệm, phương pháp bói, chuyên môn.
    /// Dùng chung class LocalizedText với NotificationDocument.
    /// </summary>
    [BsonElement("bio")]
    public LocalizedText Bio { get; set; } = new();

    /// <summary>
    /// Danh sách chuyên môn: ["love", "career", "finance", "health", "general"].
    /// Dùng cho bộ lọc (filter) trên trang directory — User chọn "love" → chỉ hiện Reader chuyên tình cảm.
    /// </summary>
    [BsonElement("specialties")]
    public List<string> Specialties { get; set; } = new();

    /// <summary>
    /// Thống kê đánh giá (denormalized — lưu sẵn để UI load nhanh).
    /// Chứa: avg_rating (trung bình sao) và total_reviews (tổng lượt đánh giá).
    /// Cập nhật mỗi khi có review mới (không tính lại mỗi lần query).
    /// </summary>
    [BsonElement("stats")]
    public ReaderStats Stats { get; set; } = new();

    /// <summary>Danh sách badge codes (tính năng tương lai).</summary>
    [BsonElement("badges")]
    public List<string> Badges { get; set; } = new();

    /// <summary>ObjectId → titles collection (danh hiệu đang hiển thị, tính năng tương lai).</summary>
    [BsonElement("title_ref")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? TitleRef { get; set; }

    /// <summary>Tên hiển thị — denormalized từ users.display_name để khỏi JOIN cross-DB.</summary>
    [BsonElement("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>URL ảnh đại diện — denormalized từ users.avatar_url.</summary>
    [BsonElement("avatar_url")]
    [BsonIgnoreIfNull]
    public string? AvatarUrl { get; set; }

    /// <summary>Soft delete flag. True = profile bị ẩn nhưng vẫn trong DB.</summary>
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    [BsonIgnoreIfNull]
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Giá dịch vụ Reader. Tách class riêng để mở rộng thêm tier giá sau.
/// </summary>
public class ReaderPricing
{
    /// <summary>Số Diamond mỗi câu hỏi — giá cơ bản. Mặc định 5.</summary>
    [BsonElement("diamond_per_question")]
    public long DiamondPerQuestion { get; set; } = 5;
}

/// <summary>
/// Thống kê đánh giá Reader — denormalized cho fast read trên trang directory.
/// Tại sao denormalize? → Trang listing cần load &lt;200ms, tính aggregate mỗi lần query quá chậm.
/// </summary>
public class ReaderStats
{
    /// <summary>Điểm trung bình (1.0–5.0).</summary>
    [BsonElement("avg_rating")]
    public double AvgRating { get; set; } = 0;

    /// <summary>Tổng số lượt đánh giá.</summary>
    [BsonElement("total_reviews")]
    public int TotalReviews { get; set; } = 0;
}

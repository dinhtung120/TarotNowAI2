using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

[BsonIgnoreExtraElements]
/// <summary>
/// Document hồ sơ Reader dùng để hiển thị directory và trạng thái hoạt động.
/// </summary>
public class ReaderProfileDocument
{
    /// <summary>
    /// ObjectId của hồ sơ reader.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>
    /// User sở hữu hồ sơ Reader.
    /// </summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái online/offline/busy của Reader.
    /// </summary>
    [BsonElement("status")]
    public string Status { get; set; } = "offline";

    /// <summary>
    /// Cấu hình giá dịch vụ của Reader.
    /// </summary>
    [BsonElement("pricing")]
    public ReaderPricing Pricing { get; set; } = new();

    /// <summary>
    /// Mô tả hồ sơ đa ngôn ngữ.
    /// </summary>
    [BsonElement("bio")]
    public LocalizedText Bio { get; set; } = new();

    /// <summary>
    /// Danh sách chuyên môn Reader.
    /// </summary>
    [BsonElement("specialties")]
    public List<string> Specialties { get; set; } = [];

    /// <summary>
    /// Số năm kinh nghiệm Reader.
    /// </summary>
    [BsonElement("years_of_experience")]
    public int YearsOfExperience { get; set; }

    /// <summary>
    /// Link social của Reader.
    /// </summary>
    [BsonElement("social_links")]
    public ReaderSocialLinks SocialLinks { get; set; } = new();

    /// <summary>
    /// Chỉ số đánh giá hiệu suất Reader.
    /// </summary>
    [BsonElement("stats")]
    public ReaderStats Stats { get; set; } = new();

    /// <summary>
    /// Badge hiển thị trong directory.
    /// </summary>
    [BsonElement("badges")]
    public List<string> Badges { get; set; } = [];

    /// <summary>
    /// Tham chiếu danh hiệu đang gắn cho reader (nếu có).
    /// </summary>
    [BsonElement("title_ref")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? TitleRef { get; set; }

    /// <summary>
    /// Tên hiển thị công khai của Reader.
    /// </summary>
    [BsonElement("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Avatar hiển thị trong directory/chat.
    /// </summary>
    [BsonElement("avatar_url")]
    [BsonIgnoreIfNull]
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// Soft-delete flag của hồ sơ.
    /// </summary>
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Mốc thời gian xóa mềm.
    /// </summary>
    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Thời điểm tạo hồ sơ Reader.
    /// </summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Thời điểm cập nhật gần nhất.
    /// </summary>
    [BsonElement("updated_at")]
    [BsonIgnoreIfNull]
    public DateTime? UpdatedAt { get; set; }
}

[BsonIgnoreExtraElements]
/// <summary>
/// Cấu hình giá Reader.
/// </summary>
public class ReaderPricing
{
    /// <summary>
    /// Đơn giá diamond cho mỗi câu hỏi.
    /// </summary>
    [BsonElement("diamond_per_question")]
    public long DiamondPerQuestion { get; set; } = 50;
}

[BsonIgnoreExtraElements]
/// <summary>
/// Nhóm link mạng xã hội của Reader.
/// </summary>
public class ReaderSocialLinks
{
    /// <summary>
    /// Link Facebook.
    /// </summary>
    [BsonElement("facebook_url")]
    [BsonIgnoreIfNull]
    public string? FacebookUrl { get; set; }

    /// <summary>
    /// Link Instagram.
    /// </summary>
    [BsonElement("instagram_url")]
    [BsonIgnoreIfNull]
    public string? InstagramUrl { get; set; }

    /// <summary>
    /// Link TikTok.
    /// </summary>
    [BsonElement("tiktok_url")]
    [BsonIgnoreIfNull]
    public string? TikTokUrl { get; set; }
}

[BsonIgnoreExtraElements]
/// <summary>
/// Thống kê đánh giá Reader.
/// </summary>
public class ReaderStats
{
    /// <summary>
    /// Điểm đánh giá trung bình.
    /// </summary>
    [BsonElement("avg_rating")]
    public double AvgRating { get; set; }

    /// <summary>
    /// Tổng số lượt đánh giá đã nhận.
    /// </summary>
    [BsonElement("total_reviews")]
    public int TotalReviews { get; set; }
}

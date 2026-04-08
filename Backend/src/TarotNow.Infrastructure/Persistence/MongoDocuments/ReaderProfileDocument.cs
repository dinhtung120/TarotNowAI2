

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

[BsonIgnoreExtraElements]
// Document hồ sơ reader dùng để hiển thị directory và trạng thái hoạt động.
public class ReaderProfileDocument
{
    // ObjectId của hồ sơ reader.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // User sở hữu hồ sơ reader.
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    // Trạng thái online/offline/busy của reader.
    [BsonElement("status")]
    public string Status { get; set; } = "offline";

    // Cấu hình giá dịch vụ của reader.
    [BsonElement("pricing")]
    public ReaderPricing Pricing { get; set; } = new();

    // Mô tả hồ sơ đa ngôn ngữ.
    [BsonElement("bio")]
    public LocalizedText Bio { get; set; } = new();

    // Danh sách chuyên môn reader tự khai báo.
    [BsonElement("specialties")]
    public List<string> Specialties { get; set; } = new();

    // Chỉ số đánh giá hiệu suất reader.
    [BsonElement("stats")]
    public ReaderStats Stats { get; set; } = new();

    // Badge hiển thị trong directory.
    [BsonElement("badges")]
    public List<string> Badges { get; set; } = new();

    // Tham chiếu danh hiệu đang gắn cho reader (nếu có).
    [BsonElement("title_ref")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? TitleRef { get; set; }

    // Tên hiển thị công khai của reader.
    [BsonElement("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    // Avatar hiển thị trong directory/chat.
    [BsonElement("avatar_url")]
    [BsonIgnoreIfNull]
    public string? AvatarUrl { get; set; }

    // Soft-delete flag của hồ sơ.
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    // Mốc thời gian xóa mềm.
    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    // Thời điểm tạo hồ sơ reader.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Thời điểm cập nhật gần nhất.
    [BsonElement("updated_at")]
    [BsonIgnoreIfNull]
    public DateTime? UpdatedAt { get; set; }
}

[BsonIgnoreExtraElements]
// Cấu hình giá reader.
public class ReaderPricing
{
    // Đơn giá diamond cho mỗi câu hỏi.
    [BsonElement("diamond_per_question")]
    public long DiamondPerQuestion { get; set; } = 5;
}

[BsonIgnoreExtraElements]
// Thống kê đánh giá reader.
public class ReaderStats
{
    // Điểm đánh giá trung bình.
    [BsonElement("avg_rating")]
    public double AvgRating { get; set; } = 0;

    // Tổng số lượt đánh giá đã nhận.
    [BsonElement("total_reviews")]
    public int TotalReviews { get; set; } = 0;
}

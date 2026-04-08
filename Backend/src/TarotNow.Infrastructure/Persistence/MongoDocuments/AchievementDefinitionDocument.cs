

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document định nghĩa achievement dùng cho hệ thống gamification.
public class AchievementDefinitionDocument
{
    // ObjectId của document trong Mongo.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // Mã achievement duy nhất để map với logic nghiệp vụ.
    [BsonElement("code")]
    public string Code { get; set; } = string.Empty;

    // Tiêu đề đa ngôn ngữ để hiển thị trên client.
    [BsonElement("title_vi")] public string TitleVi { get; set; } = string.Empty;
    [BsonElement("title_en")] public string TitleEn { get; set; } = string.Empty;
    [BsonElement("title_zh")] public string TitleZh { get; set; } = string.Empty;

    // Mô tả đa ngôn ngữ giải thích điều kiện/ý nghĩa achievement.
    [BsonElement("description_vi")] public string DescriptionVi { get; set; } = string.Empty;
    [BsonElement("description_en")] public string DescriptionEn { get; set; } = string.Empty;
    [BsonElement("description_zh")] public string DescriptionZh { get; set; } = string.Empty;

    // URL icon đại diện achievement.
    [BsonElement("icon")]
    public string? Icon { get; set; }

    // Mã title được tặng kèm khi mở khóa achievement (nếu có).
    [BsonElement("grants_title_code")]
    [BsonIgnoreIfNull]
    public string? GrantsTitleCode { get; set; }

    // Cờ bật/tắt achievement để vận hành feature flag ở mức dữ liệu.
    [BsonElement("is_active")]
    public bool IsActive { get; set; } = true;

    // Mốc tạo định nghĩa achievement phục vụ audit.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}



using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document định nghĩa danh hiệu có thể cấp cho người dùng.
public class TitleDefinitionDocument
{
    // ObjectId của title definition.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // Mã danh hiệu duy nhất.
    [BsonElement("code")]
    public string Code { get; set; } = string.Empty;

    // Tên danh hiệu theo từng ngôn ngữ.
    [BsonElement("name_vi")] public string NameVi { get; set; } = string.Empty;
    [BsonElement("name_en")] public string NameEn { get; set; } = string.Empty;
    [BsonElement("name_zh")] public string NameZh { get; set; } = string.Empty;

    // Mô tả danh hiệu theo từng ngôn ngữ.
    [BsonElement("description_vi")] public string DescriptionVi { get; set; } = string.Empty;
    [BsonElement("description_en")] public string DescriptionEn { get; set; } = string.Empty;
    [BsonElement("description_zh")] public string DescriptionZh { get; set; } = string.Empty;

    // Độ hiếm danh hiệu để hiển thị phân tầng.
    [BsonElement("rarity")]
    public string Rarity { get; set; } = "Common";

    // Cờ bật/tắt danh hiệu.
    [BsonElement("is_active")]
    public bool IsActive { get; set; } = true;

    // Mốc tạo danh hiệu.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

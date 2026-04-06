/*
 * FILE: TitleDefinitionDocument.cs
 * MỤC ĐÍCH: Định nghĩa thông tin Danh hiệu (Titles).
 *   - Các danh hiệu user có thể đạt được và trang bị để hiển thị kế bên tên.
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// Định nghĩa 1 Danh hiệu (Title). (Bảng titles).
/// </summary>
public class TitleDefinitionDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("code")]
    public string Code { get; set; } = string.Empty;

    // --- Localization ---
    [BsonElement("name_vi")] public string NameVi { get; set; } = string.Empty;
    [BsonElement("name_en")] public string NameEn { get; set; } = string.Empty;
    [BsonElement("name_zh")] public string NameZh { get; set; } = string.Empty;

    [BsonElement("description_vi")] public string DescriptionVi { get; set; } = string.Empty;
    [BsonElement("description_en")] public string DescriptionEn { get; set; } = string.Empty;
    [BsonElement("description_zh")] public string DescriptionZh { get; set; } = string.Empty;

    /// <summary>Common, Uncommon, Rare, Epic, Legendary</summary>
    [BsonElement("rarity")]
    public string Rarity { get; set; } = "Common";

    [BsonElement("is_active")]
    public bool IsActive { get; set; } = true;

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

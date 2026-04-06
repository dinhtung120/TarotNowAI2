/*
 * FILE: AchievementDefinitionDocument.cs
 * MỤC ĐÍCH: Cấu trúc của Thành Tựu/Achievement (Vd: Rút 100 lần, Nhận 10 Danh hiệu...).
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// Định nghĩa dữ liệu cho 1 Achievement. (Bảng achievements).
/// </summary>
public class AchievementDefinitionDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("code")]
    public string Code { get; set; } = string.Empty;

    // --- Localization ---
    [BsonElement("title_vi")] public string TitleVi { get; set; } = string.Empty;
    [BsonElement("title_en")] public string TitleEn { get; set; } = string.Empty;
    [BsonElement("title_zh")] public string TitleZh { get; set; } = string.Empty;

    [BsonElement("description_vi")] public string DescriptionVi { get; set; } = string.Empty;
    [BsonElement("description_en")] public string DescriptionEn { get; set; } = string.Empty;
    [BsonElement("description_zh")] public string DescriptionZh { get; set; } = string.Empty;

    /// <summary>Icon hiển thị cho thành tựu (URL hoặc sprite class).</summary>
    [BsonElement("icon")]
    public string? Icon { get; set; }

    /// <summary>Cấp danh hiệu nếu có.</summary>
    [BsonElement("grants_title_code")]
    [BsonIgnoreIfNull]
    public string? GrantsTitleCode { get; set; }

    [BsonElement("is_active")]
    public bool IsActive { get; set; } = true;

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

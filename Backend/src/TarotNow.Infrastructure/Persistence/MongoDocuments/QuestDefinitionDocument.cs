/*
 * FILE: QuestDefinitionDocument.cs
 * MỤC ĐÍCH: Lưu trữ định nghĩa của bài tập/nhiệm vụ (Quests) trong MongoDB.
 *   - Các quest được admin tạo, sau đó user sẽ làm theo.
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// Định nghĩa cấu trúc của một nhiệm vụ/Quest. (Bảng quests).
/// </summary>
public class QuestDefinitionDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>Mã code duy nhất (VD: daily_1_reading, weekly_checkin_5).</summary>
    [BsonElement("code")]
    public string Code { get; set; } = string.Empty;

    // --- Localization ---
    [BsonElement("title_vi")] public string TitleVi { get; set; } = string.Empty;
    [BsonElement("title_en")] public string TitleEn { get; set; } = string.Empty;
    [BsonElement("title_zh")] public string TitleZh { get; set; } = string.Empty;

    [BsonElement("description_vi")] public string DescriptionVi { get; set; } = string.Empty;
    [BsonElement("description_en")] public string DescriptionEn { get; set; } = string.Empty;
    [BsonElement("description_zh")] public string DescriptionZh { get; set; } = string.Empty;

    /// <summary>daily, weekly, monthly, seasonal (Dùng QuestType enum)</summary>
    [BsonElement("quest_type")]
    public string QuestType { get; set; } = string.Empty;

    /// <summary>ReadingCompleted, DailyCheckin, v.v để backend hook gọi đúng.</summary>
    [BsonElement("trigger_event")]
    public string TriggerEvent { get; set; } = string.Empty;

    /// <summary>Số lượng tiến độ cần đạt (VD: 5 bài => Target = 5).</summary>
    [BsonElement("target")]
    public int Target { get; set; }

    /// <summary>Mảng chứa các phần thưởng.</summary>
    [BsonElement("rewards")]
    public List<QuestRewardItem> Rewards { get; set; } = new();

    [BsonElement("is_active")]
    public bool IsActive { get; set; } = true;

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class QuestRewardItem
{
    /// <summary>Loại quà: gold, diamond, exp, title</summary>
    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>Số lượng, nếu là tiền.</summary>
    [BsonElement("amount")]
    public int Amount { get; set; }

    /// <summary>Nếu loại quà là Title, dùng trường này chứa Title Code.</summary>
    [BsonElement("title_code")]
    [BsonIgnoreIfNull]
    public string? TitleCode { get; set; }
}

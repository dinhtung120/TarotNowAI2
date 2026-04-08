

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document định nghĩa quest trong hệ gamification.
public class QuestDefinitionDocument
{
    // ObjectId của quest definition.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // Mã quest duy nhất để map với trigger event.
    [BsonElement("code")]
    public string Code { get; set; } = string.Empty;

    // Tiêu đề quest theo từng ngôn ngữ.
    [BsonElement("title_vi")] public string TitleVi { get; set; } = string.Empty;
    [BsonElement("title_en")] public string TitleEn { get; set; } = string.Empty;
    [BsonElement("title_zh")] public string TitleZh { get; set; } = string.Empty;

    // Mô tả quest theo từng ngôn ngữ.
    [BsonElement("description_vi")] public string DescriptionVi { get; set; } = string.Empty;
    [BsonElement("description_en")] public string DescriptionEn { get; set; } = string.Empty;
    [BsonElement("description_zh")] public string DescriptionZh { get; set; } = string.Empty;

    // Loại quest (daily/weekly/special...).
    [BsonElement("quest_type")]
    public string QuestType { get; set; } = string.Empty;

    // Sự kiện kích hoạt tăng tiến độ quest.
    [BsonElement("trigger_event")]
    public string TriggerEvent { get; set; } = string.Empty;

    // Mục tiêu cần đạt để hoàn thành quest.
    [BsonElement("target")]
    public int Target { get; set; }

    // Danh sách phần thưởng nhận được khi claim.
    [BsonElement("rewards")]
    public List<QuestRewardItem> Rewards { get; set; } = new();

    // Cờ bật/tắt quest ở mức cấu hình dữ liệu.
    [BsonElement("is_active")]
    public bool IsActive { get; set; } = true;

    // Thời điểm tạo quest definition.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Thời điểm cập nhật gần nhất.
    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

// Item phần thưởng bên trong quest definition.
public class QuestRewardItem
{
    // Loại reward (gold/diamond/title...).
    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    // Số lượng reward.
    [BsonElement("amount")]
    public int Amount { get; set; }

    // Mã title được thưởng khi reward type là title.
    [BsonElement("title_code")]
    [BsonIgnoreIfNull]
    public string? TitleCode { get; set; }
}

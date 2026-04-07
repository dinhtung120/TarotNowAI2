

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

public class QuestDefinitionDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("code")]
    public string Code { get; set; } = string.Empty;

    
    [BsonElement("title_vi")] public string TitleVi { get; set; } = string.Empty;
    [BsonElement("title_en")] public string TitleEn { get; set; } = string.Empty;
    [BsonElement("title_zh")] public string TitleZh { get; set; } = string.Empty;

    [BsonElement("description_vi")] public string DescriptionVi { get; set; } = string.Empty;
    [BsonElement("description_en")] public string DescriptionEn { get; set; } = string.Empty;
    [BsonElement("description_zh")] public string DescriptionZh { get; set; } = string.Empty;

        [BsonElement("quest_type")]
    public string QuestType { get; set; } = string.Empty;

        [BsonElement("trigger_event")]
    public string TriggerEvent { get; set; } = string.Empty;

        [BsonElement("target")]
    public int Target { get; set; }

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
        [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

        [BsonElement("amount")]
    public int Amount { get; set; }

        [BsonElement("title_code")]
    [BsonIgnoreIfNull]
    public string? TitleCode { get; set; }
}

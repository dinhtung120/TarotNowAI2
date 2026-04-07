

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

public class QuestProgressDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    [BsonElement("quest_code")]
    public string QuestCode { get; set; } = string.Empty;

        [BsonElement("period_key")]
    public string PeriodKey { get; set; } = string.Empty;

        [BsonElement("current_progress")]
    public int CurrentProgress { get; set; }

    [BsonElement("target")]
    public int Target { get; set; }

    [BsonElement("is_claimed")]
    public bool IsClaimed { get; set; } = false;

    [BsonElement("claimed_at")]
    [BsonIgnoreIfNull]
    public DateTime? ClaimedAt { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

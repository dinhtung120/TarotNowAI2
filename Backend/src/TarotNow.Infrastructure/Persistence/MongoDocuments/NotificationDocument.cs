

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

public class NotificationDocument
{
        [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

        [BsonElement("title")]
    public LocalizedText Title { get; set; } = new();

        [BsonElement("body")]
    public LocalizedText Body { get; set; } = new();

        [BsonElement("type")]
    public string Type { get; set; } = "system";

        [BsonElement("is_read")]
    public bool IsRead { get; set; } = false;

        [BsonElement("metadata")]
    [BsonIgnoreIfNull]
    public BsonDocument? Metadata { get; set; }

        [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class LocalizedText
{
    [BsonElement("vi")] public string Vi { get; set; } = string.Empty;
    [BsonElement("en")] public string En { get; set; } = string.Empty;
    [BsonElement("zh")] public string Zh { get; set; } = string.Empty;
}

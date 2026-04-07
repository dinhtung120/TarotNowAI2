

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

public class UserAchievementDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    [BsonElement("achievement_code")]
    public string AchievementCode { get; set; } = string.Empty;

    [BsonElement("unlocked_at")]
    public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;
}

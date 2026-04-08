

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document achievement đã unlock của người dùng.
public class UserAchievementDocument
{
    // ObjectId của bản ghi unlock.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // User sở hữu achievement.
    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    // Mã achievement đã unlock.
    [BsonElement("achievement_code")]
    public string AchievementCode { get; set; } = string.Empty;

    // Thời điểm unlock achievement.
    [BsonElement("unlocked_at")]
    public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;
}

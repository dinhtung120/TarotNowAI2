/*
 * FILE: UserAchievementDocument.cs
 * MỤC ĐÍCH: Lưu trữ thành tựu mà từng user đã đạt được.
 *   - Idempotency: Unique Index (UserId, AchievementCode) để đảm bảo 1 user chỉ đạt achievement 1 lần.
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// Ghi nhận 1 Achievement đã được mở khóa bởi 1 User. (Bảng user_achievements).
/// </summary>
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

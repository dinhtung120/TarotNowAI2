

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document điểm số leaderboard theo user và kỳ xếp hạng.
public class LeaderboardEntryDocument
{
    // ObjectId của bản ghi điểm.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // User sở hữu điểm số.
    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    // Track điểm (daily_streak, total_readings...).
    [BsonElement("score_track")]
    public string ScoreTrack { get; set; } = string.Empty;

    // Kỳ tính điểm (daily/weekly key).
    [BsonElement("period_key")]
    public string PeriodKey { get; set; } = string.Empty;

    // Điểm số hiện tại của user trong kỳ.
    [BsonElement("score")]
    public long Score { get; set; }

    // Mốc cập nhật điểm gần nhất.
    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

/*
 * FILE: LeaderboardEntryDocument.cs
 * MỤC ĐÍCH: Dữ liệu điểm số live của người chơi.
 *   - Các Query Top 100 sẽ quét qua bảng này. Job sẽ snapshot bảng này để chốt hạng.
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// Điểm số hiện tại của 1 user trên 1 track (daily/monthly/hi). (Bảng leaderboard_entries).
/// </summary>
public class LeaderboardEntryDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    /// <summary>daily, monthly, lifetime</summary>
    [BsonElement("score_track")]
    public string ScoreTrack { get; set; } = string.Empty;

    /// <summary>Ví dụ: "2026-04-06" (nếu daily), "2026-04" (nếu monthly), "all" (nếu lifetime)</summary>
    [BsonElement("period_key")]
    public string PeriodKey { get; set; } = string.Empty;

    [BsonElement("score")]
    public long Score { get; set; }

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

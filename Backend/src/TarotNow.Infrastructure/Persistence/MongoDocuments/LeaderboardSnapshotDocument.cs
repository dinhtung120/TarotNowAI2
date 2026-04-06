/*
 * FILE: LeaderboardSnapshotDocument.cs
 * MỤC ĐÍCH: Lưu trữ bảng xếp hạng đã chốt sổ của các kỳ trước.
 *   - Giúp UI lấy lịch sử BXH cũ rất nhanh mà không cần tính toán lại.
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// Kết quả xếp hạng chốt sổ (Snapshot) lưu lịch sử các kỳ. (Bảng leaderboard_snapshots).
/// </summary>
public class LeaderboardSnapshotDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("score_track")]
    public string ScoreTrack { get; set; } = string.Empty;

    [BsonElement("period_key")]
    public string PeriodKey { get; set; } = string.Empty;

    [BsonElement("total_participants")]
    public int TotalParticipants { get; set; }

    [BsonElement("entries")]
    public List<LeaderboardSnapshotEntry> Entries { get; set; } = new();

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class LeaderboardSnapshotEntry
{
    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    [BsonElement("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    [BsonElement("avatar")]
    public string? Avatar { get; set; }

    [BsonElement("active_title")]
    public string? ActiveTitle { get; set; }

    [BsonElement("score")]
    public long Score { get; set; }

    [BsonElement("rank")]
    public int Rank { get; set; }
}

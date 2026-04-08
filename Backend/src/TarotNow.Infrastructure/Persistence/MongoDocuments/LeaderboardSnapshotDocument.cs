

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document snapshot chốt bảng xếp hạng theo kỳ.
public class LeaderboardSnapshotDocument
{
    // ObjectId của snapshot.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // Track điểm của bảng xếp hạng.
    [BsonElement("score_track")]
    public string ScoreTrack { get; set; } = string.Empty;

    // Kỳ snapshot (period key).
    [BsonElement("period_key")]
    public string PeriodKey { get; set; } = string.Empty;

    // Tổng số user tham gia kỳ xếp hạng này.
    [BsonElement("total_participants")]
    public int TotalParticipants { get; set; }

    // Danh sách top entry đã chốt.
    [BsonElement("entries")]
    public List<LeaderboardSnapshotEntry> Entries { get; set; } = new();

    // Mốc thời gian tạo snapshot.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

// Entry hiển thị trong snapshot leaderboard.
public class LeaderboardSnapshotEntry
{
    // User của entry xếp hạng.
    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    // Tên hiển thị tại thời điểm chốt bảng.
    [BsonElement("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    // Avatar snapshot để không phụ thuộc profile realtime.
    [BsonElement("avatar")]
    public string? Avatar { get; set; }

    // Danh hiệu đang kích hoạt tại thời điểm snapshot.
    [BsonElement("active_title")]
    public string? ActiveTitle { get; set; }

    // Điểm số chốt.
    [BsonElement("score")]
    public long Score { get; set; }

    // Thứ hạng cuối cùng.
    [BsonElement("rank")]
    public int Rank { get; set; }
}

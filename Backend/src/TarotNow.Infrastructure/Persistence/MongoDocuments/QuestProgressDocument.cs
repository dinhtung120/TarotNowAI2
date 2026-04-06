/*
 * FILE: QuestProgressDocument.cs
 * MỤC ĐÍCH: Lưu tiến độ thực hiện nhiệm vụ của từng User trong MongoDB.
 *   - Idempotency bằng cách dùng Unique Index: (UserId, QuestCode, PeriodKey).
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// Tiến độ làm nhiệm vụ của 1 user trong 1 chu kỳ. (Bảng quest_progress).
/// </summary>
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

    /// <summary>Khóa chu kỳ để reset. VD: "2026-04-06" (Daily), "2026-W14" (Weekly).</summary>
    [BsonElement("period_key")]
    public string PeriodKey { get; set; } = string.Empty;

    /// <summary>Tiến độ đã đạt (≤ Target).</summary>
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

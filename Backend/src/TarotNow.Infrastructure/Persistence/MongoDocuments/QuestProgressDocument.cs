

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document tiến độ quest theo user và kỳ.
public class QuestProgressDocument
{
    // ObjectId của bản ghi tiến độ.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // User sở hữu tiến độ quest.
    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    // Mã quest tương ứng.
    [BsonElement("quest_code")]
    public string QuestCode { get; set; } = string.Empty;

    // Khóa kỳ (daily/weekly) để tách tiến độ theo giai đoạn.
    [BsonElement("period_key")]
    public string PeriodKey { get; set; } = string.Empty;

    // Tiến độ hiện tại đã tích lũy.
    [BsonElement("current_progress")]
    public int CurrentProgress { get; set; }

    // Mục tiêu cần đạt để hoàn thành.
    [BsonElement("target")]
    public int Target { get; set; }

    // Cờ đã nhận thưởng hay chưa.
    [BsonElement("is_claimed")]
    public bool IsClaimed { get; set; } = false;

    // Mốc thời gian claim thưởng.
    [BsonElement("claimed_at")]
    [BsonIgnoreIfNull]
    public DateTime? ClaimedAt { get; set; }

    // Thời điểm tạo tiến độ.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Thời điểm cập nhật tiến độ gần nhất.
    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

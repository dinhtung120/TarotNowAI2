using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document phiên gọi realtime trong hội thoại.
public class CallSessionDocument
{
    // ObjectId của phiên gọi.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    // Hội thoại sở hữu phiên gọi này.
    [BsonElement("conversation_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ConversationId { get; set; } = string.Empty;

    // User khởi tạo cuộc gọi.
    [BsonElement("initiator_id")]
    public string InitiatorId { get; set; } = string.Empty;

    // Loại cuộc gọi (audio/video).
    [BsonElement("type")]
    public string Type { get; set; } = "audio";

    // Trạng thái tiến trình cuộc gọi.
    [BsonElement("status")]
    public string Status { get; set; } = "requested";

    // Lý do kết thúc nếu cuộc gọi đã đóng.
    [BsonElement("end_reason")]
    public string? EndReason { get; set; }

    // Mốc bắt đầu cuộc gọi.
    [BsonElement("started_at")]
    public DateTime? StartedAt { get; set; }

    // Mốc kết thúc cuộc gọi.
    [BsonElement("ended_at")]
    public DateTime? EndedAt { get; set; }

    // Thời lượng đã gọi (giây), dùng cho billing/thống kê.
    [BsonElement("duration_seconds")]
    public int? DurationSeconds { get; set; }

    // Thời điểm tạo phiên gọi.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    // Thời điểm cập nhật trạng thái gần nhất.
    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; }
}

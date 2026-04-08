

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document log telemetry khi gọi AI provider.
public class AiProviderLogDocument
{
    // ObjectId của log record.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // User phát sinh request AI.
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    // Tham chiếu phiên đọc bài liên quan (nếu có).
    [BsonElement("reading_ref")]
    [BsonIgnoreIfNull]
    public string? ReadingRef { get; set; }

    // Tham chiếu request AI nội bộ để truy vết xuyên hệ thống.
    [BsonElement("ai_request_ref")]
    [BsonIgnoreIfNull]
    public string? AiRequestRef { get; set; }

    // Tên model AI thực tế được sử dụng.
    [BsonElement("model")]
    public string Model { get; set; } = string.Empty;

    // Thống kê token vào/ra phục vụ billing và giám sát chi phí.
    [BsonElement("tokens")]
    public TokenUsage Tokens { get; set; } = new();

    // Độ trễ xử lý tính bằng milliseconds.
    [BsonElement("latency_ms")]
    public int LatencyMs { get; set; }

    // Phiên bản prompt template áp dụng tại thời điểm gọi.
    [BsonElement("prompt_version")]
    [BsonIgnoreIfNull]
    public string? PromptVersion { get; set; }

    // Phiên bản policy/moderation đang áp dụng.
    [BsonElement("policy_version")]
    [BsonIgnoreIfNull]
    public string? PolicyVersion { get; set; }

    // Trạng thái vòng đời request (requested/succeeded/failed...).
    [BsonElement("status")]
    public string Status { get; set; } = "requested";

    // Mã lỗi chuẩn hóa khi request thất bại.
    [BsonElement("error_code")]
    [BsonIgnoreIfNull]
    public string? ErrorCode { get; set; }

    // Trace id để nối log giữa API, worker và provider.
    [BsonElement("trace_id")]
    [BsonIgnoreIfNull]
    public string? TraceId { get; set; }

    // Thời điểm ghi log.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

// Payload token usage lồng trong log AI.
public class TokenUsage
{
    // Số token đầu vào.
    [BsonElement("in")] public int InputTokens { get; set; }

    // Số token đầu ra.
    [BsonElement("out")] public int OutputTokens { get; set; }
}

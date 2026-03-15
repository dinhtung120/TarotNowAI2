using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// MongoDB Document cho collection "ai_provider_logs" — Log gọi AI provider.
///
/// Tại sao dùng MongoDB thay vì PostgreSQL cho logs?
/// → Volume cao (mỗi AI call = 1 log), schema linh hoạt (tokens, latency, trace_id),
///   và TTL 90 ngày tự động xóa bản ghi cũ — MongoDB xử lý tốt hơn cho use case này.
///
/// TTL index trên created_at: 7,776,000 giây = 90 ngày (xem init.js dòng 200).
/// </summary>
public class AiProviderLogDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>UUID user từ PostgreSQL.</summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>ObjectId tham chiếu reading_sessions._id.</summary>
    [BsonElement("reading_ref")]
    [BsonIgnoreIfNull]
    public string? ReadingRef { get; set; }

    /// <summary>
    /// UUID tham chiếu ai_requests.id (PostgreSQL).
    /// Cross-DB mapping trực tiếp — M6 fix.
    /// </summary>
    [BsonElement("ai_request_ref")]
    [BsonIgnoreIfNull]
    public string? AiRequestRef { get; set; }

    /// <summary>Model AI đã gọi (VD: "grok-3", "gpt-4o").</summary>
    [BsonElement("model")]
    public string Model { get; set; } = string.Empty;

    /// <summary>Số tokens input/output — dùng tính cost và monitor usage.</summary>
    [BsonElement("tokens")]
    public TokenUsage Tokens { get; set; } = new();

    /// <summary>Thời gian phản hồi tính bằng milliseconds.</summary>
    [BsonElement("latency_ms")]
    public int LatencyMs { get; set; }

    /// <summary>Phiên bản prompt template đang dùng.</summary>
    [BsonElement("prompt_version")]
    [BsonIgnoreIfNull]
    public string? PromptVersion { get; set; }

    /// <summary>Phiên bản content policy.</summary>
    [BsonElement("policy_version")]
    [BsonIgnoreIfNull]
    public string? PolicyVersion { get; set; }

    /// <summary>Trạng thái cuối: requested / completed / failed.</summary>
    [BsonElement("status")]
    public string Status { get; set; } = "requested";

    /// <summary>Mã lỗi nếu failed — dùng cho alert/debug.</summary>
    [BsonElement("error_code")]
    [BsonIgnoreIfNull]
    public string? ErrorCode { get; set; }

    /// <summary>OpenTelemetry trace ID — correlation giữa các service.</summary>
    [BsonElement("trace_id")]
    [BsonIgnoreIfNull]
    public string? TraceId { get; set; }

    /// <summary>
    /// Thời điểm tạo log — TTL index dựa trên field này (90 ngày auto-delete).
    /// </summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>Token usage — phân tích chi phí AI call.</summary>
public class TokenUsage
{
    [BsonElement("in")] public int InputTokens { get; set; }
    [BsonElement("out")] public int OutputTokens { get; set; }
}

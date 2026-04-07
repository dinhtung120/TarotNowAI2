

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

public class AiProviderLogDocument
{
        [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

        [BsonElement("reading_ref")]
    [BsonIgnoreIfNull]
    public string? ReadingRef { get; set; }

        [BsonElement("ai_request_ref")]
    [BsonIgnoreIfNull]
    public string? AiRequestRef { get; set; }

        [BsonElement("model")]
    public string Model { get; set; } = string.Empty;

        [BsonElement("tokens")]
    public TokenUsage Tokens { get; set; } = new();

        [BsonElement("latency_ms")]
    public int LatencyMs { get; set; }

        [BsonElement("prompt_version")]
    [BsonIgnoreIfNull]
    public string? PromptVersion { get; set; }

        [BsonElement("policy_version")]
    [BsonIgnoreIfNull]
    public string? PolicyVersion { get; set; }

        [BsonElement("status")]
    public string Status { get; set; } = "requested";

        [BsonElement("error_code")]
    [BsonIgnoreIfNull]
    public string? ErrorCode { get; set; }

        [BsonElement("trace_id")]
    [BsonIgnoreIfNull]
    public string? TraceId { get; set; }

        [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class TokenUsage
{
        [BsonElement("in")] public int InputTokens { get; set; }

        [BsonElement("out")] public int OutputTokens { get; set; }
}

using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

[BsonIgnoreExtraElements]
// Document phiên upload tạm để validate uploadToken one-time.
public sealed class UploadSessionDocument
{
    [BsonId]
    [BsonElement("upload_token")]
    public string UploadToken { get; set; } = string.Empty;

    [BsonElement("owner_user_id")]
    public string OwnerUserId { get; set; } = string.Empty;

    [BsonElement("scope")]
    public string Scope { get; set; } = string.Empty;

    [BsonElement("object_key")]
    public string ObjectKey { get; set; } = string.Empty;

    [BsonElement("public_url")]
    public string PublicUrl { get; set; } = string.Empty;

    [BsonElement("content_type")]
    public string ContentType { get; set; } = string.Empty;

    [BsonElement("size_bytes")]
    public long SizeBytes { get; set; }

    [BsonElement("conversation_id")]
    [BsonIgnoreIfNull]
    public string? ConversationId { get; set; }

    [BsonElement("context_type")]
    [BsonIgnoreIfNull]
    public string? ContextType { get; set; }

    [BsonElement("context_draft_id")]
    [BsonIgnoreIfNull]
    public string? ContextDraftId { get; set; }

    [BsonElement("created_at_utc")]
    public DateTime CreatedAtUtc { get; set; }

    [BsonElement("expires_at_utc")]
    public DateTime ExpiresAtUtc { get; set; }

    [BsonElement("consumed_at_utc")]
    [BsonIgnoreIfNull]
    public DateTime? ConsumedAtUtc { get; set; }

    [BsonElement("cleaned_up_at_utc")]
    [BsonIgnoreIfNull]
    public DateTime? CleanedUpAtUtc { get; set; }
}

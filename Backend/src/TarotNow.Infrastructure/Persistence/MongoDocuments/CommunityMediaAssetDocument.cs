using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

[BsonIgnoreExtraElements]
// Document asset ảnh community để theo dõi vòng đời upload/attach/cleanup.
public sealed class CommunityMediaAssetDocument
{
    [BsonId]
    [BsonElement("object_key")]
    public string ObjectKey { get; set; } = string.Empty;

    [BsonElement("public_url")]
    public string PublicUrl { get; set; } = string.Empty;

    [BsonElement("owner_user_id")]
    public string OwnerUserId { get; set; } = string.Empty;

    [BsonElement("context_type")]
    public string ContextType { get; set; } = string.Empty;

    [BsonElement("context_draft_id")]
    [BsonIgnoreIfNull]
    public string? ContextDraftId { get; set; }

    [BsonElement("context_entity_id")]
    [BsonIgnoreIfNull]
    public string? ContextEntityId { get; set; }

    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;

    [BsonElement("created_at_utc")]
    public DateTime CreatedAtUtc { get; set; }

    [BsonElement("updated_at_utc")]
    public DateTime UpdatedAtUtc { get; set; }

    [BsonElement("attached_at_utc")]
    [BsonIgnoreIfNull]
    public DateTime? AttachedAtUtc { get; set; }

    [BsonElement("orphaned_at_utc")]
    [BsonIgnoreIfNull]
    public DateTime? OrphanedAtUtc { get; set; }

    [BsonElement("deleted_at_utc")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAtUtc { get; set; }

    [BsonElement("expires_at_utc")]
    public DateTime ExpiresAtUtc { get; set; }
}

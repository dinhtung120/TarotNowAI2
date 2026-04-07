

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

[BsonIgnoreExtraElements]
public class ConversationDocument
{
        [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

        [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

        [BsonElement("reader_id")]
    public string ReaderId { get; set; } = string.Empty;

        [BsonElement("status")]
    public string Status { get; set; } = string.Empty;

        [BsonElement("confirm")]
    public ConversationConfirm? Confirm { get; set; }

        [BsonElement("last_message_at")]
    public DateTime? LastMessageAt { get; set; }

        [BsonElement("offer_expires_at")]
    public DateTime? OfferExpiresAt { get; set; }

        [BsonElement("sla_hours")]
    public int SlaHours { get; set; } = 12;

        [BsonElement("unread_count")]
    public UnreadCount UnreadCount { get; set; } = new();

        [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

        [BsonElement("deleted_at")]
    public DateTime? DeletedAt { get; set; }

        [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

[BsonIgnoreExtraElements]
public class ConversationConfirm
{
        [BsonElement("user_at")]
    public DateTime? UserAt { get; set; }

        [BsonElement("reader_at")]
    public DateTime? ReaderAt { get; set; }

    [BsonElement("requested_by")]
    public string? RequestedBy { get; set; }

    [BsonElement("requested_at")]
    public DateTime? RequestedAt { get; set; }

    [BsonElement("auto_resolve_at")]
    public DateTime? AutoResolveAt { get; set; }
}

[BsonIgnoreExtraElements]
public class UnreadCount
{
        [BsonElement("user")]
    public int User { get; set; }

        [BsonElement("reader")]
    public int Reader { get; set; }
}



using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

[BsonIgnoreExtraElements]
public partial class ChatMessageDocument
{
        [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

        [BsonElement("conversation_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ConversationId { get; set; } = string.Empty;

        [BsonElement("sender_id")]
    public string SenderId { get; set; } = string.Empty;

        [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

        [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

        [BsonElement("payment_payload")]
    [BsonIgnoreIfNull]
    public ChatPaymentPayload? PaymentPayload { get; set; }

        [BsonElement("is_read")]
    public bool IsRead { get; set; }

        [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

        [BsonElement("deleted_at")]
    public DateTime? DeletedAt { get; set; }

        [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
    public DateTime? UpdatedAt { get; set; }

        [BsonElement("is_flagged")]
    public bool IsFlagged { get; set; }

        [BsonElement("call_payload")]
    [BsonIgnoreIfNull]
    public ChatCallPayload? CallPayload { get; set; }
}

[BsonIgnoreExtraElements]
public class ChatPaymentPayload
{
        [BsonElement("amount_diamond")]
    public long AmountDiamond { get; set; }

        [BsonElement("proposal_id")]
    public string? ProposalId { get; set; }

        [BsonElement("expires_at")]
    public DateTime? ExpiresAt { get; set; }
}

[BsonIgnoreExtraElements]
public class ChatCallPayload
{
    [BsonElement("session_id")]
    public string SessionId { get; set; } = string.Empty;

    [BsonElement("call_type")]
    public string CallType { get; set; } = string.Empty;

    [BsonElement("end_reason")]
    public string? EndReason { get; set; }

    [BsonElement("duration_seconds")]
    public int DurationSeconds { get; set; }
}

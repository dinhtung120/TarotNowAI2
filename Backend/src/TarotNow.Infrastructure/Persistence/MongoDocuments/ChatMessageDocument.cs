using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// MongoDB document cho collection chat_messages (schema §8).
///
/// Tin nhắn trong conversation — hỗ trợ text, system, payment events.
/// payment_payload chỉ có khi type = payment_offer.
/// </summary>
public class ChatMessageDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>ObjectId conversations._id.</summary>
    [BsonElement("conversation_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>UUID người gửi.</summary>
    [BsonElement("sender_id")]
    public string SenderId { get; set; } = string.Empty;

    /// <summary>Loại: text | system | card_share | payment_*</summary>
    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>Nội dung tin nhắn.</summary>
    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

    /// <summary>Payload cho payment_offer — optional.</summary>
    [BsonElement("payment_payload")]
    [BsonIgnoreIfNull]
    public ChatPaymentPayload? PaymentPayload { get; set; }

    /// <summary>Đã đọc hay chưa.</summary>
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
}

/// <summary>
/// Payload chi tiết cho payment_offer messages.
/// </summary>
public class ChatPaymentPayload
{
    [BsonElement("amount_diamond")]
    public long AmountDiamond { get; set; }

    [BsonElement("proposal_id")]
    public string? ProposalId { get; set; }

    [BsonElement("expires_at")]
    public DateTime? ExpiresAt { get; set; }
}

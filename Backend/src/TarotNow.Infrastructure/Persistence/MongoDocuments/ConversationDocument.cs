using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// MongoDB document cho collection conversations (schema §7).
///
/// Mỗi conversation = 1 chat 1-1 giữa user và reader.
/// unread_count là denormalized counter — reset khi user đọc tin.
/// </summary>
public class ConversationDocument
{
    /// <summary>MongoDB ObjectId — Auto-generated.</summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>UUID user (payer) — string để phù hợp PostgreSQL UUID.</summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>UUID reader (receiver).</summary>
    [BsonElement("reader_id")]
    public string ReaderId { get; set; } = string.Empty;

    /// <summary>Trạng thái: pending | active | completed | cancelled | disputed.</summary>
    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Xác nhận release — convenience UI field.
    /// Source of truth cho settlement là chat_question_items (PostgreSQL).
    /// </summary>
    [BsonElement("confirm")]
    public ConversationConfirm? Confirm { get; set; }

    /// <summary>Thời điểm tin nhắn cuối — sort inbox.</summary>
    [BsonElement("last_message_at")]
    public DateTime? LastMessageAt { get; set; }

    /// <summary>Thời hạn chờ Reader phản hồi trước khi bị hủy tự động.</summary>
    [BsonElement("offer_expires_at")]
    public DateTime? OfferExpiresAt { get; set; }

    /// <summary>Denormalized counter tin chưa đọc.</summary>
    [BsonElement("unread_count")]
    public UnreadCount UnreadCount { get; set; } = new();

    /// <summary>Soft delete flag.</summary>
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>Xác nhận release từ 2 bên.</summary>
public class ConversationConfirm
{
    [BsonElement("user_at")]
    public DateTime? UserAt { get; set; }

    [BsonElement("reader_at")]
    public DateTime? ReaderAt { get; set; }
}

/// <summary>Denormalized unread counter.</summary>
public class UnreadCount
{
    [BsonElement("user")]
    public int User { get; set; }

    [BsonElement("reader")]
    public int Reader { get; set; }
}

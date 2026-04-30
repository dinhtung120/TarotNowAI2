

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

[BsonIgnoreExtraElements]
// Document tin nhắn trong hội thoại chat.
public partial class ChatMessageDocument
{
    // ObjectId của message.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    // Hội thoại chứa tin nhắn.
    [BsonElement("conversation_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ConversationId { get; set; } = string.Empty;

    // User gửi tin nhắn.
    [BsonElement("sender_id")]
    public string SenderId { get; set; } = string.Empty;

    // Loại tin nhắn (text/media/payment...).
    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    // Nội dung chính của message.
    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

    // Định danh message từ client để idempotency khi retry gửi.
    [BsonElement("client_message_id")]
    [BsonIgnoreIfNull]
    public string? ClientMessageId { get; set; }

    // Khóa idempotency cho system message phát sinh từ background/outbox.
    [BsonElement("system_event_key")]
    [BsonIgnoreIfNull]
    public string? SystemEventKey { get; set; }

    // Payload nghiệp vụ thanh toán khi message là payment offer.
    [BsonElement("payment_payload")]
    [BsonIgnoreIfNull]
    public ChatPaymentPayload? PaymentPayload { get; set; }

    // Cờ đánh dấu đã đọc.
    [BsonElement("is_read")]
    public bool IsRead { get; set; }

    // Cờ soft-delete để ẩn message mà vẫn giữ audit.
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    // Mốc thời gian soft-delete.
    [BsonElement("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    // Thời điểm tạo message.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    // Thời điểm chỉnh sửa/cập nhật message.
    [BsonElement("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Cờ moderation để đánh dấu nội dung cần xem xét.
    [BsonElement("is_flagged")]
    public bool IsFlagged { get; set; }

}

[BsonIgnoreExtraElements]
// Payload đề xuất thanh toán trong chat.
public class ChatPaymentPayload
{
    // Số diamond đề xuất thanh toán.
    [BsonElement("amount_diamond")]
    public long AmountDiamond { get; set; }

    // Mã proposal để idempotency khi user chấp nhận/từ chối.
    [BsonElement("proposal_id")]
    public string? ProposalId { get; set; }

    // Mốc hết hạn đề xuất thanh toán.
    [BsonElement("expires_at")]
    public DateTime? ExpiresAt { get; set; }
}



using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

[BsonIgnoreExtraElements]
// Document hội thoại giữa user và reader.
public class ConversationDocument
{
    // ObjectId của hội thoại.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    // User tạo yêu cầu tư vấn.
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    // Reader nhận xử lý hội thoại.
    [BsonElement("reader_id")]
    public string ReaderId { get; set; } = string.Empty;

    // Trạng thái vòng đời hội thoại.
    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;

    // Thông tin xác nhận hoàn tất từ hai phía.
    [BsonElement("confirm")]
    public ConversationConfirm? Confirm { get; set; }

    // Thời điểm có tin nhắn mới nhất.
    [BsonElement("last_message_at")]
    public DateTime? LastMessageAt { get; set; }

    // Mốc hết hạn đề nghị add-money (nếu có).
    [BsonElement("offer_expires_at")]
    public DateTime? OfferExpiresAt { get; set; }

    // SLA giờ xử lý dùng cho timeout nghiệp vụ.
    [BsonElement("sla_hours")]
    public int SlaHours { get; set; } = 12;

    // Bộ đếm unread tách theo vai trò user/reader.
    [BsonElement("unread_count")]
    public UnreadCount UnreadCount { get; set; } = new();

    // Soft-delete flag của hội thoại.
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    // Mốc thời gian xóa mềm.
    [BsonElement("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    // Thời điểm tạo hội thoại.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    // Thời điểm cập nhật trạng thái gần nhất.
    [BsonElement("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

[BsonIgnoreExtraElements]
// Trạng thái xác nhận hoàn tất từ user và reader.
public class ConversationConfirm
{
    // Thời điểm user xác nhận.
    [BsonElement("user_at")]
    public DateTime? UserAt { get; set; }

    // Thời điểm reader xác nhận.
    [BsonElement("reader_at")]
    public DateTime? ReaderAt { get; set; }

    // Ai là bên tạo yêu cầu hoàn tất.
    [BsonElement("requested_by")]
    public string? RequestedBy { get; set; }

    // Thời điểm tạo yêu cầu hoàn tất.
    [BsonElement("requested_at")]
    public DateTime? RequestedAt { get; set; }

    // Mốc tự động resolve nếu đối tác không phản hồi.
    [BsonElement("auto_resolve_at")]
    public DateTime? AutoResolveAt { get; set; }
}

[BsonIgnoreExtraElements]
// Bộ đếm tin chưa đọc cho từng vai trò trong hội thoại.
public class UnreadCount
{
    // Số tin chưa đọc phía user.
    [BsonElement("user")]
    public int User { get; set; }

    // Số tin chưa đọc phía reader.
    [BsonElement("reader")]
    public int Reader { get; set; }
}

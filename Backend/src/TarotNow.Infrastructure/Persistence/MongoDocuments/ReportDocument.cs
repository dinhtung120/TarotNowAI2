

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document báo cáo vi phạm trong chat/community.
public class ReportDocument
{
    // ObjectId của report.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    // User tạo report.
    [BsonElement("reporter_id")]
    public string ReporterId { get; set; } = string.Empty;

    // Đối tượng bị report (post/comment/message...).
    [BsonElement("target")]
    public ReportTarget Target { get; set; } = new();

    // Tham chiếu hội thoại liên quan nếu report phát sinh từ chat.
    [BsonElement("conversation_ref")]
    [BsonIgnoreIfNull]
    public string? ConversationRef { get; set; }

    // Lý do report do người dùng cung cấp.
    [BsonElement("reason")]
    public string Reason { get; set; } = string.Empty;

    // Trạng thái xử lý report.
    [BsonElement("status")]
    public string Status { get; set; } = "pending";

    // Kết quả xử lý cuối cùng (nếu đã resolve).
    [BsonElement("result")]
    [BsonIgnoreIfNull]
    public string? Result { get; set; }

    // Ghi chú nội bộ của admin khi xử lý.
    [BsonElement("admin_note")]
    [BsonIgnoreIfNull]
    public string? AdminNote { get; set; }

    // Admin đã chốt xử lý report.
    [BsonElement("resolved_by")]
    [BsonIgnoreIfNull]
    public string? ResolvedBy { get; set; }

    // Mốc thời gian resolve.
    [BsonElement("resolved_at")]
    public DateTime? ResolvedAt { get; set; }

    // Soft-delete flag.
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    // Mốc xóa mềm.
    [BsonElement("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    // Thời điểm tạo report.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    // Thời điểm cập nhật report gần nhất.
    [BsonElement("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

// Đối tượng bị report.
public class ReportTarget
{
    // Loại target (post/comment/message...).
    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    // Id của target tương ứng.
    [BsonElement("id")]
    public string Id { get; set; } = string.Empty;
}

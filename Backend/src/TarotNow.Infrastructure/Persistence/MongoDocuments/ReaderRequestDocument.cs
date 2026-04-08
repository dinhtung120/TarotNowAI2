

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document yêu cầu đăng ký reader của người dùng.
public class ReaderRequestDocument
{
    // ObjectId của yêu cầu.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // User gửi yêu cầu.
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    // Trạng thái duyệt yêu cầu (pending/approved/rejected).
    [BsonElement("status")]
    public string Status { get; set; } = "pending";

    // Phần giới thiệu bản thân của ứng viên.
    [BsonElement("intro_text")]
    public string IntroText { get; set; } = string.Empty;

    // Danh sách tài liệu minh chứng.
    [BsonElement("proof_documents")]
    public List<string> ProofDocuments { get; set; } = new();

    // Ghi chú xử lý từ admin (nếu có).
    [BsonElement("admin_note")]
    [BsonIgnoreIfNull]
    public string? AdminNote { get; set; }

    // Admin đã duyệt yêu cầu.
    [BsonElement("reviewed_by")]
    [BsonIgnoreIfNull]
    public string? ReviewedBy { get; set; }

    // Mốc thời gian duyệt.
    [BsonElement("reviewed_at")]
    [BsonIgnoreIfNull]
    public DateTime? ReviewedAt { get; set; }

    // Soft-delete flag.
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    // Mốc thời gian xóa mềm.
    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    // Thời điểm tạo yêu cầu.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Thời điểm cập nhật gần nhất.
    [BsonElement("updated_at")]
    [BsonIgnoreIfNull]
    public DateTime? UpdatedAt { get; set; }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// MongoDB Document cho collection "reader_requests" — Đơn xin trở thành Reader.
///
/// Luồng nghiệp vụ:
/// 1. User gửi đơn → tạo document status = "pending"
/// 2. Admin duyệt → status = "approved", hệ thống tạo reader_profiles + cập nhật user role
/// 3. Admin từ chối → status = "rejected", admin_note giải thích lý do
///
/// Tại sao lưu trong MongoDB thay vì PostgreSQL?
/// → Dữ liệu bán cấu trúc (proof_documents là mảng linh hoạt).
/// → Không có yêu cầu ACID transaction với bảng khác.
/// → Phù hợp với schema.md collection #6 đã thiết kế.
///
/// Tham chiếu: schema.md ## 6. reader_requests
/// </summary>
public class ReaderRequestDocument
{
    /// <summary>MongoDB ObjectId — primary key tự động.</summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>UUID user gửi đơn — tham chiếu users.id (PostgreSQL).</summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái đơn: pending | approved | rejected.
    /// Mapping với ReaderApprovalStatus enum trong Domain.
    /// </summary>
    [BsonElement("status")]
    public string Status { get; set; } = "pending";

    /// <summary>
    /// Lời giới thiệu bản thân của user — tại sao muốn trở thành Reader.
    /// Bắt buộc khi submit, admin sẽ đọc để đánh giá.
    /// </summary>
    [BsonElement("intro_text")]
    public string IntroText { get; set; } = string.Empty;

    /// <summary>
    /// Danh sách tham chiếu tài liệu chứng minh (URL hoặc file path).
    /// Có thể trống nếu user không cung cấp bằng chứng.
    /// </summary>
    [BsonElement("proof_documents")]
    public List<string> ProofDocuments { get; set; } = new();

    /// <summary>Ghi chú của admin khi duyệt/từ chối — giải thích quyết định.</summary>
    [BsonElement("admin_note")]
    [BsonIgnoreIfNull]
    public string? AdminNote { get; set; }

    /// <summary>UUID admin đã duyệt/từ chối đơn.</summary>
    [BsonElement("reviewed_by")]
    [BsonIgnoreIfNull]
    public string? ReviewedBy { get; set; }

    /// <summary>Thời điểm admin duyệt/từ chối.</summary>
    [BsonElement("reviewed_at")]
    [BsonIgnoreIfNull]
    public DateTime? ReviewedAt { get; set; }

    /// <summary>Soft delete flag — theo chính sách lưu trữ chung.</summary>
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    /// <summary>Thời điểm soft delete.</summary>
    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    /// <summary>Thời điểm tạo đơn.</summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Thời điểm cập nhật cuối.</summary>
    [BsonElement("updated_at")]
    [BsonIgnoreIfNull]
    public DateTime? UpdatedAt { get; set; }
}

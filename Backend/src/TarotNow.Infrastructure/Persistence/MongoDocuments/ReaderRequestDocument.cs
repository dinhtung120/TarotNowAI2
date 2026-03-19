/*
 * FILE: ReaderRequestDocument.cs
 * MỤC ĐÍCH: Schema cho collection "reader_requests" (MongoDB).
 *   Đơn xin trở thành Reader (thầy bói).
 *
 *   LUỒNG NGHIỆP VỤ:
 *   1. User gửi đơn → tạo document status = "pending"
 *   2. Admin duyệt → status = "approved" → hệ thống tạo reader_profiles + cập nhật user.role
 *   3. Admin từ chối → status = "rejected" → admin_note giải thích lý do
 *
 *   TẠI SAO MONGODB?
 *   → Dữ liệu bán cấu trúc (proof_documents là mảng URL linh hoạt).
 *   → Không cần ACID transaction với bảng khác (chỉ đọc/ghi đơn lẻ).
 *
 *   Tham chiếu: schema.md §6 (reader_requests)
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// 1 đơn xin làm Reader trong collection "reader_requests".
/// </summary>
public class ReaderRequestDocument
{
    /// <summary>ObjectId tự sinh — khóa chính.</summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>UUID user gửi đơn — tham chiếu users.id (PostgreSQL).</summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái đơn: "pending" (chờ duyệt) | "approved" (đã duyệt) | "rejected" (từ chối).
    /// Mapping với ReaderApprovalStatus enum trong Domain layer.
    /// </summary>
    [BsonElement("status")]
    public string Status { get; set; } = "pending";

    /// <summary>
    /// Lời giới thiệu bản thân — User viết tại sao muốn trở thành Reader.
    /// Bắt buộc khi submit đơn. Admin sẽ đọc để đánh giá năng lực.
    /// </summary>
    [BsonElement("intro_text")]
    public string IntroText { get; set; } = string.Empty;

    /// <summary>
    /// Danh sách URL/đường dẫn tài liệu chứng minh (chứng chỉ, ảnh, v.v.).
    /// Có thể trống nếu User không cung cấp bằng chứng.
    /// Dùng List vì số lượng file không cố định.
    /// </summary>
    [BsonElement("proof_documents")]
    public List<string> ProofDocuments { get; set; } = new();

    /// <summary>Ghi chú của Admin khi duyệt/từ chối — giải thích quyết định.</summary>
    [BsonElement("admin_note")]
    [BsonIgnoreIfNull]
    public string? AdminNote { get; set; }

    /// <summary>UUID Admin đã duyệt/từ chối đơn.</summary>
    [BsonElement("reviewed_by")]
    [BsonIgnoreIfNull]
    public string? ReviewedBy { get; set; }

    /// <summary>Thời điểm Admin duyệt/từ chối.</summary>
    [BsonElement("reviewed_at")]
    [BsonIgnoreIfNull]
    public DateTime? ReviewedAt { get; set; }

    /// <summary>Soft delete flag.</summary>
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    [BsonIgnoreIfNull]
    public DateTime? UpdatedAt { get; set; }
}

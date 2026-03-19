/*
 * FILE: ReportDocument.cs
 * MỤC ĐÍCH: Schema cho collection "reports" (MongoDB).
 *   Báo cáo vi phạm từ User — Admin xử lý qua hàng đợi (queue).
 *
 *   LUỒNG NGHIỆP VỤ:
 *   1. User thấy nội dung vi phạm → bấm nút "Báo cáo"
 *   2. Document tạo với status = "pending"
 *   3. Admin xem queue → xử lý (status = "processing")
 *   4. Admin quyết định: "resolved" (xử lý xong) hoặc "rejected" (không vi phạm)
 *   5. Nếu vi phạm → result: "warn" (cảnh cáo), "freeze" (đóng băng), "refund" (hoàn tiền), "no_action"
 *
 *   Tham chiếu: schema.md §10 (reports)
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// 1 báo cáo vi phạm trong collection "reports".
/// </summary>
public class ReportDocument
{
    /// <summary>ObjectId tự sinh.</summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>UUID người báo cáo (reporter).</summary>
    [BsonElement("reporter_id")]
    public string ReporterId { get; set; } = string.Empty;

    /// <summary>
    /// Đối tượng bị báo cáo — chứa type (loại) và id (mã).
    /// Type: "message" (tin nhắn), "conversation" (cuộc hội thoại), "user" (người dùng).
    /// </summary>
    [BsonElement("target")]
    public ReportTarget Target { get; set; } = new();

    /// <summary>ObjectId conversation liên quan (nếu báo cáo liên quan tới chat).</summary>
    [BsonElement("conversation_ref")]
    [BsonIgnoreIfNull]
    public string? ConversationRef { get; set; }

    /// <summary>Lý do báo cáo do User nhập (ví dụ: "Spam", "Lừa đảo", "Nội dung không phù hợp").</summary>
    [BsonElement("reason")]
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái xử lý: "pending" → "processing" → "resolved" | "rejected".
    /// Index (status, created_at desc) giúp Admin xem queue nhanh.
    /// </summary>
    [BsonElement("status")]
    public string Status { get; set; } = "pending";

    /// <summary>
    /// Kết quả xử lý (chỉ có khi status = "resolved"):
    ///   "warn": cảnh cáo đối tượng
    ///   "freeze": đóng băng tài khoản
    ///   "refund": hoàn tiền cho người bị hại
    ///   "no_action": không có hành động (báo cáo sai)
    /// </summary>
    [BsonElement("result")]
    [BsonIgnoreIfNull]
    public string? Result { get; set; }

    /// <summary>Ghi chú nội bộ của Admin (không hiển thị cho User).</summary>
    [BsonElement("admin_note")]
    [BsonIgnoreIfNull]
    public string? AdminNote { get; set; }

    /// <summary>UUID Admin xử lý báo cáo.</summary>
    [BsonElement("resolved_by")]
    [BsonIgnoreIfNull]
    public string? ResolvedBy { get; set; }

    /// <summary>Thời điểm xử lý xong.</summary>
    [BsonElement("resolved_at")]
    public DateTime? ResolvedAt { get; set; }

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

/// <summary>
/// Đối tượng bị báo cáo: loại (message/conversation/user) + ID.
/// Tách class riêng vì trong MongoDB đây là nested object: { type: "...", id: "..." }
/// </summary>
public class ReportTarget
{
    /// <summary>Loại: "message" | "conversation" | "user".</summary>
    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>ID tương ứng (ObjectId hoặc UUID tùy loại).</summary>
    [BsonElement("id")]
    public string Id { get; set; } = string.Empty;
}

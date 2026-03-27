/*
 * ===================================================================
 * FILE: ConversationDocument.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.MongoDocuments
 * ===================================================================
 * MỤC ĐÍCH:
 *   Định nghĩa cấu trúc cho Collection "conversations" trên MongoDB.
 *   Mỗi document = 1 cuộc hội thoại (chat 1-1) giữa User (khách hỏi bài) và Reader (thầy bói).
 *
 *   VÒNG ĐỜI CỦA MỘT CONVERSATION:
 *   1. "pending": User gửi yêu cầu chat → Reader chưa phản hồi
 *   2. "active": Reader chấp nhận → 2 bên có thể nhắn tin
 *   3. "completed": Cả 2 bên xác nhận hoàn thành → tiền từ Escrow chuyển cho Reader
 *   4. "cancelled": Bị hủy (hết hạn chờ, User hủy, hoặc hệ thống tự hủy)
 *   5. "disputed": Có tranh chấp → Admin xử lý
 *
 *   LIÊN KẾT VỚI CÁC HỆ THỐNG KHÁC:
 *   → chat_messages (MongoDB): tin nhắn thuộc conversation này
 *   → chat_finance_sessions (PostgreSQL): phiên tài chính (escrow) tương ứng
 *   → chat_question_items (PostgreSQL): từng câu hỏi có trả phí trong conversation
 *
 *   Tham chiếu: schema.md §7 (conversations)
 * ===================================================================
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// Đại diện cho 1 cuộc hội thoại chat 1-1 giữa User và Reader trong collection "conversations".
/// </summary>
public class ConversationDocument
{
    /// <summary>
    /// MongoDB ObjectId — ID duy nhất của cuộc hội thoại, tự động sinh bởi MongoDB.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// UUID của User (người hỏi bài / người trả tiền).
    /// Lưu dạng string vì UUID từ PostgreSQL không phải ObjectId của MongoDB.
    /// </summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// UUID của Reader (thầy bói / người nhận tiền).
    /// Cũng lưu dạng string UUID từ PostgreSQL.
    /// </summary>
    [BsonElement("reader_id")]
    public string ReaderId { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái hiện tại của cuộc hội thoại.
    /// Các giá trị hợp lệ: "pending" | "active" | "completed" | "cancelled" | "disputed".
    /// Trạng thái này quyết định:
    ///   - User/Reader có thể gửi tin hay không
    ///   - Nút nào hiển thị trên UI (Accept, Cancel, Confirm Complete, v.v.)
    ///   - Tiền trong Escrow được xử lý thế nào
    /// </summary>
    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Thông tin xác nhận "hoàn thành" từ 2 phía (User và Reader).
    /// Khi CẢ HAI bên đều confirm → status chuyển thành "completed" → tiền Escrow chuyển về Reader.
    /// Null nếu chưa ai confirm. Xem class con ConversationConfirm bên dưới.
    /// 
    /// LƯU Ý: Đây chỉ là trường tiện lợi cho UI. Nguồn dữ liệu chính xác (source of truth) 
    /// cho việc thanh toán nằm ở bảng chat_question_items bên PostgreSQL.
    /// </summary>
    [BsonElement("confirm")]
    public ConversationConfirm? Confirm { get; set; }

    /// <summary>
    /// Thời điểm tin nhắn cuối cùng được gửi trong cuộc hội thoại.
    /// Dùng để sắp xếp danh sách hội thoại (inbox) — conversation mới nhất hiện lên trên cùng.
    /// </summary>
    [BsonElement("last_message_at")]
    public DateTime? LastMessageAt { get; set; }

    /// <summary>
    /// Thời hạn để Reader phản hồi (chấp nhận/từ chối) yêu cầu chat.
    /// Nếu quá thời hạn này mà Reader chưa phản hồi → EscrowTimerService sẽ tự động hủy.
    /// Tiền bị đóng băng (frozen) trong Escrow sẽ được hoàn trả về ví User.
    /// </summary>
    [BsonElement("offer_expires_at")]
    public DateTime? OfferExpiresAt { get; set; }

    /// <summary>
    /// Bộ đếm tin nhắn chưa đọc (denormalized counter).
    /// "Denormalized" nghĩa là: thông tin này THỪA (có thể tính lại từ chat_messages),
    /// nhưng lưu sẵn ở đây để UI lấy NHANH (không cần đếm lại mỗi lần mở inbox).
    /// Được reset về 0 khi người dùng mở cuộc hội thoại và đọc hết tin.
    /// </summary>
    [BsonElement("unread_count")]
    public UnreadCount UnreadCount { get; set; } = new();

    /// <summary>
    /// Cờ xóa mềm (soft delete). True = cuộc hội thoại đã bị xóa nhưng vẫn còn trong DB.
    /// Giữ lại để Admin có thể review khi có tranh chấp hoặc báo cáo vi phạm.
    /// </summary>
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    /// <summary>Thời điểm cuộc hội thoại bị xóa mềm. Null nếu chưa xóa.</summary>
    [BsonElement("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    /// <summary>Thời điểm cuộc hội thoại được tạo (User gửi yêu cầu chat đầu tiên).</summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>Thời điểm cập nhật cuối cùng (thay đổi status, gửi tin mới, v.v.).</summary>
    [BsonElement("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Ghi nhận thời điểm xác nhận "hoàn thành" từ mỗi bên trong cuộc hội thoại.
/// Khi cả UserAt VÀ ReaderAt đều có giá trị (không null) → cuộc hội thoại coi như "completed".
/// Cơ chế xác nhận 2 bên giúp tránh tranh chấp: không bên nào tự ý kết thúc được.
/// </summary>
public class ConversationConfirm
{
    /// <summary>Thời điểm User xác nhận hoàn thành. Null = User chưa xác nhận.</summary>
    [BsonElement("user_at")]
    public DateTime? UserAt { get; set; }

    /// <summary>Thời điểm Reader xác nhận hoàn thành. Null = Reader chưa xác nhận.</summary>
    [BsonElement("reader_at")]
    public DateTime? ReaderAt { get; set; }
}

/// <summary>
/// Bộ đếm tin nhắn chưa đọc — tách riêng cho mỗi bên (User và Reader).
/// Ví dụ: User gửi 3 tin mà Reader chưa đọc → Reader = 3, User = 0.
///
/// Tại sao tách riêng User và Reader?
/// → Mỗi bên có số tin chưa đọc khác nhau (ai gửi thì bên kia tăng count).
/// → UI hiển thị badge đỏ riêng cho từng người.
/// </summary>
public class UnreadCount
{
    /// <summary>Số tin nhắn mà User chưa đọc (do Reader gửi).</summary>
    [BsonElement("user")]
    public int User { get; set; }

    /// <summary>Số tin nhắn mà Reader chưa đọc (do User gửi).</summary>
    [BsonElement("reader")]
    public int Reader { get; set; }
}

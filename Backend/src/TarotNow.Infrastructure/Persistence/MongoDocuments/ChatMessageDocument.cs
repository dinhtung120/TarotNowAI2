/*
 * ===================================================================
 * FILE: ChatMessageDocument.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.MongoDocuments
 * ===================================================================
 * MỤC ĐÍCH:
 *   Định nghĩa cấu trúc cho Collection "chat_messages" trên MongoDB.
 *   Mỗi document = 1 tin nhắn trong cuộc hội thoại giữa User và Reader.
 *
 *   HỆ THỐNG CHAT HỖ TRỢ CÁC LOẠI TIN NHẮN:
 *   → "text": tin nhắn văn bản thông thường
 *   → "system": tin nhắn hệ thống (ví dụ: "Cuộc hội thoại đã bắt đầu")
 *   → "card_share": chia sẻ lá bài Tarot
 *   → "payment_offer": đề xuất thanh toán (Reader đề giá, User chấp nhận/từ chối)
 *   → "payment_accepted": thanh toán đã được chấp nhận
 *   → "payment_rejected": thanh toán bị từ chối
 *
 *   TẠI SAO DÙNG MONGODB CHO CHAT?
 *   → Chat message có volume CỰC CAO (hàng nghìn tin/giây ở peak).
 *   → MongoDB write speed nhanh hơn PostgreSQL với workload này.
 *   → Cấu trúc tin nhắn linh hoạt (payment_payload chỉ có ở payment_offer).
 *   → Dễ sharding theo conversation_id nếu cần scale horizontal.
 *
 *   Tham chiếu: schema.md §8 (chat_messages)
 * ===================================================================
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// Đại diện cho 1 tin nhắn trong collection "chat_messages" trên MongoDB.
/// Mỗi tin nhắn thuộc về 1 cuộc hội thoại (conversation), được xác định qua ConversationId.
/// </summary>
[BsonIgnoreExtraElements]
public partial class ChatMessageDocument
{
    /// <summary>
    /// ID duy nhất của tin nhắn trong MongoDB (ObjectId tự sinh).
    /// [BsonId] + [BsonRepresentation(BsonType.ObjectId)] = lưu dạng ObjectId trong DB,
    /// nhưng trong C# sử dụng kiểu string cho tiện xử lý.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// ID cuộc hội thoại mà tin nhắn này thuộc về.
    /// Tham chiếu tới conversations._id (cũng là ObjectId).
    /// [BsonRepresentation(BsonType.ObjectId)] = đảm bảo lưu đúng kiểu ObjectId trong DB
    /// (nếu không, MongoDB sẽ lưu dạng string thường → không match khi query).
    /// </summary>
    [BsonElement("conversation_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>
    /// UUID người gửi tin nhắn (là User ID từ PostgreSQL).
    /// Có thể là User hoặc Reader — phân biệt bằng cách so sánh với conversation.user_id / reader_id.
    /// </summary>
    [BsonElement("sender_id")]
    public string SenderId { get; set; } = string.Empty;

    /// <summary>
    /// Loại tin nhắn — quyết định cách hiển thị trên UI:
    ///   - "text": tin nhắn văn bản → hiển thị bong bóng chat thường
    ///   - "system": tin nhắn hệ thống → hiển thị ở giữa, màu xám, không có avatar
    ///   - "card_share": chia sẻ lá bài → hiển thị card UI đặc biệt
    ///   - "payment_offer": đề xuất thanh toán → hiển thị nút Accept/Reject
    ///   - "payment_accepted"/"payment_rejected": kết quả thanh toán
    /// </summary>
    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Nội dung chính của tin nhắn.
    /// Với type="text": đây là nội dung chat.
    /// Với type="system": đây là thông báo hệ thống (ví dụ: "Reader đã chấp nhận yêu cầu").
    /// Với type="payment_offer": đây là lời mô tả đi kèm đề xuất giá.
    /// </summary>
    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Payload chi tiết cho tin nhắn loại "payment_offer".
    /// Chứa thông tin: số Diamond đề xuất, ID đề xuất, thời hạn hết hạn.
    /// CHỈ CÓ khi type = "payment_offer" → các loại khác là null.
    /// [BsonIgnoreIfNull] = không lưu trường này vào DB nếu null → tiết kiệm dung lượng.
    /// </summary>
    [BsonElement("payment_payload")]
    [BsonIgnoreIfNull]
    public ChatPaymentPayload? PaymentPayload { get; set; }

    /// <summary>
    /// Tin nhắn đã được đọc hay chưa.
    /// Dùng để tính "unread_count" trên UI (số tin chưa đọc hiển thị badge đỏ).
    /// Khi người nhận mở cuộc hội thoại → tất cả tin nhắn chưa đọc sẽ được set IsRead = true.
    /// </summary>
    [BsonElement("is_read")]
    public bool IsRead { get; set; }

    /// <summary>
    /// Cờ "xóa mềm" (soft delete). Nếu true: tin nhắn đã bị xóa nhưng VẪN CÒN trong DB.
    /// Tại sao không xóa thật? → Cần giữ lại để:
    ///   1. Admin có thể review nội dung vi phạm.
    ///   2. Đảm bảo sequence tin nhắn không bị hỏng.
    ///   3. Có thể khôi phục nếu xóa nhầm.
    /// </summary>
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    /// <summary>Thời điểm tin nhắn bị xóa mềm. Null nếu chưa xóa.</summary>
    [BsonElement("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    /// <summary>Thời điểm tin nhắn được tạo (gửi). Dùng để sắp xếp timeline chat.</summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>Thời điểm cập nhật cuối (nếu tin nhắn bị sửa). Null nếu chưa sửa.</summary>
    [BsonElement("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Cờ đánh dấu tin nhắn vi phạm tiêu chuẩn cộng đồng (Bị Worker tự động ghim).
    /// </summary>
    [BsonElement("is_flagged")]
    public bool IsFlagged { get; set; }

    /// <summary>
    /// Payload lịch sử cuộc gọi đính kèm cho tin nhắn loại "call_log".
    /// </summary>
    [BsonElement("call_payload")]
    [BsonIgnoreIfNull]
    public ChatCallPayload? CallPayload { get; set; }
}

/// <summary>
/// Payload chi tiết đính kèm cho tin nhắn loại "payment_offer" (đề xuất thanh toán).
/// Khi Reader gửi đề xuất giá cho User, tin nhắn sẽ chứa object này bên trong.
/// </summary>
[BsonIgnoreExtraElements]
public class ChatPaymentPayload
{
    /// <summary>
    /// Số Kim Cương (Diamond) mà Reader yêu cầu cho dịch vụ.
    /// Kiểu long vì số Diamond có thể rất lớn (tránh tràn số int).
    /// </summary>
    [BsonElement("amount_diamond")]
    public long AmountDiamond { get; set; }

    /// <summary>
    /// ID của đề xuất thanh toán — dùng để theo dõi và xử lý Accept/Reject.
    /// Liên kết với hệ thống Escrow (ký quỹ) bên PostgreSQL.
    /// </summary>
    [BsonElement("proposal_id")]
    public string? ProposalId { get; set; }

    /// <summary>
    /// Thời hạn hết hạn của đề xuất. Sau thời điểm này, User không thể chấp nhận nữa.
    /// Hệ thống EscrowTimerService sẽ tự động hủy đề xuất hết hạn.
    /// </summary>
    [BsonElement("expires_at")]
    public DateTime? ExpiresAt { get; set; }
}

/// <summary>
/// Payload lịch sử cuộc gọi đính kèm cho tin nhắn loại "call_log".
/// </summary>
[BsonIgnoreExtraElements]
public class ChatCallPayload
{
    [BsonElement("session_id")]
    public string SessionId { get; set; } = string.Empty;

    [BsonElement("call_type")]
    public string CallType { get; set; } = string.Empty;

    [BsonElement("end_reason")]
    public string? EndReason { get; set; }

    [BsonElement("duration_seconds")]
    public int DurationSeconds { get; set; }
}

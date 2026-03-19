/*
 * ===================================================================
 * FILE: ChatDtos.cs
 * NAMESPACE: TarotNow.Application.Common
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Tập hợp các DTO (Data Transfer Object) cho tính năng CHAT.
 *   DTO là "bản sao nhẹ" của dữ liệu, dùng để TRUYỀN TẢI giữa các layer.
 *
 * TẠI SAO DÙNG DTO THAY VÌ DOCUMENT GỐC?
 *   - MongoDB lưu dữ liệu dạng Document (MongoDocuments/)
 *   - Application layer KHÔNG ĐƯỢC biết về MongoDB (Clean Architecture)
 *   - DTO nằm ở Application layer → mọi layer đều dùng được
 *   - Repository (Infrastructure) chuyển đổi Document ↔ DTO
 *   
 *   Ví dụ: ConversationDocument (MongoDB) → ConversationDto (Application) → JSON (API)
 *
 * CÁC DTO TRONG FILE NÀY:
 *   1. ConversationDto: Cuộc trò chuyện
 *   2. ChatMessageDto: Tin nhắn trong cuộc trò chuyện
 *   3. PaymentPayloadDto: Thông tin thanh toán đính kèm tin nhắn
 *   4. ReportDto: Báo cáo vi phạm
 * ===================================================================
 */

namespace TarotNow.Application.Common;

/// <summary>
/// DTO đại diện cho MỘT CUỘC TRÒ CHUYỆN (conversation) giữa user và reader.
///
/// Map từ MongoDB collection "conversations" (theo schema §7 trong blueprint).
/// Repository (Infrastructure layer) chịu trách nhiệm map ConversationDocument ↔ ConversationDto.
/// </summary>
public class ConversationDto
{
    /// <summary>
    /// ID duy nhất của conversation - MongoDB ObjectId dạng string.
    /// Ví dụ: "507f1f77bcf86cd799439011"
    /// MongoDB tự tạo ID này khi insert document.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// UUID của USER (người dùng/người trả tiền) trong cuộc trò chuyện.
    /// Map đến bảng users.id trong PostgreSQL.
    /// Dùng string thay vì Guid vì MongoDB lưu dạng string.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// UUID của READER (người đọc bài/người nhận tiền) trong cuộc trò chuyện.
    /// </summary>
    public string ReaderId { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái cuộc trò chuyện:
    ///   - "pending": đang chờ reader chấp nhận
    ///   - "active": đang hoạt động (2 bên đang chat)
    ///   - "completed": đã hoàn thành (kết thúc bình thường)
    ///   - "cancelled": đã hủy (một bên hủy trước khi bắt đầu)
    ///   - "disputed": đang tranh chấp (cần admin can thiệp)
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Thời điểm tin nhắn cuối cùng được gửi.
    /// Dùng để SẮP XẾP inbox → conversation có tin mới nhất hiện đầu tiên.
    /// Nullable vì conversation mới tạo có thể chưa có tin nhắn.
    /// </summary>
    public DateTime? LastMessageAt { get; set; }

    /// <summary>
    /// Thời hạn chờ reader phản hồi.
    /// Nếu hết hạn mà reader chưa phản hồi → tự động hủy conversation.
    /// Nullable vì không phải tất cả conversation đều có deadline.
    /// </summary>
    public DateTime? OfferExpiresAt { get; set; }

    /// <summary>
    /// Số tin nhắn chưa đọc CỦA USER.
    /// Hiển thị badge đỏ "3" trên icon conversation trong inbox.
    /// Reset về 0 khi user mở conversation (MarkRead).
    /// </summary>
    public int UnreadCountUser { get; set; }

    /// <summary>
    /// Số tin nhắn chưa đọc CỦA READER.
    /// Tương tự UnreadCountUser nhưng cho phía reader.
    /// </summary>
    public int UnreadCountReader { get; set; }

    /// <summary>Thời điểm tạo conversation.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Thời điểm cập nhật cuối cùng.</summary>
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO đại diện cho MỘT TIN NHẮN CHAT.
///
/// Map từ MongoDB collection "chat_messages" (schema §8).
/// Mỗi tin nhắn thuộc về 1 conversation.
/// </summary>
public class ChatMessageDto
{
    /// <summary>MongoDB ObjectId string của tin nhắn.</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>ObjectId string của conversation chứa tin nhắn này.</summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>UUID của người gửi tin nhắn.</summary>
    public string SenderId { get; set; } = string.Empty;

    /// <summary>
    /// Loại tin nhắn:
    ///   - "text": văn bản thường
    ///   - "system": tin nhắn hệ thống (tự động tạo, VD: "User đã join")
    ///   - "card_share": chia sẻ lá bài tarot
    ///   - "payment_offer": đề nghị thanh toán (kèm PaymentPayload)
    ///   - "payment_accepted": đề nghị đã được chấp nhận
    ///   - "payment_rejected": đề nghị bị từ chối
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>Nội dung tin nhắn (text, markdown, hoặc JSON serialized).</summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Thông tin thanh toán đính kèm (chỉ có khi Type = "payment_offer").
    /// Nullable vì đa số tin nhắn là text thường, không có payment.
    /// </summary>
    public PaymentPayloadDto? PaymentPayload { get; set; }

    /// <summary>
    /// Đã đọc hay chưa.
    /// true = người nhận đã mở và đọc tin nhắn.
    /// Dùng cho tính năng "tick xanh" (read receipt) giống WhatsApp.
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>Thời điểm gửi tin nhắn (UTC).</summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO chứa thông tin thanh toán đính kèm trong tin nhắn "payment_offer".
/// Khi reader muốn thu phí → tạo tin nhắn type "payment_offer" kèm payload này.
/// User xem tin nhắn → accept hoặc reject offer.
/// </summary>
public class PaymentPayloadDto
{
    /// <summary>Số diamond mà reader đề xuất cho câu hỏi/dịch vụ.</summary>
    public long AmountDiamond { get; set; }

    /// <summary>
    /// ID của đề xuất - dùng để accept/reject.
    /// Nullable vì có thể chưa được gán (trong một số flow).
    /// </summary>
    public string? ProposalId { get; set; }

    /// <summary>
    /// Hạn chót để user chấp nhận đề xuất.
    /// Sau thời gian này → đề xuất tự hết hạn, user phải yêu cầu mới.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
}

/// <summary>
/// DTO đại diện cho MỘT BÁO CÁO VI PHẠM (Report).
///
/// Map từ MongoDB collection "reports" (schema §10).
/// User tạo report → Admin xem xét → Xử lý (cảnh cáo/khóa/hoàn tiền).
/// </summary>
public class ReportDto
{
    /// <summary>MongoDB ObjectId string.</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>UUID người gửi báo cáo.</summary>
    public string ReporterId { get; set; } = string.Empty;

    /// <summary>
    /// Loại đối tượng bị báo cáo:
    ///   - "message": tin nhắn vi phạm
    ///   - "conversation": cuộc trò chuyện vi phạm
    ///   - "user": người dùng vi phạm
    /// </summary>
    public string TargetType { get; set; } = string.Empty;

    /// <summary>ID của đối tượng bị báo cáo.</summary>
    public string TargetId { get; set; } = string.Empty;

    /// <summary>ObjectId conversation liên quan (tùy chọn, giúp admin xem ngữ cảnh).</summary>
    public string? ConversationRef { get; set; }

    /// <summary>Lý do báo cáo do user nhập.</summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái xử lý báo cáo:
    ///   - "pending": chờ xử lý
    ///   - "processing": đang xem xét
    ///   - "resolved": đã xử lý xong
    ///   - "rejected": báo cáo bị từ chối (không vi phạm)
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Kết quả xử lý (sau khi admin quyết định):
    ///   - "warn": cảnh cáo
    ///   - "freeze": khóa tài khoản
    ///   - "refund": hoàn tiền cho user
    ///   - "no_action": không xử lý (không vi phạm)
    /// </summary>
    public string? Result { get; set; }

    /// <summary>Ghi chú của admin khi xử lý.</summary>
    public string? AdminNote { get; set; }

    /// <summary>Thời điểm tạo báo cáo.</summary>
    public DateTime CreatedAt { get; set; }
}

namespace TarotNow.Application.Common;

/// <summary>
/// DTO cho conversation chat 1-1 — dùng ở Application layer.
///
/// Map từ MongoDB conversations collection (schema §7).
/// Repository chịu trách nhiệm map ConversationDocument ↔ ConversationDto.
/// </summary>
public class ConversationDto
{
    /// <summary>MongoDB ObjectId string.</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>UUID user (payer) — mapping users.id (PostgreSQL).</summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>UUID reader (receiver).</summary>
    public string ReaderId { get; set; } = string.Empty;

    /// <summary>Trạng thái: pending | active | completed | cancelled | disputed.</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Thời điểm tin nhắn cuối — dùng sort inbox.</summary>
    public DateTime? LastMessageAt { get; set; }

    /// <summary>Thời hạn chờ Reader phản hồi trước khi bị hủy tự động.</summary>
    public DateTime? OfferExpiresAt { get; set; }

    /// <summary>Số tin chưa đọc của user.</summary>
    public int UnreadCountUser { get; set; }

    /// <summary>Số tin chưa đọc của reader.</summary>
    public int UnreadCountReader { get; set; }

    /// <summary>Thời điểm tạo.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Thời điểm cập nhật cuối.</summary>
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO cho tin nhắn chat — dùng ở Application layer.
///
/// Map từ MongoDB chat_messages collection (schema §8).
/// </summary>
public class ChatMessageDto
{
    /// <summary>MongoDB ObjectId string.</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>ObjectId string của conversation chứa tin nhắn.</summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>UUID người gửi.</summary>
    public string SenderId { get; set; } = string.Empty;

    /// <summary>Loại tin nhắn: text | system | card_share | payment_*</summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>Nội dung tin nhắn.</summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>Payload thanh toán — chỉ có cho payment_offer.</summary>
    public PaymentPayloadDto? PaymentPayload { get; set; }

    /// <summary>Đã đọc hay chưa.</summary>
    public bool IsRead { get; set; }

    /// <summary>Thời điểm gửi.</summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Payload chi tiết cho tin nhắn payment_offer.
/// Chứa amount diamond và thời hạn.
/// </summary>
public class PaymentPayloadDto
{
    /// <summary>Số diamond đề xuất.</summary>
    public long AmountDiamond { get; set; }

    /// <summary>ID đề xuất — dùng để accept/reject.</summary>
    public string? ProposalId { get; set; }

    /// <summary>Hạn chót chấp nhận.</summary>
    public DateTime? ExpiresAt { get; set; }
}

/// <summary>
/// DTO cho báo cáo vi phạm — dùng ở Application layer.
///
/// Map từ MongoDB reports collection (schema §10).
/// </summary>
public class ReportDto
{
    /// <summary>MongoDB ObjectId string.</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>UUID người báo cáo.</summary>
    public string ReporterId { get; set; } = string.Empty;

    /// <summary>Loại đối tượng bị báo cáo: message | conversation | user.</summary>
    public string TargetType { get; set; } = string.Empty;

    /// <summary>ID đối tượng bị báo cáo.</summary>
    public string TargetId { get; set; } = string.Empty;

    /// <summary>ObjectId conversation liên quan (nếu có).</summary>
    public string? ConversationRef { get; set; }

    /// <summary>Lý do báo cáo.</summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>Trạng thái: pending | processing | resolved | rejected.</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Kết quả xử lý: warn | freeze | refund | no_action.</summary>
    public string? Result { get; set; }

    /// <summary>Ghi chú admin.</summary>
    public string? AdminNote { get; set; }

    /// <summary>Thời điểm tạo.</summary>
    public DateTime CreatedAt { get; set; }
}

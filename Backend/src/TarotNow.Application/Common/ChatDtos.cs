namespace TarotNow.Application.Common;

// DTO mô tả tổng quan một hội thoại chat giữa user và reader.
public class ConversationDto
{
    // Định danh hội thoại.
    public string Id { get; set; } = string.Empty;

    // Định danh người dùng tạo hội thoại.
    public string UserId { get; set; } = string.Empty;

    // Định danh reader tham gia hội thoại.
    public string ReaderId { get; set; } = string.Empty;

    // Tên hiển thị của user cho UI.
    public string? UserName { get; set; }

    // Ảnh đại diện user cho UI.
    public string? UserAvatar { get; set; }

    // Tên hiển thị của reader cho UI.
    public string? ReaderName { get; set; }

    // Ảnh đại diện reader cho UI.
    public string? ReaderAvatar { get; set; }

    // Trạng thái online/offline của reader tại thời điểm trả dữ liệu.
    public string? ReaderStatus { get; set; }

    // Tổng kim cương đang escrow/frozen trong hội thoại.
    public long EscrowTotalFrozen { get; set; }

    // Trạng thái hiện tại của escrow gắn với hội thoại.
    public string? EscrowStatus { get; set; }

    // Trạng thái nghiệp vụ tổng thể của hội thoại.
    public string Status { get; set; } = string.Empty;

    // Thời điểm tin nhắn cuối cùng.
    public DateTime? LastMessageAt { get; set; }

    // Bản xem trước nội dung tin nhắn gần nhất.
    public string? LastMessagePreview { get; set; }

    // Hạn chót đề nghị/offer nếu có SLA.
    public DateTime? OfferExpiresAt { get; set; }

    // SLA giờ áp dụng cho hội thoại.
    public int SlaHours { get; set; } = 12;

    // Trạng thái xác nhận hoàn tất giữa hai bên.
    public ConversationConfirmDto? Confirm { get; set; }

    // Số tin chưa đọc phía user.
    public int UnreadCountUser { get; set; }

    // Số tin chưa đọc phía reader.
    public int UnreadCountReader { get; set; }

    // Thời điểm tạo hội thoại.
    public DateTime CreatedAt { get; set; }

    // Thời điểm cập nhật hội thoại gần nhất.
    public DateTime? UpdatedAt { get; set; }
}

// DTO trạng thái xác nhận hoàn tất hội thoại của hai phía.
public class ConversationConfirmDto
{
    // Mốc thời gian user xác nhận.
    public DateTime? UserAt { get; set; }

    // Mốc thời gian reader xác nhận.
    public DateTime? ReaderAt { get; set; }

    // Chủ thể đã yêu cầu luồng xác nhận.
    public string? RequestedBy { get; set; }

    // Thời điểm tạo yêu cầu xác nhận.
    public DateTime? RequestedAt { get; set; }

    // Mốc auto-resolve nếu hết hạn xác nhận.
    public DateTime? AutoResolveAt { get; set; }
}

// DTO tin nhắn chat thống nhất cho các loại nội dung text/media/payment.
public class ChatMessageDto
{
    // Định danh tin nhắn.
    public string Id { get; set; } = string.Empty;

    // Định danh client-side để idempotency và reconcile optimistic UI.
    public string? ClientMessageId { get; set; }

    // Định danh hội thoại chứa tin nhắn.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh người gửi.
    public string SenderId { get; set; } = string.Empty;

    // Loại tin nhắn nghiệp vụ.
    public string Type { get; set; } = string.Empty;

    // Nội dung text chính của tin nhắn.
    public string Content { get; set; } = string.Empty;

    // Khóa idempotency cho system message phát sinh từ event nền.
    public string? SystemEventKey { get; set; }

    // Payload thanh toán nếu đây là tin nhắn payment.
    public PaymentPayloadDto? PaymentPayload { get; set; }

    // Payload media nếu đây là tin nhắn media.
    public MediaPayloadDto? MediaPayload { get; set; }

    // Cờ đánh dấu tin đã đọc.
    public bool IsRead { get; set; }

    // Thời điểm tạo tin nhắn.
    public DateTime CreatedAt { get; set; }

    // Cờ moderation đánh dấu tin cần kiểm duyệt.
    public bool IsFlagged { get; set; }
}

// DTO thông tin đề nghị thanh toán trong hội thoại.
public class PaymentPayloadDto
{
    // Số kim cương đề nghị thanh toán.
    public long AmountDiamond { get; set; }

    // Mô tả ngắn của đề nghị thanh toán.
    public string? Description { get; set; }

    // Định danh proposal liên quan nếu có.
    public string? ProposalId { get; set; }

    // Hạn hết hiệu lực đề nghị thanh toán.
    public DateTime? ExpiresAt { get; set; }
}

// DTO phản ánh một báo cáo vi phạm trong hệ thống.
public class ReportDto
{
    // Định danh báo cáo.
    public string Id { get; set; } = string.Empty;

    // Định danh người tạo báo cáo.
    public string ReporterId { get; set; } = string.Empty;

    // Loại đối tượng bị báo cáo.
    public string TargetType { get; set; } = string.Empty;

    // Định danh đối tượng bị báo cáo.
    public string TargetId { get; set; } = string.Empty;

    // Tham chiếu hội thoại liên quan nếu có.
    public string? ConversationRef { get; set; }

    // Lý do báo cáo từ người dùng.
    public string Reason { get; set; } = string.Empty;

    // Trạng thái xử lý báo cáo.
    public string Status { get; set; } = string.Empty;

    // Kết quả xử lý cuối cùng.
    public string? Result { get; set; }

    // Ghi chú của quản trị viên.
    public string? AdminNote { get; set; }

    // Thời điểm tạo báo cáo.
    public DateTime CreatedAt { get; set; }
}

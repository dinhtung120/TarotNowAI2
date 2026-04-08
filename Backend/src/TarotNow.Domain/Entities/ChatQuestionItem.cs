
namespace TarotNow.Domain.Entities;

// Entity một item câu hỏi có ràng buộc thanh toán/escrow trong hội thoại.
public class ChatQuestionItem
{
    // Định danh item câu hỏi.
    public Guid Id { get; set; }

    // Định danh phiên tài chính chứa item.
    public Guid FinanceSessionId { get; set; }

    // Mã tham chiếu cuộc hội thoại.
    public string ConversationRef { get; set; } = string.Empty;

    // Người thanh toán cho item này.
    public Guid PayerId { get; set; }

    // Người nhận thanh toán (reader).
    public Guid ReceiverId { get; set; }

    // Loại item câu hỏi (main/follow-up/...).
    public string Type { get; set; } = "main_question";

    // Số Diamond gắn với item.
    public long AmountDiamond { get; set; }

    // Trạng thái vòng đời item (pending/released/refunded/...).
    public string Status { get; set; } = "pending";

    // Tham chiếu tin nhắn đề xuất thanh toán.
    public string? ProposalMessageRef { get; set; }

    // Thời điểm offer hết hạn.
    public DateTime? OfferExpiresAt { get; set; }

    // Mốc người dùng chấp nhận offer.
    public DateTime? AcceptedAt { get; set; }

    // Hạn reader phải phản hồi sau khi nhận item.
    public DateTime? ReaderResponseDueAt { get; set; }

    // Mốc reader đã phản hồi.
    public DateTime? RepliedAt { get; set; }

    // Thời điểm hệ thống tự động release nếu đủ điều kiện.
    public DateTime? AutoReleaseAt { get; set; }

    // Thời điểm hệ thống tự động refund nếu quá hạn xử lý.
    public DateTime? AutoRefundAt { get; set; }

    // Mốc thực tế release tiền.
    public DateTime? ReleasedAt { get; set; }

    // Mốc người dùng xác nhận hoàn tất.
    public DateTime? ConfirmedAt { get; set; }

    // Mốc thực tế hoàn tiền.
    public DateTime? RefundedAt { get; set; }

    // Thời điểm mở cửa sổ khiếu nại.
    public DateTime? DisputeWindowStart { get; set; }

    // Thời điểm đóng cửa sổ khiếu nại.
    public DateTime? DisputeWindowEnd { get; set; }

    // Khóa idempotency để chống tạo item trùng khi retry.
    public string? IdempotencyKey { get; set; }

    // Thời điểm tạo item.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Thời điểm cập nhật gần nhất.
    public DateTime? UpdatedAt { get; set; }

    // Navigation tới phiên tài chính cha.
    public virtual ChatFinanceSession? FinanceSession { get; set; }
}

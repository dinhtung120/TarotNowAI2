
namespace TarotNow.Domain.Entities;

// Entity phiên tài chính của hội thoại để theo dõi trạng thái escrow tổng và các item thanh toán liên quan.
public class ChatFinanceSession
{
    // Định danh phiên tài chính.
    public Guid Id { get; set; }

    // Mã tham chiếu cuộc hội thoại chứa phiên tài chính.
    public string ConversationRef { get; set; } = string.Empty;

    // Người dùng trả phí trong phiên.
    public Guid UserId { get; set; }

    // Reader nhận thanh toán trong phiên.
    public Guid ReaderId { get; set; }

    // Trạng thái phiên tài chính (pending/closed/...).
    public string Status { get; set; } = "pending";

    // Tổng số Diamond đang bị giữ trong escrow của phiên.
    public long TotalFrozen { get; set; }

    // Thời điểm tạo phiên.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Thời điểm cập nhật gần nhất.
    public DateTime? UpdatedAt { get; set; }

    // Danh sách item câu hỏi tài chính thuộc phiên.
    public virtual ICollection<ChatQuestionItem> QuestionItems { get; set; } = new List<ChatQuestionItem>();
}

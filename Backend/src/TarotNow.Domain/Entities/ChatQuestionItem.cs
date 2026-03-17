using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarotNow.Domain.Entities;

/// <summary>
/// EF Core entity map bảng chat_question_items (PostgreSQL).
///
/// Escrow từng câu hỏi — mỗi item có timer riêng.
/// Timer logic:
///   accepted_at + 24h → auto_refund_at (reader phải reply trước)
///   replied_at + 24h → auto_release_at (auto-release nếu không dispute)
///   dispute_window: 24h từ release/refund
/// </summary>
[Table("chat_question_items")]
public class ChatQuestionItem
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("finance_session_id")]
    public Guid FinanceSessionId { get; set; }

    /// <summary>MongoDB conversations._id.</summary>
    [Column("conversation_ref")]
    [Required]
    public string ConversationRef { get; set; } = string.Empty;

    /// <summary>Payer (user).</summary>
    [Column("payer_id")]
    public Guid PayerId { get; set; }

    /// <summary>Receiver (reader).</summary>
    [Column("receiver_id")]
    public Guid ReceiverId { get; set; }

    /// <summary>main_question | add_question.</summary>
    [Column("type")]
    [Required]
    public string Type { get; set; } = "main_question";

    /// <summary>Số Diamond escrow cho item này.</summary>
    [Column("amount_diamond")]
    public long AmountDiamond { get; set; }

    /// <summary>pending | accepted | released | refunded | disputed.</summary>
    [Column("status")]
    [Required]
    public string Status { get; set; } = "pending";

    /// <summary>chat_messages._id (MongoDB ObjectId) — proposal message.</summary>
    [Column("proposal_message_ref")]
    public string? ProposalMessageRef { get; set; }

    /// <summary>Hết hạn offer (nếu reader không accept).</summary>
    [Column("offer_expires_at")]
    public DateTime? OfferExpiresAt { get; set; }

    [Column("accepted_at")]
    public DateTime? AcceptedAt { get; set; }

    /// <summary>= accepted_at + 24h — deadline cho reader reply.</summary>
    [Column("reader_response_due_at")]
    public DateTime? ReaderResponseDueAt { get; set; }

    [Column("replied_at")]
    public DateTime? RepliedAt { get; set; }

    /// <summary>= replied_at + 24h — tự release nếu không confirm/dispute.</summary>
    [Column("auto_release_at")]
    public DateTime? AutoReleaseAt { get; set; }

    /// <summary>= accepted_at + 24h — tự refund nếu reader không reply.</summary>
    [Column("auto_refund_at")]
    public DateTime? AutoRefundAt { get; set; }

    [Column("released_at")]
    public DateTime? ReleasedAt { get; set; }

    [Column("confirmed_at")]
    public DateTime? ConfirmedAt { get; set; }

    [Column("refunded_at")]
    public DateTime? RefundedAt { get; set; }

    /// <summary>Bắt đầu cửa sổ dispute (24h).</summary>
    [Column("dispute_window_start")]
    public DateTime? DisputeWindowStart { get; set; }

    /// <summary>Kết thúc cửa sổ dispute.</summary>
    [Column("dispute_window_end")]
    public DateTime? DisputeWindowEnd { get; set; }

    /// <summary>Idempotency key — chống double-freeze/release/refund.</summary>
    [Column("idempotency_key")]
    public string? IdempotencyKey { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    [ForeignKey("FinanceSessionId")]
    public virtual ChatFinanceSession? FinanceSession { get; set; }
}

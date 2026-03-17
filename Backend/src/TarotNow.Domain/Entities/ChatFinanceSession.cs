using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarotNow.Domain.Entities;

/// <summary>
/// EF Core entity map bảng chat_finance_sessions (PostgreSQL).
///
/// 1 conversation = 1 finance session.
/// Gom tất cả escrow question items của conversation.
/// conversation_ref → MongoDB conversations._id.
/// </summary>
[Table("chat_finance_sessions")]
public class ChatFinanceSession
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>MongoDB conversations._id (24 hex chars).</summary>
    [Column("conversation_ref")]
    [Required]
    public string ConversationRef { get; set; } = string.Empty;

    /// <summary>Payer (user).</summary>
    [Column("user_id")]
    public Guid UserId { get; set; }

    /// <summary>Receiver (reader).</summary>
    [Column("reader_id")]
    public Guid ReaderId { get; set; }

    /// <summary>pending | active | completed | refunded | disputed | cancelled.</summary>
    [Column("status")]
    [Required]
    public string Status { get; set; } = "pending";

    /// <summary>Tổng Diamond đang freeze trong session này.</summary>
    [Column("total_frozen")]
    public long TotalFrozen { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<ChatQuestionItem> QuestionItems { get; set; } = new List<ChatQuestionItem>();
}

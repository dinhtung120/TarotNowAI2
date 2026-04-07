

namespace TarotNow.Domain.Entities;

public class ChatQuestionItem
{
    
    public Guid Id { get; set; }

    
    public Guid FinanceSessionId { get; set; }

        public string ConversationRef { get; set; } = string.Empty;

        public Guid PayerId { get; set; }

        public Guid ReceiverId { get; set; }

        public string Type { get; set; } = "main_question";

        public long AmountDiamond { get; set; }

        public string Status { get; set; } = "pending";

        public string? ProposalMessageRef { get; set; }

        public DateTime? OfferExpiresAt { get; set; }

    
    public DateTime? AcceptedAt { get; set; }

        public DateTime? ReaderResponseDueAt { get; set; }

    
    public DateTime? RepliedAt { get; set; }

        public DateTime? AutoReleaseAt { get; set; }

        public DateTime? AutoRefundAt { get; set; }

    
    public DateTime? ReleasedAt { get; set; }

    
    public DateTime? ConfirmedAt { get; set; }

    
    public DateTime? RefundedAt { get; set; }

        public DateTime? DisputeWindowStart { get; set; }
    public DateTime? DisputeWindowEnd { get; set; }

        public string? IdempotencyKey { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    
    public virtual ChatFinanceSession? FinanceSession { get; set; }
}

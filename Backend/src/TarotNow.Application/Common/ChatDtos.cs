

namespace TarotNow.Application.Common;

public class ConversationDto
{
        public string Id { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string ReaderId { get; set; } = string.Empty;

    
    public string? UserName { get; set; }
    public string? UserAvatar { get; set; }
    public string? ReaderName { get; set; }
    public string? ReaderAvatar { get; set; }
    public string? ReaderStatus { get; set; }
    
    
    public long EscrowTotalFrozen { get; set; }
    public string? EscrowStatus { get; set; }
    

        public string Status { get; set; } = string.Empty;

        public DateTime? LastMessageAt { get; set; }

        public string? LastMessagePreview { get; set; }

        public DateTime? OfferExpiresAt { get; set; }

        public int SlaHours { get; set; } = 12;

        public ConversationConfirmDto? Confirm { get; set; }

        public int UnreadCountUser { get; set; }

        public int UnreadCountReader { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
}

public class ConversationConfirmDto
{
    public DateTime? UserAt { get; set; }

    public DateTime? ReaderAt { get; set; }

    public string? RequestedBy { get; set; }

    public DateTime? RequestedAt { get; set; }

    public DateTime? AutoResolveAt { get; set; }
}

public class ChatMessageDto
{
        public string Id { get; set; } = string.Empty;

        public string ConversationId { get; set; } = string.Empty;

        public string SenderId { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public PaymentPayloadDto? PaymentPayload { get; set; }

        public MediaPayloadDto? MediaPayload { get; set; }

        public CallSessionDto? CallPayload { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsFlagged { get; set; }
}

public class PaymentPayloadDto
{
        public long AmountDiamond { get; set; }

        public string? Description { get; set; }

        public string? ProposalId { get; set; }

        public DateTime? ExpiresAt { get; set; }
}

public class ReportDto
{
        public string Id { get; set; } = string.Empty;

        public string ReporterId { get; set; } = string.Empty;

        public string TargetType { get; set; } = string.Empty;

        public string TargetId { get; set; } = string.Empty;

        public string? ConversationRef { get; set; }

        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? Result { get; set; }

        public string? AdminNote { get; set; }

        public DateTime CreatedAt { get; set; }
}

public class CallSessionDto
{
        public string Id { get; set; } = string.Empty;

        public string ConversationId { get; set; } = string.Empty;

        public string InitiatorId { get; set; } = string.Empty;

        public TarotNow.Domain.Enums.CallType Type { get; set; }

        public TarotNow.Domain.Enums.CallSessionStatus Status { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? EndedAt { get; set; }

        public string? EndReason { get; set; }

        public int? DurationSeconds { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
}

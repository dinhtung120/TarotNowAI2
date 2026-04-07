

namespace TarotNow.Domain.Entities;

public class AiRequest
{
    
    public Guid Id { get; set; } = Guid.NewGuid();
    
    
    public Guid UserId { get; set; }
    
        public string ReadingSessionRef { get; set; } = null!;
    
        public short? FollowupSequence { get; set; }

        public string Status { get; set; } = Enums.AiRequestStatus.Requested;

    
    public DateTimeOffset? FirstTokenAt { get; set; }
    
    public DateTimeOffset? CompletionMarkerAt { get; set; }
    
    
    public string? FinishReason { get; set; }
    
    public short RetryCount { get; set; }

    
    public string? PromptVersion { get; set; }
    
    public string? PolicyVersion { get; set; }
    
    
    public Guid? CorrelationId { get; set; }
    public string? TraceId { get; set; }

    
    public long ChargeGold { get; set; }
    public long ChargeDiamond { get; set; }

    
    public string? RequestedLocale { get; set; }
    public string? ReturnedLocale { get; set; }
    
    public string? FallbackReason { get; set; }
    
        public string? IdempotencyKey { get; set; }
    
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    
    public User User { get; set; } = null!;
}



namespace TarotNow.Domain.Entities;

public class WithdrawalRequest
{
    
    public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateOnly BusinessDateUtc { get; set; }

        public long AmountDiamond { get; set; }

        public long AmountVnd { get; set; }

        public long FeeVnd { get; set; }

        public long NetAmountVnd { get; set; }

    
    public string BankName { get; set; } = string.Empty;

    
    public string BankAccountName { get; set; } = string.Empty;

    
    public string BankAccountNumber { get; set; } = string.Empty;

        public string Status { get; set; } = "pending";

        public Guid? AdminId { get; set; }

    
    public string? AdminNote { get; set; }

    
    public DateTime? ProcessedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}

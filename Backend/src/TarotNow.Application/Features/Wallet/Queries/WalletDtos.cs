

using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Wallet.Queries;

public class WalletBalanceDto
{
        public long GoldBalance { get; set; }
    
        public long DiamondBalance { get; set; }
    
        public long FrozenDiamondBalance { get; set; }
}

public class WalletTransactionDto
{
    public Guid Id { get; set; }
    
        public string Currency { get; set; } = string.Empty;
    
        public string Type { get; set; } = string.Empty;
    
        public long Amount { get; set; }
    
        public long BalanceBefore { get; set; }
    
        public long BalanceAfter { get; set; }
    
        public string? Description { get; set; }
    
        public DateTime CreatedAt { get; set; }
}

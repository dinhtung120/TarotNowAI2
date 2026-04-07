

using System;

namespace TarotNow.Domain.Entities;

public class DepositOrder
{
    
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }
    
    
    public long AmountVnd { get; private set; }
    
    public long DiamondAmount { get; private set; }
    
    
    public string Status { get; private set; } = string.Empty;

    
    public string? TransactionId { get; private set; }

    
    public string? FxSnapshot { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime? ProcessedAt { get; private set; }

    
    public User User { get; private set; } = null!;

    protected DepositOrder() { } 

        public DepositOrder(Guid userId, long amountVnd, long diamondAmount)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        AmountVnd = amountVnd;
        DiamondAmount = diamondAmount;
        Status = "Pending";
        CreatedAt = DateTime.UtcNow;
    }

        public void MarkAsSuccess(string transactionId, string? fxSnapshot = null)
    {
        if (Status == "Success")
            throw new InvalidOperationException("This order is already marked as success.");

        Status = "Success";
        TransactionId = transactionId;
        FxSnapshot = fxSnapshot;
        ProcessedAt = DateTime.UtcNow;
    }

    public void MarkAsFailed(string transactionId)
    {
        if (Status == "Success")
            throw new InvalidOperationException("Cannot fail a successful order.");

        Status = "Failed";
        TransactionId = transactionId;
        ProcessedAt = DateTime.UtcNow;
    }

    public void SetClientTransactionToken(string transactionToken)
    {
        if (string.IsNullOrWhiteSpace(transactionToken))
            throw new ArgumentException("Transaction token is required.", nameof(transactionToken));

        if (Status != "Pending")
            throw new InvalidOperationException("Client transaction token can only be set on pending order.");

        if (!string.IsNullOrWhiteSpace(TransactionId))
            throw new InvalidOperationException("Transaction token is already set.");

        TransactionId = transactionToken.Trim();
    }
}

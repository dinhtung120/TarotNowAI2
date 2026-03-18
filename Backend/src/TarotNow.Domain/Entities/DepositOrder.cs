using System;

namespace TarotNow.Domain.Entities;

public class DepositOrder
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    
    public long AmountVnd { get; private set; }
    public long DiamondAmount { get; private set; }
    
    // Status: Pending, Success, Failed
    public string Status { get; private set; } = string.Empty;

    // Idempotency: Lấy mã giao dịch từ Payment Gateway
    public string? TransactionId { get; private set; }

    // Theo dõi tỉ giá hoặc log phụ
    public string? FxSnapshot { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }

    public User User { get; private set; } = null!;

    protected DepositOrder() { } // EF Core

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
}

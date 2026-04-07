

using System;

namespace TarotNow.Domain.Entities;

public class WalletTransaction
{
    
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }
    
    
    public string Currency { get; private set; } = string.Empty;
    
    public string Type { get; private set; } = string.Empty;
    
    
    public long Amount { get; private set; }
    
    
    public long BalanceBefore { get; private set; }
    
    public long BalanceAfter { get; private set; }
    
    
    public string? ReferenceSource { get; private set; }
    
    public string? ReferenceId { get; private set; }
    
    
    public string? Description { get; private set; }
    
    public string? MetadataJson { get; private set; }
    
    
    public string? IdempotencyKey { get; private set; }
    
    
    public DateTime CreatedAt { get; private set; }

    protected WalletTransaction() { } 

        private WalletTransaction(Guid userId, string currency, string type, long amount, long balanceBefore, long balanceAfter, string? referenceSource, string? referenceId, string? description, string? metadataJson, string? idempotencyKey)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Currency = currency;
        Type = type;
        Amount = amount;
        BalanceBefore = balanceBefore;
        BalanceAfter = balanceAfter;
        ReferenceSource = referenceSource;
        ReferenceId = referenceId;
        Description = description;
        MetadataJson = metadataJson;
        IdempotencyKey = idempotencyKey;
        CreatedAt = DateTime.UtcNow;
    }

        public static WalletTransaction Create(WalletTransactionCreateRequest request)
    {
        return new WalletTransaction(
            request.UserId,
            request.Currency,
            request.Type,
            request.Amount,
            request.BalanceBefore,
            request.BalanceAfter,
            request.ReferenceSource,
            request.ReferenceId,
            request.Description,
            request.MetadataJson,
            request.IdempotencyKey);
    }
}

public sealed class WalletTransactionCreateRequest
{
    public Guid UserId { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public long Amount { get; init; }
    public long BalanceBefore { get; init; }
    public long BalanceAfter { get; init; }
    public string? ReferenceSource { get; init; }
    public string? ReferenceId { get; init; }
    public string? Description { get; init; }
    public string? MetadataJson { get; init; }
    public string? IdempotencyKey { get; init; }
}

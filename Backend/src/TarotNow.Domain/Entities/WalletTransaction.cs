using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity đại diện cho một lịch sử giao dịch (Ledger).
/// Tương ứng với bảng wallet_transactions. Entity này chỉ Read/Insert, không bao giờ Update.
/// </summary>
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

    /// <summary>
    /// Tạo 1 record Ledger lưu lịch sử thay đổi số dư.
    /// Giải thích: Factory method giúp chuẩn hóa đầu vào trước khi insert DB.
    /// </summary>
    public static WalletTransaction Create(
        Guid userId, 
        string currency, 
        string type, 
        long amount, 
        long balanceBefore, 
        long balanceAfter, 
        string? referenceSource = null, 
        string? referenceId = null, 
        string? description = null, 
        string? metadataJson = null, 
        string? idempotencyKey = null)
    {
        return new WalletTransaction(userId, currency, type, amount, balanceBefore, balanceAfter, referenceSource, referenceId, description, metadataJson, idempotencyKey);
    }
}

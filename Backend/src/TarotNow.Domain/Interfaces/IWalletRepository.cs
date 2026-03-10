using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Interfaces;

/// <summary>
/// Domain Interface cho thao tác ví bảo mật cao.
/// Bắt buộc gọi qua PostgreSQL Stored Procedures proc_wallet_* để chống race condition.
/// App layer không được phép tự do lưu số dư.
/// </summary>
public interface IWalletRepository
{
    Task CreditAsync(Guid userId, string currency, string type, long amount, 
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    Task DebitAsync(Guid userId, string currency, string type, long amount, 
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    Task FreezeAsync(Guid userId, long amount, 
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    Task ReleaseAsync(Guid payerId, Guid receiverId, long amount, 
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    Task RefundAsync(Guid userId, long amount, 
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);
}

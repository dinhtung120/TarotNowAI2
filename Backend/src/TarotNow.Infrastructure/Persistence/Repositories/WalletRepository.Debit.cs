using TarotNow.Application.Common;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial thao tác debit ví.
public partial class WalletRepository
{
    /// <summary>
    /// Debit ví và bỏ qua kết quả chi tiết.
    /// Luồng xử lý: chuyển tiếp sang DebitWithResultAsync để dùng chung luồng xử lý chuẩn.
    /// </summary>
    public Task DebitAsync(
        Guid userId,
        string currency,
        string type,
        long amount,
        string? referenceSource = null,
        string? referenceId = null,
        string? description = null,
        string? metadataJson = null,
        string? idempotencyKey = null,
        CancellationToken cancellationToken = default)
        => DebitWithResultAsync(
            userId,
            currency,
            type,
            amount,
            referenceSource,
            referenceId,
            description,
            metadataJson,
            idempotencyKey,
            cancellationToken);

    /// <summary>
    /// Debit ví và trả về kết quả xử lý.
    /// Luồng xử lý: dựng BalanceChangeRequest với IsDebit=true rồi gọi ExecuteBalanceChangeAsync.
    /// </summary>
    public Task<WalletOperationResult> DebitWithResultAsync(
        Guid userId,
        string currency,
        string type,
        long amount,
        string? referenceSource = null,
        string? referenceId = null,
        string? description = null,
        string? metadataJson = null,
        string? idempotencyKey = null,
        CancellationToken cancellationToken = default)
    {
        var request = new BalanceChangeRequest(
            userId,
            currency,
            type,
            amount,
            true,
            "Debit",
            referenceSource,
            referenceId,
            description,
            metadataJson,
            idempotencyKey);

        return ExecuteBalanceChangeAsync(request, cancellationToken);
        // Debit sẽ tự đi qua nhánh tracking chi tiêu trong BalanceOperations.
    }
}

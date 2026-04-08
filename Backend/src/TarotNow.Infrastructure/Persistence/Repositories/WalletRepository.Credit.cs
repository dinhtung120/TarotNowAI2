using TarotNow.Application.Common;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial thao tác credit ví.
public partial class WalletRepository
{
    /// <summary>
    /// Credit ví và bỏ qua kết quả chi tiết.
    /// Luồng xử lý: chuyển tiếp sang CreditWithResultAsync để dùng chung logic idempotency.
    /// </summary>
    public Task CreditAsync(
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
        => CreditWithResultAsync(
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
    /// Credit ví và trả về kết quả xử lý idempotency.
    /// Luồng xử lý: dựng BalanceChangeRequest với IsDebit=false rồi thực thi qua ExecuteBalanceChangeAsync.
    /// </summary>
    public Task<WalletOperationResult> CreditWithResultAsync(
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
            false,
            "Credit",
            referenceSource,
            referenceId,
            description,
            metadataJson,
            idempotencyKey);

        return ExecuteBalanceChangeAsync(request, cancellationToken);
        // Tập trung toàn bộ kiểm soát transaction/idempotency ở một luồng xử lý thống nhất.
    }
}

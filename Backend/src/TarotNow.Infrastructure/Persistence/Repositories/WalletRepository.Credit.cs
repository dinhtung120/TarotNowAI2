using TarotNow.Application.Common;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class WalletRepository
{
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
    }
}

using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class WalletRepository
{
    public Task FreezeAsync(
        Guid userId,
        long amount,
        string? referenceSource = null,
        string? referenceId = null,
        string? description = null,
        string? metadataJson = null,
        string? idempotencyKey = null,
        CancellationToken cancellationToken = default)
    {
        var request = new EscrowMutationRequest(
            userId,
            amount,
            TransactionType.EscrowFreeze,
            -amount,
            "Freeze",
            referenceSource,
            referenceId,
            description,
            metadataJson,
            idempotencyKey);

        return ExecuteEscrowMutationAsync(request, static (user, value) => user.FreezeDiamond(value), cancellationToken);
    }
}

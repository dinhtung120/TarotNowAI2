namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class WalletRepository
{
    public Task ReleaseAsync(
        Guid payerId,
        Guid receiverId,
        long amount,
        string? referenceSource = null,
        string? referenceId = null,
        string? description = null,
        string? metadataJson = null,
        string? idempotencyKey = null,
        CancellationToken cancellationToken = default)
    {
        var request = new ReleaseRequest(
            payerId,
            receiverId,
            amount,
            "Release",
            referenceSource,
            referenceId,
            description,
            metadataJson,
            idempotencyKey);

        return ExecuteReleaseAsync(request, cancellationToken);
    }
}

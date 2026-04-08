using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial thao tác consume số dư frozen diamond.
public partial class WalletRepository
{
    /// <summary>
    /// Tiêu thụ frozen diamond sau khi nghiệp vụ escrow hoàn tất.
    /// Luồng xử lý: tạo request EscrowRelease với ledger amount âm và thực thi mutation trên frozen balance.
    /// </summary>
    public Task ConsumeAsync(
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
            TransactionType.EscrowRelease,
            -amount,
            "Consume",
            referenceSource,
            referenceId,
            description,
            metadataJson,
            idempotencyKey);

        return ExecuteEscrowMutationAsync(request, static (user, value) => user.ConsumeFrozenDiamond(value), cancellationToken);
        // Consume giảm frozen quỹ ký quỹ và ghi ledger phục vụ đối soát.
    }
}

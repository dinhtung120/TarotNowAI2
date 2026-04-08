using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial thao tác freeze diamond vào escrow.
public partial class WalletRepository
{
    /// <summary>
    /// Khóa một lượng diamond vào frozen balance.
    /// Luồng xử lý: tạo EscrowMutationRequest loại EscrowFreeze và áp dụng mutation FreezeDiamond.
    /// </summary>
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
        // Freeze ghi ledger âm để phản ánh giảm số dư khả dụng ngay lập tức.
    }
}

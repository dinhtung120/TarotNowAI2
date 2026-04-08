using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial thao tác hoàn trả frozen diamond cho user.
public partial class WalletRepository
{
    /// <summary>
    /// Hoàn trả frozen diamond về số dư khả dụng.
    /// Luồng xử lý: tạo EscrowMutationRequest loại EscrowRefund và áp dụng mutation RefundFrozenDiamond.
    /// </summary>
    public Task RefundAsync(
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
            TransactionType.EscrowRefund,
            amount,
            "Refund",
            referenceSource,
            referenceId,
            description,
            metadataJson,
            idempotencyKey);

        return ExecuteEscrowMutationAsync(request, static (user, value) => user.RefundFrozenDiamond(value), cancellationToken);
        // Refund ghi ledger dương để phản ánh tiền quay lại ví khả dụng.
    }
}

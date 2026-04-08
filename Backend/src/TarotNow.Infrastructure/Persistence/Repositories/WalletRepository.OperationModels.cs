namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial khai báo các operation model nội bộ cho WalletRepository.
public partial class WalletRepository
{
    // Model chuẩn hóa dữ liệu cho các mutation escrow (freeze/refund/consume).
    private readonly record struct EscrowMutationRequest(
        Guid UserId,
        long Amount,
        string TransactionType,
        long LedgerAmount,
        string OperationName,
        string? ReferenceSource,
        string? ReferenceId,
        string? Description,
        string? MetadataJson,
        string? IdempotencyKey);

    // Model chuẩn hóa dữ liệu cho mutation balance credit/debit.
    private readonly record struct BalanceChangeRequest(
        Guid UserId,
        string Currency,
        string Type,
        long Amount,
        bool IsDebit,
        string OperationName,
        string? ReferenceSource,
        string? ReferenceId,
        string? Description,
        string? MetadataJson,
        string? IdempotencyKey);

    // Model dữ liệu cho luồng release từ payer sang receiver.
    private readonly record struct ReleaseRequest(
        Guid PayerId,
        Guid ReceiverId,
        long Amount,
        string OperationName,
        string? ReferenceSource,
        string? ReferenceId,
        string? Description,
        string? MetadataJson,
        string? IdempotencyKey);

    // Model dữ liệu ghi ledger entry chuẩn hóa.
    private readonly record struct WalletLedgerEntryRequest(
        Guid UserId,
        string Currency,
        string Type,
        long Amount,
        long BalanceBefore,
        long BalanceAfter,
        string? ReferenceSource,
        string? ReferenceId,
        string? Description,
        string? MetadataJson,
        string? IdempotencyKey);
}

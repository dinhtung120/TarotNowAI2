namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class WalletRepository
{
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

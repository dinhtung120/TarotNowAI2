using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý mutation quỹ escrow (freeze/refund/consume...).
public partial class WalletRepository
{
    /// <summary>
    /// Thực thi mutation escrow trong transaction với idempotency.
    /// Luồng xử lý: chuẩn hóa idempotency key, chạy ApplyEscrowMutationAsync và bắt unique violation để coi là đã xử lý.
    /// </summary>
    private async Task ExecuteEscrowMutationAsync(
        EscrowMutationRequest request,
        Action<User, long> applyMutation,
        CancellationToken cancellationToken)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(request.IdempotencyKey);

        try
        {
            await ExecuteWithTransactionAsync(
                () => ApplyEscrowMutationAsync(request, normalizedIdempotencyKey, applyMutation, cancellationToken),
                cancellationToken);
        }
        catch (DbUpdateException exception) when (IsIdempotencyUniqueViolation(exception, normalizedIdempotencyKey))
        {
            LogIdempotencyAlreadyHandled(exception, request.OperationName, request.UserId, normalizedIdempotencyKey);
            // Trùng idempotency key là trạng thái hợp lệ do retry request từ client/job.
        }
    }

    /// <summary>
    /// Áp dụng mutation escrow và ghi ledger.
    /// Luồng xử lý: kiểm tra idempotency trước/sau lock user, mutate balance, ghi ledger, rồi track spending nếu là EscrowRelease.
    /// </summary>
    private async Task ApplyEscrowMutationAsync(
        EscrowMutationRequest request,
        string? normalizedIdempotencyKey,
        Action<User, long> applyMutation,
        CancellationToken cancellationToken)
    {
        if (await TryHandleIdempotentAsync(normalizedIdempotencyKey, cancellationToken)) return;
        var user = await GetUserForUpdateAsync(request.UserId, "user", cancellationToken);
        if (await TryHandleIdempotentAsync(normalizedIdempotencyKey, cancellationToken)) return;
        // Double-check sau lock để tránh ghi ledger trùng trong race-condition.

        var balanceBefore = user.DiamondBalance;
        var frozenBalanceBefore = user.FrozenDiamondBalance;
        applyMutation(user, request.Amount);
        var balanceAfter = user.DiamondBalance;
        var frozenBalanceAfter = user.FrozenDiamondBalance;
        var ledgerEntry = CreateWalletLedgerEntry(new WalletLedgerEntryRequest(
            request.UserId,
            CurrencyType.Diamond,
            request.TransactionType,
            request.LedgerAmount,
            balanceBefore,
            balanceAfter,
            balanceBefore,
            balanceAfter,
            frozenBalanceBefore,
            frozenBalanceAfter,
            request.ReferenceSource,
            request.ReferenceId,
            request.Description,
            request.MetadataJson,
            normalizedIdempotencyKey));

        _dbContext.Set<WalletTransaction>().Add(ledgerEntry);
        await _dbContext.SaveChangesAsync(cancellationToken);
        // Persist mutation và ledger trong cùng transaction để đảm bảo tính toàn vẹn tài chính.

    }
}

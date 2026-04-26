using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Common;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý thay đổi số dư ví (credit/debit) có idempotency.
public partial class WalletRepository
{
    /// <summary>
    /// Thực thi một thao tác thay đổi số dư với cơ chế transaction + idempotency.
    /// Luồng xử lý: chuẩn hóa idempotency key, chạy mutation trong transaction và bắt unique violation để coi là đã xử lý trước đó.
    /// </summary>
    private async Task<WalletOperationResult> ExecuteBalanceChangeAsync(
        BalanceChangeRequest request,
        CancellationToken cancellationToken)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(request.IdempotencyKey);
        var alreadyHandled = false;

        try
        {
            await ExecuteWithTransactionAsync(async () =>
            {
                alreadyHandled = await TryApplyBalanceChangeAsync(request, normalizedIdempotencyKey, cancellationToken);
            }, cancellationToken);
        }
        catch (DbUpdateException exception) when (IsIdempotencyUniqueViolation(exception, normalizedIdempotencyKey))
        {
            LogIdempotencyAlreadyHandled(exception, request.OperationName, request.UserId, normalizedIdempotencyKey);
            alreadyHandled = true;
            // Unique violation tại ledger idempotency index nghĩa là request đã được xử lý trước đó.
        }

        return alreadyHandled ? WalletOperationResult.AlreadyHandledResult : WalletOperationResult.ExecutedResult;
    }

    /// <summary>
    /// Áp dụng mutation số dư nếu chưa xử lý idempotent.
    /// Luồng xử lý: kiểm tra idempotency trước/sau khóa user, mutate balance, ghi ledger và track leaderboard khi là debit.
    /// </summary>
    private async Task<bool> TryApplyBalanceChangeAsync(
        BalanceChangeRequest request,
        string? normalizedIdempotencyKey,
        CancellationToken cancellationToken)
    {
        if (await TryHandleIdempotentAsync(normalizedIdempotencyKey, cancellationToken))
        {
            return true;
            // Request đã có ledger theo idempotency key nên bỏ qua toàn bộ mutation.
        }

        var user = await GetUserForUpdateAsync(request.UserId, "user", cancellationToken);
        if (await TryHandleIdempotentAsync(normalizedIdempotencyKey, cancellationToken))
        {
            return true;
            // Double-check sau FOR UPDATE để chặn race-condition giữa các request song song.
        }

        var balanceBefore = ResolveAvailableBalance(user, request.Currency);
        var frozenBalanceBefore = ResolveFrozenBalance(user, request.Currency);
        ApplyBalanceMutation(user, request);
        var balanceAfter = ResolveAvailableBalance(user, request.Currency);
        var frozenBalanceAfter = ResolveFrozenBalance(user, request.Currency);

        var ledgerEntry = CreateWalletLedgerEntry(new WalletLedgerEntryRequest(
            request.UserId,
            request.Currency,
            request.Type,
            ResolveLedgerAmount(request),
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
        // Lưu ledger cùng transaction để bảo đảm audit trail luôn đồng bộ với số dư.

        return false;
    }

    /// <summary>
    /// Áp dụng mutation số dư trên aggregate User.
    /// Luồng xử lý: debit gọi user.Debit, còn lại gọi user.Credit theo currency/type.
    /// </summary>
    private static void ApplyBalanceMutation(User user, BalanceChangeRequest request)
    {
        if (request.IsDebit)
        {
            user.Debit(request.Currency, request.Amount);
            return;
            // Nhánh debit dừng sớm để tránh rơi vào credit branch.
        }

        user.Credit(request.Currency, request.Amount, request.Type);
    }

    /// <summary>
    /// Tính amount ghi vào ledger.
    /// Luồng xử lý: debit ghi âm, credit ghi dương.
    /// </summary>
    private static long ResolveLedgerAmount(BalanceChangeRequest request)
        => request.IsDebit ? -request.Amount : request.Amount;

    /// <summary>
    /// Lấy số dư khả dụng hiện tại theo currency.
    /// Luồng xử lý: gold trả GoldBalance, còn lại trả DiamondBalance.
    /// </summary>
    private static long ResolveAvailableBalance(User user, string currency)
        => currency == CurrencyType.Gold ? user.GoldBalance : user.DiamondBalance;

    /// <summary>
    /// Lấy số dư frozen hiện tại theo currency.
    /// Luồng xử lý: chỉ Diamond có frozen balance, Gold luôn bằng 0.
    /// </summary>
    private static long ResolveFrozenBalance(User user, string currency)
        => currency == CurrencyType.Diamond ? user.FrozenDiamondBalance : 0;
}

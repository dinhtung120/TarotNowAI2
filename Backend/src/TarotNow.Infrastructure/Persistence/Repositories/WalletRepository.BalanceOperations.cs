using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Common;
using TarotNow.Application.Helpers;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class WalletRepository
{
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
        }

        return alreadyHandled ? WalletOperationResult.AlreadyHandledResult : WalletOperationResult.ExecutedResult;
    }

    private async Task<bool> TryApplyBalanceChangeAsync(
        BalanceChangeRequest request,
        string? normalizedIdempotencyKey,
        CancellationToken cancellationToken)
    {
        if (await TryHandleIdempotentAsync(normalizedIdempotencyKey, cancellationToken))
        {
            return true;
        }

        var user = await GetUserForUpdateAsync(request.UserId, "user", cancellationToken);
        if (await TryHandleIdempotentAsync(normalizedIdempotencyKey, cancellationToken))
        {
            return true;
        }

        var balanceBefore = ResolveBalance(user, request.Currency);
        ApplyBalanceMutation(user, request);
        var balanceAfter = ResolveBalance(user, request.Currency);

        var ledgerEntry = CreateWalletLedgerEntry(new WalletLedgerEntryRequest(
            request.UserId,
            request.Currency,
            request.Type,
            ResolveLedgerAmount(request),
            balanceBefore,
            balanceAfter,
            request.ReferenceSource,
            request.ReferenceId,
            request.Description,
            request.MetadataJson,
            normalizedIdempotencyKey));

        _dbContext.Set<WalletTransaction>().Add(ledgerEntry);
        await _dbContext.SaveChangesAsync(cancellationToken);

        
        if (request.IsDebit)
        {
            await TrackSpendingToLeaderboardAsync(request.UserId, request.Currency, request.Amount, cancellationToken);
        }

        return false;
    }

    private static void ApplyBalanceMutation(User user, BalanceChangeRequest request)
    {
        if (request.IsDebit)
        {
            user.Debit(request.Currency, request.Amount);
            return;
        }

        user.Credit(request.Currency, request.Amount, request.Type);
    }

    private static long ResolveLedgerAmount(BalanceChangeRequest request)
        => request.IsDebit ? -request.Amount : request.Amount;

    private static long ResolveBalance(User user, string currency)
        => currency == CurrencyType.Gold ? user.GoldBalance : user.DiamondBalance;
}

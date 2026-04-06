using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class WalletRepository
{
    private async Task ExecuteReleaseAsync(ReleaseRequest request, CancellationToken cancellationToken)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(request.IdempotencyKey);

        try
        {
            await ExecuteWithTransactionAsync(async () =>
            {
                if (await TryHandleIdempotentAsync(normalizedIdempotencyKey, cancellationToken))
                {
                    return;
                }

                var payer = await GetUserForUpdateAsync(request.PayerId, "payer", cancellationToken);
                var receiver = await GetUserForUpdateAsync(request.ReceiverId, "receiver", cancellationToken);
                if (await TryHandleIdempotentAsync(normalizedIdempotencyKey, cancellationToken))
                {
                    return;
                }

                var entries = ApplyReleaseAndCreateEntries(request, normalizedIdempotencyKey, payer, receiver);
                _dbContext.Set<WalletTransaction>().AddRange(entries.PayerEntry, entries.ReceiverEntry);
                await _dbContext.SaveChangesAsync(cancellationToken);

                // -- Gamification: Ghi nhận điểm cho người trả tiền khi hoàn tất giải ngân --
                await TrackSpendingToLeaderboardAsync(request.PayerId, CurrencyType.Diamond, request.Amount, cancellationToken);
            }, cancellationToken);
        }
        catch (DbUpdateException exception) when (IsIdempotencyUniqueViolation(exception, normalizedIdempotencyKey))
        {
            LogIdempotencyAlreadyHandled(exception, request.OperationName, request.PayerId, normalizedIdempotencyKey);
        }
    }

    private static (WalletTransaction PayerEntry, WalletTransaction ReceiverEntry) ApplyReleaseAndCreateEntries(
        ReleaseRequest request,
        string? normalizedIdempotencyKey,
        User payer,
        User receiver)
    {
        var payerBalanceBefore = payer.DiamondBalance;
        payer.ReleaseFrozenDiamond(request.Amount);
        var payerBalanceAfter = payer.DiamondBalance;

        var receiverBalanceBefore = receiver.DiamondBalance;
        receiver.Credit(CurrencyType.Diamond, request.Amount, TransactionType.EscrowRelease);
        var receiverBalanceAfter = receiver.DiamondBalance;

        var payerEntry = CreateWalletLedgerEntry(new WalletLedgerEntryRequest(
            request.PayerId,
            CurrencyType.Diamond,
            TransactionType.EscrowRelease,
            -request.Amount,
            payerBalanceBefore,
            payerBalanceAfter,
            request.ReferenceSource,
            request.ReferenceId,
            request.Description,
            request.MetadataJson,
            normalizedIdempotencyKey));

        var receiverEntry = CreateWalletLedgerEntry(new WalletLedgerEntryRequest(
            request.ReceiverId,
            CurrencyType.Diamond,
            TransactionType.EscrowRelease,
            request.Amount,
            receiverBalanceBefore,
            receiverBalanceAfter,
            request.ReferenceSource,
            request.ReferenceId,
            request.Description,
            request.MetadataJson,
            BuildReceiverIdempotencyKey(normalizedIdempotencyKey)));

        return (payerEntry, receiverEntry);
    }

    private static string? BuildReceiverIdempotencyKey(string? idempotencyKey)
        => idempotencyKey == null ? null : $"{idempotencyKey}_receiver";
}

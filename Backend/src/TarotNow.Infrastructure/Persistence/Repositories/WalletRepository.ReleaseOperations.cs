using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý release escrow chi tiết.
public partial class WalletRepository
{
    /// <summary>
    /// Thực thi release escrow trong transaction.
    /// Luồng xử lý: kiểm tra idempotency, khóa payer/receiver, áp dụng release + ghi 2 ledger entries, rồi track spending.
    /// </summary>
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
                    // Request đã xử lý trước đó nên thoát sớm theo nguyên tắc idempotent.
                }

                var lockedUsers = await GetUsersForUpdatePairAsync(
                    request.PayerId,
                    request.ReceiverId,
                    cancellationToken);
                var payer = ResolveLockedUserOrThrow(lockedUsers, request.PayerId, "payer");
                var receiver = ResolveLockedUserOrThrow(lockedUsers, request.ReceiverId, "receiver");
                if (await TryHandleIdempotentAsync(normalizedIdempotencyKey, cancellationToken))
                {
                    return;
                    // Double-check sau lock để chặn race giữa các tiến trình release đồng thời.
                }

                var entries = ApplyReleaseAndCreateEntries(request, normalizedIdempotencyKey, payer, receiver);
                _dbContext.Set<WalletTransaction>().AddRange(entries.PayerEntry, entries.ReceiverEntry);
                await _dbContext.SaveChangesAsync(cancellationToken);
                // Ghi đồng thời cả hai ledger entries để đảm bảo cân bằng bút toán.
            }, cancellationToken);
        }
        catch (DbUpdateException exception) when (IsIdempotencyUniqueViolation(exception, normalizedIdempotencyKey))
        {
            LogIdempotencyAlreadyHandled(exception, request.OperationName, request.PayerId, normalizedIdempotencyKey);
        }
    }

    /// <summary>
    /// Áp dụng thay đổi số dư khi release và tạo hai ledger entries tương ứng.
    /// Luồng xử lý: trừ frozen từ payer, cộng diamond cho receiver và dựng entry cho từng bên.
    /// </summary>
    private static (WalletTransaction PayerEntry, WalletTransaction ReceiverEntry) ApplyReleaseAndCreateEntries(
        ReleaseRequest request,
        string? normalizedIdempotencyKey,
        User payer,
        User receiver)
    {
        var payerBalanceBefore = payer.DiamondBalance;
        var payerFrozenBalanceBefore = payer.FrozenDiamondBalance;
        payer.ReleaseFrozenDiamond(request.Amount);
        var payerBalanceAfter = payer.DiamondBalance;
        var payerFrozenBalanceAfter = payer.FrozenDiamondBalance;

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
            payerBalanceBefore,
            payerBalanceAfter,
            payerFrozenBalanceBefore,
            payerFrozenBalanceAfter,
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
            receiverBalanceBefore,
            receiverBalanceAfter,
            0,
            0,
            request.ReferenceSource,
            request.ReferenceId,
            request.Description,
            request.MetadataJson,
            BuildReceiverIdempotencyKey(normalizedIdempotencyKey)));
        // Receiver entry dùng idempotency key hậu tố riêng để không đụng unique key với payer entry.

        return (payerEntry, receiverEntry);
    }

    /// <summary>
    /// Tạo idempotency key cho ledger của receiver.
    /// Luồng xử lý: thêm hậu tố _receiver khi key gốc tồn tại.
    /// </summary>
    private static string? BuildReceiverIdempotencyKey(string? idempotencyKey)
        => idempotencyKey == null ? null : $"{idempotencyKey}_receiver";
}

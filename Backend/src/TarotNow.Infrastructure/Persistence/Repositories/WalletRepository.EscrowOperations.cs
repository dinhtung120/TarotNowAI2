using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class WalletRepository
{
    private async Task ExecuteEscrowMutationAsync(
        EscrowMutationRequest request,
        Action<User, long> applyMutation,
        CancellationToken cancellationToken)
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

                var user = await GetUserForUpdateAsync(request.UserId, "user", cancellationToken);
                if (await TryHandleIdempotentAsync(normalizedIdempotencyKey, cancellationToken))
                {
                    return;
                }

                var balanceBefore = user.DiamondBalance;
                applyMutation(user, request.Amount);
                var balanceAfter = user.DiamondBalance;

                var ledgerEntry = CreateWalletLedgerEntry(new WalletLedgerEntryRequest(
                    request.UserId,
                    CurrencyType.Diamond,
                    request.TransactionType,
                    request.LedgerAmount,
                    balanceBefore,
                    balanceAfter,
                    request.ReferenceSource,
                    request.ReferenceId,
                    request.Description,
                    request.MetadataJson,
                    normalizedIdempotencyKey));

                _dbContext.Set<WalletTransaction>().Add(ledgerEntry);
                await _dbContext.SaveChangesAsync(cancellationToken);

                // -- Gamification: Ghi nhận điểm khi tiền bị "Tiêu thụ" (Consume) --
                if (request.TransactionType == TransactionType.EscrowRelease)
                {
                    // AI Reading hiện tại chủ yếu dùng Diamond, nhưng ta dùng currency từ ledgerEntry cho linh hoạt
                    await TrackSpendingToLeaderboardAsync(request.UserId, CurrencyType.Diamond, request.Amount, cancellationToken);
                }
            }, cancellationToken);
        }
        catch (DbUpdateException exception) when (IsIdempotencyUniqueViolation(exception, normalizedIdempotencyKey))
        {
            LogIdempotencyAlreadyHandled(exception, request.OperationName, request.UserId, normalizedIdempotencyKey);
        }
    }
}

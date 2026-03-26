using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class WalletRepository
{
    private async Task<bool> TryHandleIdempotentAsync(string? idempotencyKey, CancellationToken cancellationToken)
        => await ExistsByIdempotencyKeyAsync(idempotencyKey, cancellationToken);

    private async Task<User> GetUserForUpdateAsync(Guid userId, string role, CancellationToken cancellationToken)
    {
        var users = await _dbContext.Set<User>()
            .FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", userId)
            .ToListAsync(cancellationToken);

        return users.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy {role} {userId}");
    }

    private static WalletTransaction CreateWalletLedgerEntry(WalletLedgerEntryRequest request)
    {
        return WalletTransaction.Create(new WalletTransactionCreateRequest
        {
            UserId = request.UserId,
            Currency = request.Currency,
            Type = request.Type,
            Amount = request.Amount,
            BalanceBefore = request.BalanceBefore,
            BalanceAfter = request.BalanceAfter,
            ReferenceSource = request.ReferenceSource,
            ReferenceId = request.ReferenceId,
            Description = request.Description,
            MetadataJson = request.MetadataJson,
            IdempotencyKey = request.IdempotencyKey
        });
    }

    private void LogIdempotencyAlreadyHandled(
        DbUpdateException exception,
        string operationName,
        Guid principalId,
        string? idempotencyKey)
    {
        _logger.LogInformation(
            exception,
            "[WalletRepository] Idempotency key already handled for {Operation}. PrincipalId={PrincipalId}, Key={IdempotencyKey}",
            operationName,
            principalId,
            idempotencyKey);
    }
}

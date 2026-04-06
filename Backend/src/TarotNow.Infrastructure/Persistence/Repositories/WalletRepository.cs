using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class WalletRepository : IWalletRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<WalletRepository> _logger;
    private readonly ILeaderboardRepository _lbRepo;

    public WalletRepository(
        ApplicationDbContext dbContext,
        ILogger<WalletRepository> logger,
        ILeaderboardRepository lbRepo)
    {
        _dbContext = dbContext;
        _logger = logger;
        _lbRepo = lbRepo;
    }

    private async Task ExecuteWithTransactionAsync(Func<Task> action, CancellationToken cancellationToken)
    {
        if (_dbContext.Database.CurrentTransaction != null)
        {
            await action();
            return;
        }

        var strategy = _dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(
                System.Data.IsolationLevel.ReadCommitted,
                cancellationToken);

            await action();
            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static string? NormalizeIdempotencyKey(string? idempotencyKey)
        => string.IsNullOrWhiteSpace(idempotencyKey) ? null : idempotencyKey.Trim();

    private async Task<bool> ExistsByIdempotencyKeyAsync(string? idempotencyKey, CancellationToken cancellationToken)
    {
        if (idempotencyKey == null)
        {
            return false;
        }

        return await _dbContext.Set<WalletTransaction>()
            .AnyAsync(x => x.IdempotencyKey == idempotencyKey, cancellationToken);
    }

    private static bool IsIdempotencyUniqueViolation(DbUpdateException exception, string? idempotencyKey)
    {
        if (idempotencyKey == null)
        {
            return false;
        }

        return exception.InnerException is PostgresException postgresException
               && postgresException.SqlState == PostgresErrorCodes.UniqueViolation
               && string.Equals(
                   postgresException.ConstraintName,
                   "ix_wallet_transactions_idempotency_key",
                   StringComparison.OrdinalIgnoreCase);
    }
}

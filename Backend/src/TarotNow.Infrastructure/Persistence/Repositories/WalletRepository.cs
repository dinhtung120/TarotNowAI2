using Microsoft.EntityFrameworkCore;
using Npgsql;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly ApplicationDbContext _dbContext;

    public WalletRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private async Task ExecuteWithTransactionAsync(Func<Task> action, CancellationToken cancellationToken)
    {
        if (_dbContext.Database.CurrentTransaction != null)
        {
            // Nếu đã có transaction từ trước (vd: được gọi từ ReadingSession), tái sử dụng nó
            await action();
            return;
        }

        // Nếu chưa có, tạo transaction mới
        var strategy = _dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);
            await action();
            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static string? NormalizeIdempotencyKey(string? idempotencyKey)
        => string.IsNullOrWhiteSpace(idempotencyKey) ? null : idempotencyKey.Trim();

    private async Task<bool> ExistsByIdempotencyKeyAsync(string? idempotencyKey, CancellationToken cancellationToken)
    {
        if (idempotencyKey == null) return false;
        return await _dbContext.Set<WalletTransaction>().AnyAsync(t => t.IdempotencyKey == idempotencyKey, cancellationToken);
    }

    private static bool IsIdempotencyUniqueViolation(DbUpdateException exception, string? idempotencyKey)
    {
        if (idempotencyKey == null) return false;

        return exception.InnerException is PostgresException postgresException
               && postgresException.SqlState == PostgresErrorCodes.UniqueViolation
               && string.Equals(postgresException.ConstraintName, "ix_wallet_transactions_idempotency_key", StringComparison.OrdinalIgnoreCase);
    }

    public async Task CreditAsync(Guid userId, string currency, string type, long amount, string? referenceSource = null, string? referenceId = null, string? description = null, string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(idempotencyKey);

        try
        {
            await ExecuteWithTransactionAsync(async () =>
            {
                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                var users = await _dbContext.Set<User>().FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", userId).ToListAsync(cancellationToken);
                var user = users.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy user {userId}");

                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                long balanceBefore = currency == CurrencyType.Gold ? user.GoldBalance : user.DiamondBalance;

                user.Credit(currency, amount, type);

                long balanceAfter = currency == CurrencyType.Gold ? user.GoldBalance : user.DiamondBalance;

                var ledgerEntry = WalletTransaction.Create(
                    userId: userId,
                    currency: currency,
                    type: type,
                    amount: amount,
                    balanceBefore: balanceBefore,
                    balanceAfter: balanceAfter,
                    referenceSource: referenceSource,
                    referenceId: referenceId,
                    description: description,
                    metadataJson: metadataJson,
                    idempotencyKey: normalizedIdempotencyKey
                );

                _dbContext.Set<WalletTransaction>().Add(ledgerEntry);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsIdempotencyUniqueViolation(ex, normalizedIdempotencyKey))
        {
            // Concurrent duplicate request: unique index bảo vệ idempotency, coi như đã xử lý.
        }
    }

    public async Task DebitAsync(Guid userId, string currency, string type, long amount, string? referenceSource = null, string? referenceId = null, string? description = null, string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(idempotencyKey);

        try
        {
            await ExecuteWithTransactionAsync(async () =>
            {
                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                var users = await _dbContext.Set<User>().FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", userId).ToListAsync(cancellationToken);
                var user = users.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy user {userId}");

                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                long balanceBefore = currency == CurrencyType.Gold ? user.GoldBalance : user.DiamondBalance;

                user.Debit(currency, amount);

                long balanceAfter = currency == CurrencyType.Gold ? user.GoldBalance : user.DiamondBalance;

                var ledgerEntry = WalletTransaction.Create(
                    userId: userId,
                    currency: currency,
                    type: type,
                    amount: -amount,
                    balanceBefore: balanceBefore,
                    balanceAfter: balanceAfter,
                    referenceSource: referenceSource,
                    referenceId: referenceId,
                    description: description,
                    metadataJson: metadataJson,
                    idempotencyKey: normalizedIdempotencyKey
                );

                _dbContext.Set<WalletTransaction>().Add(ledgerEntry);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsIdempotencyUniqueViolation(ex, normalizedIdempotencyKey))
        {
            // Concurrent duplicate request: unique index bảo vệ idempotency, coi như đã xử lý.
        }
    }

    public async Task FreezeAsync(Guid userId, long amount, string? referenceSource = null, string? referenceId = null, string? description = null, string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(idempotencyKey);

        try
        {
            await ExecuteWithTransactionAsync(async () =>
            {
                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                var users = await _dbContext.Set<User>().FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", userId).ToListAsync(cancellationToken);
                var user = users.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy user {userId}");

                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                long balanceBefore = user.DiamondBalance;

                user.FreezeDiamond(amount);

                long balanceAfter = user.DiamondBalance;

                var ledgerEntry = WalletTransaction.Create(
                    userId: userId,
                    currency: CurrencyType.Diamond,
                    type: TransactionType.EscrowFreeze,
                    amount: -amount,
                    balanceBefore: balanceBefore,
                    balanceAfter: balanceAfter,
                    referenceSource: referenceSource,
                    referenceId: referenceId,
                    description: description,
                    metadataJson: metadataJson,
                    idempotencyKey: normalizedIdempotencyKey
                );

                _dbContext.Set<WalletTransaction>().Add(ledgerEntry);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsIdempotencyUniqueViolation(ex, normalizedIdempotencyKey))
        {
            // Concurrent duplicate request: unique index bảo vệ idempotency, coi như đã xử lý.
        }
    }

    public async Task ReleaseAsync(Guid payerId, Guid receiverId, long amount, string? referenceSource = null, string? referenceId = null, string? description = null, string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(idempotencyKey);
        var receiverIdempotencyKey = normalizedIdempotencyKey == null ? null : $"{normalizedIdempotencyKey}_receiver";

        try
        {
            await ExecuteWithTransactionAsync(async () =>
            {
                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                var payers = await _dbContext.Set<User>().FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", payerId).ToListAsync(cancellationToken);
                var payer = payers.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy payer {payerId}");

                var receivers = await _dbContext.Set<User>().FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", receiverId).ToListAsync(cancellationToken);
                var receiver = receivers.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy receiver {receiverId}");

                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                long payerBalanceBefore = payer.DiamondBalance;
                payer.ReleaseFrozenDiamond(amount);
                long payerBalanceAfter = payer.DiamondBalance;

                long receiverBalanceBefore = receiver.DiamondBalance;
                receiver.Credit(CurrencyType.Diamond, amount, TransactionType.EscrowRelease);
                long receiverBalanceAfter = receiver.DiamondBalance;

                var ledgerEntryPayer = WalletTransaction.Create(
                    userId: payerId,
                    currency: CurrencyType.Diamond,
                    type: TransactionType.EscrowRelease,
                    amount: -amount,
                    balanceBefore: payerBalanceBefore,
                    balanceAfter: payerBalanceAfter,
                    referenceSource: referenceSource,
                    referenceId: referenceId,
                    description: description,
                    metadataJson: metadataJson,
                    idempotencyKey: normalizedIdempotencyKey
                );

                var ledgerEntryReceiver = WalletTransaction.Create(
                    userId: receiverId,
                    currency: CurrencyType.Diamond,
                    type: TransactionType.EscrowRelease,
                    amount: amount,
                    balanceBefore: receiverBalanceBefore,
                    balanceAfter: receiverBalanceAfter,
                    referenceSource: referenceSource,
                    referenceId: referenceId,
                    description: description,
                    metadataJson: metadataJson,
                    idempotencyKey: receiverIdempotencyKey
                );

                _dbContext.Set<WalletTransaction>().AddRange(ledgerEntryPayer, ledgerEntryReceiver);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsIdempotencyUniqueViolation(ex, normalizedIdempotencyKey))
        {
            // Concurrent duplicate request: unique index bảo vệ idempotency, coi như đã xử lý.
        }
    }

    public async Task RefundAsync(Guid userId, long amount, string? referenceSource = null, string? referenceId = null, string? description = null, string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(idempotencyKey);

        try
        {
            await ExecuteWithTransactionAsync(async () =>
            {
                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                var users = await _dbContext.Set<User>().FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", userId).ToListAsync(cancellationToken);
                var user = users.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy user {userId}");

                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                long balanceBefore = user.DiamondBalance;

                user.RefundFrozenDiamond(amount);

                long balanceAfter = user.DiamondBalance;

                var ledgerEntry = WalletTransaction.Create(
                    userId: userId,
                    currency: CurrencyType.Diamond,
                    type: TransactionType.EscrowRefund,
                    amount: amount, // Cộng lại vào Available
                    balanceBefore: balanceBefore,
                    balanceAfter: balanceAfter,
                    referenceSource: referenceSource,
                    referenceId: referenceId,
                    description: description,
                    metadataJson: metadataJson,
                    idempotencyKey: normalizedIdempotencyKey
                );

                _dbContext.Set<WalletTransaction>().Add(ledgerEntry);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsIdempotencyUniqueViolation(ex, normalizedIdempotencyKey))
        {
            // Concurrent duplicate request: unique index bảo vệ idempotency, coi như đã xử lý.
        }
    }

    /// <summary>
    /// Tiêu thụ Diamond đã đóng băng — trừ khỏi frozen balance.
    /// Dùng khi dịch vụ hoàn tất thành công (VD: AI stream completed).
    /// Thay thế ReleaseAsync (cần receiver account) vì hệ thống chưa có System Master Account.
    /// Transaction đảm bảo atomicity: SELECT FOR UPDATE → consume → ghi ledger.
    /// </summary>
    public async Task ConsumeAsync(Guid userId, long amount, string? referenceSource = null, string? referenceId = null, string? description = null, string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(idempotencyKey);

        try
        {
            await ExecuteWithTransactionAsync(async () =>
            {
                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                // Row-level lock tránh race condition khi nhiều request đồng thời
                var users = await _dbContext.Set<User>().FromSqlRaw("SELECT * FROM users WHERE id = {0} FOR UPDATE", userId).ToListAsync(cancellationToken);
                var user = users.FirstOrDefault() ?? throw new InvalidOperationException($"Không tìm thấy user {userId}");

                if (await ExistsByIdempotencyKeyAsync(normalizedIdempotencyKey, cancellationToken)) return;

                long balanceBefore = user.DiamondBalance;

                // Trừ Diamond khỏi frozen balance (consume = "đốt" → không cộng cho ai)
                user.ConsumeFrozenDiamond(amount);

                long balanceAfter = user.DiamondBalance;

                var ledgerEntry = WalletTransaction.Create(
                    userId: userId,
                    currency: CurrencyType.Diamond,
                    type: TransactionType.EscrowRelease, // Vẫn dùng type EscrowRelease cho audit trail
                    amount: -amount,
                    balanceBefore: balanceBefore,
                    balanceAfter: balanceAfter,
                    referenceSource: referenceSource,
                    referenceId: referenceId,
                    description: description,
                    metadataJson: metadataJson,
                    idempotencyKey: normalizedIdempotencyKey
                );

                _dbContext.Set<WalletTransaction>().Add(ledgerEntry);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsIdempotencyUniqueViolation(ex, normalizedIdempotencyKey))
        {
            // Concurrent duplicate request: unique index bảo vệ idempotency, coi như đã xử lý.
        }
    }
}

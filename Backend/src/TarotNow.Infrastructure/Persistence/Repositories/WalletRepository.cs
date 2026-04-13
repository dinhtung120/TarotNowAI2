using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository chính cho mọi thao tác ví tài chính.
public partial class WalletRepository : IWalletRepository
{
    // DbContext dùng cho giao dịch tài chính.
    private readonly ApplicationDbContext _dbContext;
    // Logger phục vụ audit và chẩn đoán lỗi.
    private readonly ILogger<WalletRepository> _logger;

    /// <summary>
    /// Khởi tạo WalletRepository.
    /// Luồng xử lý: nhận các dependency bắt buộc cho transaction tài chính và side-effect leaderboard.
    /// </summary>
    public WalletRepository(
        ApplicationDbContext dbContext,
        ILogger<WalletRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Thực thi action trong transaction khi cần.
    /// Luồng xử lý: nếu đã có transaction hiện tại thì chạy trực tiếp; nếu chưa có thì tạo execution strategy + transaction mới.
    /// </summary>
    private async Task ExecuteWithTransactionAsync(Func<Task> action, CancellationToken cancellationToken)
    {
        if (_dbContext.Database.CurrentTransaction != null)
        {
            await action();
            return;
            // Reuse transaction cha để tránh nested transaction không cần thiết.
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
        // Execution strategy giúp retry an toàn các lỗi transient ở tầng DB.
    }

    /// <summary>
    /// Chuẩn hóa idempotency key đầu vào.
    /// Luồng xử lý: key rỗng/trắng trả null, còn lại trim.
    /// </summary>
    private static string? NormalizeIdempotencyKey(string? idempotencyKey)
        => string.IsNullOrWhiteSpace(idempotencyKey) ? null : idempotencyKey.Trim();

    /// <summary>
    /// Kiểm tra đã tồn tại ledger với idempotency key này chưa.
    /// Luồng xử lý: trả false ngay nếu key null, ngược lại AnyAsync trên wallet_transactions.
    /// </summary>
    private async Task<bool> ExistsByIdempotencyKeyAsync(string? idempotencyKey, CancellationToken cancellationToken)
    {
        if (idempotencyKey == null)
        {
            return false;
        }

        return await _dbContext.Set<WalletTransaction>()
            .AnyAsync(x => x.IdempotencyKey == idempotencyKey, cancellationToken);
    }

    /// <summary>
    /// Xác định DbUpdateException có phải lỗi unique idempotency key hay không.
    /// Luồng xử lý: kiểm tra Postgres unique violation và đối chiếu đúng constraint ix_wallet_transactions_idempotency_key.
    /// </summary>
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

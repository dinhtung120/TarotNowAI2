

using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence;

// Điều phối thực thi action trong transaction ứng dụng.
public class TransactionCoordinator : ITransactionCoordinator
{
    // DbContext dùng để tạo/reuse transaction.
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Khởi tạo transaction coordinator.
    /// Luồng xử lý: nhận DbContext từ DI để điều phối transaction trên cùng connection.
    /// </summary>
    public TransactionCoordinator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Thực thi action trong transaction với execution strategy.
    /// Luồng xử lý: nếu đã có transaction hiện tại thì chạy trực tiếp; nếu chưa có thì tạo strategy + transaction ReadCommitted.
    /// </summary>
    public async Task ExecuteAsync(
        Func<CancellationToken, Task> action,
        CancellationToken cancellationToken = default)
    {
        if (_dbContext.Database.CurrentTransaction != null)
        {
            await action(cancellationToken);
            return;
            // Reuse transaction cha để tránh lồng transaction không cần thiết.
        }

        var strategy = _dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database
                .BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);

            await action(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            // Commit chỉ khi action hoàn tất, lỗi sẽ rollback theo cơ chế transaction.
        });
    }
}

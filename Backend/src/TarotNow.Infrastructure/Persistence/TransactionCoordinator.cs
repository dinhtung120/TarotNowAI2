/*
 * FILE: TransactionCoordinator.cs
 * MỤC ĐÍCH: Điều phối transaction (Unit-of-Work pattern) qua nhiều repository.
 *   Cho phép nhiều repository cùng chia sẻ 1 database transaction.
 *
 *   TẠI SAO CẦN TransactionCoordinator?
 *   → Các Command handler thường gọi nhiều repository (vd: WalletRepo + UserRepo).
 *   → Nếu mỗi repo tự tạo transaction riêng → không đảm bảo atomicity.
 *   → TransactionCoordinator bao bọc tất cả operations trong 1 transaction duy nhất.
 *
 *   NESTED TRANSACTION:
 *   → Nếu đã có transaction (vd: được gọi từ nested handler) → tái sử dụng, không tạo mới.
 *   → Tránh lỗi "transaction already in progress".
 *
 *   EXECUTION STRATEGY:
 *   → CreateExecutionStrategy() → EF Core tự retry nếu connection tạm mất.
 *   → Isolation Level: ReadCommitted (đủ cho hầu hết trường hợp).
 */

using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence;

/// <summary>
/// Implement ITransactionCoordinator — quản lý transaction qua nhiều repository.
/// </summary>
public class TransactionCoordinator : ITransactionCoordinator
{
    private readonly ApplicationDbContext _dbContext;

    public TransactionCoordinator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Thực thi action trong 1 database transaction.
    /// Nếu đã có transaction (nested call) → tái sử dụng.
    /// Nếu chưa → tạo mới với ACID guarantees.
    /// </summary>
    public async Task ExecuteAsync(
        Func<CancellationToken, Task> action,
        CancellationToken cancellationToken = default)
    {
        // Kiểm tra: đã có transaction chưa? (có thể được gọi từ handler khác đã tạo transaction)
        if (_dbContext.Database.CurrentTransaction != null)
        {
            // Tái sử dụng transaction hiện tại → tránh lỗi nested
            await action(cancellationToken);
            return;
        }

        // Tạo transaction mới với retry strategy
        var strategy = _dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // ReadCommitted: đọc dữ liệu đã commit, tránh dirty read
            await using var transaction = await _dbContext.Database
                .BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);

            await action(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}

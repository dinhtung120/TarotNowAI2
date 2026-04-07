

using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence;

public class TransactionCoordinator : ITransactionCoordinator
{
    private readonly ApplicationDbContext _dbContext;

    public TransactionCoordinator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

        public async Task ExecuteAsync(
        Func<CancellationToken, Task> action,
        CancellationToken cancellationToken = default)
    {
        
        if (_dbContext.Database.CurrentTransaction != null)
        {
            
            await action(cancellationToken);
            return;
        }

        
        var strategy = _dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            
            await using var transaction = await _dbContext.Database
                .BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);

            await action(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

public interface IDepositOrderRepository
{
    Task<DepositOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DepositOrder?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DepositOrder?> GetByTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DepositOrder>> GetPendingOrdersAsync(TimeSpan olderThan, CancellationToken cancellationToken = default);
    Task<(IEnumerable<DepositOrder> Orders, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? status, CancellationToken cancellationToken = default);
    Task AddAsync(DepositOrder order, CancellationToken cancellationToken = default);
    Task UpdateAsync(DepositOrder order, CancellationToken cancellationToken = default);
}

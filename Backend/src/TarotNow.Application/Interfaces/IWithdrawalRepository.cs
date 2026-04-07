

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IWithdrawalRepository
{
        Task<WithdrawalRequest?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<bool> HasPendingRequestTodayAsync(Guid userId, DateOnly businessDate, CancellationToken ct = default);

        Task<List<WithdrawalRequest>> ListByUserAsync(Guid userId, int page, int pageSize, CancellationToken ct = default);

        Task<List<WithdrawalRequest>> ListPendingAsync(int page, int pageSize, CancellationToken ct = default);

        Task AddAsync(WithdrawalRequest request, CancellationToken ct = default);

        Task UpdateAsync(WithdrawalRequest request, CancellationToken ct = default);

        Task SaveChangesAsync(CancellationToken ct = default);
}

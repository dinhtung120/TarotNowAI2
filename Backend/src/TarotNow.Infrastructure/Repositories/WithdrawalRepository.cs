

using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.Repositories;

public class WithdrawalRepository : IWithdrawalRepository
{
    private readonly ApplicationDbContext _db;

    public WithdrawalRepository(ApplicationDbContext db) => _db = db;

        public async Task<WithdrawalRequest?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.WithdrawalRequests.FindAsync(new object[] { id }, ct);

        public async Task<bool> HasPendingRequestTodayAsync(Guid userId, DateOnly businessDate, CancellationToken ct = default)
        => await _db.WithdrawalRequests
            .AnyAsync(r => r.UserId == userId
                        && r.BusinessDateUtc == businessDate
                        && r.Status != "rejected"
                        && r.Status != "paid", ct);

        public async Task<List<WithdrawalRequest>> ListByUserAsync(Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        return await _db.WithdrawalRequests
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt) 
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ToListAsync(ct);
    }

        public async Task<List<WithdrawalRequest>> ListPendingAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        return await _db.WithdrawalRequests
            .Where(r => r.Status == "pending")
            .OrderBy(r => r.CreatedAt) 
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ToListAsync(ct);
    }

        public async Task AddAsync(WithdrawalRequest request, CancellationToken ct = default)
        => await _db.WithdrawalRequests.AddAsync(request, ct);

        public Task UpdateAsync(WithdrawalRequest request, CancellationToken ct = default)
    {
        _db.WithdrawalRequests.Update(request);
        return Task.CompletedTask;
    }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _db.SaveChangesAsync(ct);
}

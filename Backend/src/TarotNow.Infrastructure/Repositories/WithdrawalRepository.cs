using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation cho IWithdrawalRepository.
/// Quản lý withdrawal_requests (PostgreSQL).
/// </summary>
public class WithdrawalRepository : IWithdrawalRepository
{
    private readonly ApplicationDbContext _db;

    public WithdrawalRepository(ApplicationDbContext db) => _db = db;

    public async Task<WithdrawalRequest?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.WithdrawalRequests.FindAsync(new object[] { id }, ct);

    /// <summary>
    /// Kiểm tra đã có request pending/approved cùng ngày chưa.
    /// Sử dụng unique index idx_withdrawal_one_per_day (loại trừ rejected/paid).
    /// </summary>
    public async Task<bool> HasPendingRequestTodayAsync(Guid userId, DateOnly businessDate, CancellationToken ct = default)
        => await _db.WithdrawalRequests
            .AnyAsync(r => r.UserId == userId
                        && r.BusinessDateUtc == businessDate
                        && r.Status != "rejected"
                        && r.Status != "paid", ct);

    public async Task<List<WithdrawalRequest>> ListByUserAsync(Guid userId, int page, int pageSize, CancellationToken ct = default)
        => await _db.WithdrawalRequests
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    /// <summary>Admin queue: pending requests mới nhất trước.</summary>
    public async Task<List<WithdrawalRequest>> ListPendingAsync(int page, int pageSize, CancellationToken ct = default)
        => await _db.WithdrawalRequests
            .Where(r => r.Status == "pending")
            .OrderBy(r => r.CreatedAt) // FIFO — cũ nhất trước
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

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

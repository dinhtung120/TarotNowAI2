using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class DepositOrderRepository : IDepositOrderRepository
{
    private readonly ApplicationDbContext _context;

    public DepositOrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DepositOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DepositOrders
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<DepositOrder?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var orders = await _context.DepositOrders
            .FromSqlRaw("SELECT * FROM deposit_orders WHERE id = {0} FOR UPDATE", id)
            .ToListAsync(cancellationToken);

        return orders.FirstOrDefault();
    }

    public async Task<DepositOrder?> GetByTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default)
    {
        return await _context.DepositOrders
            .FirstOrDefaultAsync(o => o.TransactionId == transactionId, cancellationToken);
    }

    public async Task<IEnumerable<DepositOrder>> GetPendingOrdersAsync(TimeSpan olderThan, CancellationToken cancellationToken = default)
    {
        var threshold = DateTime.UtcNow.Subtract(olderThan);
        return await _context.DepositOrders
            .Where(o => o.Status == "Pending" && o.CreatedAt <= threshold)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<DepositOrder> Orders, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? status, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        var query = _context.DepositOrders.AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(o => o.Status == status);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var orders = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ToListAsync(cancellationToken);

        return (orders, totalCount);
    }

    public async Task AddAsync(DepositOrder order, CancellationToken cancellationToken = default)
    {
        await _context.DepositOrders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(DepositOrder order, CancellationToken cancellationToken = default)
    {
        _context.DepositOrders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

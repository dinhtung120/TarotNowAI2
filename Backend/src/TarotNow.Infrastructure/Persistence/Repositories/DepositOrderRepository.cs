using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository quản lý đơn nạp tiền qua cổng thanh toán.
public class DepositOrderRepository : IDepositOrderRepository
{
    // DbContext thao tác bảng deposit_orders.
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Khởi tạo repository đơn nạp.
    /// </summary>
    public DepositOrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy đơn nạp theo id.
    /// </summary>
    public async Task<DepositOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DepositOrders
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    /// <summary>
    /// Lấy đơn nạp theo id và owner user.
    /// </summary>
    public async Task<DepositOrder?> GetByIdForUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.DepositOrders
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId, cancellationToken);
    }

    /// <summary>
    /// Lấy đơn nạp theo id với khóa hàng để cập nhật an toàn.
    /// </summary>
    public async Task<DepositOrder?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var orders = await _context.DepositOrders
            .FromSqlRaw("SELECT * FROM deposit_orders WHERE id = {0} FOR UPDATE", id)
            .ToListAsync(cancellationToken);

        return orders.FirstOrDefault();
    }

    /// <summary>
    /// Lấy đơn nạp theo client request key.
    /// </summary>
    public async Task<DepositOrder?> GetByClientRequestKeyAsync(string clientRequestKey, CancellationToken cancellationToken = default)
    {
        return await _context.DepositOrders
            .FirstOrDefaultAsync(o => o.ClientRequestKey == clientRequestKey, cancellationToken);
    }

    /// <summary>
    /// Lấy đơn nạp theo PayOS order code.
    /// </summary>
    public async Task<DepositOrder?> GetByPayOsOrderCodeAsync(long payOsOrderCode, CancellationToken cancellationToken = default)
    {
        return await _context.DepositOrders
            .FirstOrDefaultAsync(o => o.PayOsOrderCode == payOsOrderCode, cancellationToken);
    }

    /// <summary>
    /// Lấy đơn nạp theo PayOS order code ở chế độ khóa hàng.
    /// </summary>
    public async Task<DepositOrder?> GetByPayOsOrderCodeForUpdateAsync(long payOsOrderCode, CancellationToken cancellationToken = default)
    {
        var orders = await _context.DepositOrders
            .FromSqlRaw("SELECT * FROM deposit_orders WHERE payos_order_code = {0} FOR UPDATE", payOsOrderCode)
            .ToListAsync(cancellationToken);

        return orders.FirstOrDefault();
    }

    /// <summary>
    /// Lấy các đơn pending quá thời gian ngưỡng.
    /// </summary>
    public async Task<IEnumerable<DepositOrder>> GetPendingOrdersAsync(TimeSpan olderThan, CancellationToken cancellationToken = default)
    {
        var threshold = DateTime.UtcNow.Subtract(olderThan);
        return await _context.DepositOrders
            .Where(o => o.Status == TarotNow.Domain.Enums.DepositOrderStatus.Pending && o.CreatedAt <= threshold)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Lấy danh sách đơn nạp phân trang có lọc trạng thái.
    /// </summary>
    public async Task<(IEnumerable<DepositOrder> Orders, int TotalCount)> GetPaginatedAsync(
        int page,
        int pageSize,
        string? status,
        CancellationToken cancellationToken = default)
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

    /// <summary>
    /// Thêm mới đơn nạp.
    /// </summary>
    public async Task AddAsync(DepositOrder order, CancellationToken cancellationToken = default)
    {
        await _context.DepositOrders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Cập nhật đơn nạp hiện có.
    /// </summary>
    public async Task UpdateAsync(DepositOrder order, CancellationToken cancellationToken = default)
    {
        _context.DepositOrders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

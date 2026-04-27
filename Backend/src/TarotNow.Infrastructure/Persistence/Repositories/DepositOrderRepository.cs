using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository quản lý đơn nạp tiền qua cổng thanh toán.
public partial class DepositOrderRepository : IDepositOrderRepository
{
    private const string PayOsOrderCodeSequenceName = "deposit_order_code_seq";
    // DbContext thao tác bảng deposit_orders.
    private readonly ApplicationDbContext _context;
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary>
    /// Khởi tạo repository đơn nạp.
    /// </summary>
    public DepositOrderRepository(
        ApplicationDbContext context,
        IServiceScopeFactory scopeFactory)
    {
        _context = context;
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// Khóa transaction theo client request key để tránh tạo trùng order khi request đồng thời.
    /// </summary>
    public async Task AcquireCreateOrderLockAsync(
        string clientRequestKey,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(clientRequestKey))
        {
            throw new ArgumentException("Client request key is required.", nameof(clientRequestKey));
        }

        if (_context.Database.CurrentTransaction == null)
        {
            throw new InvalidOperationException("AcquireCreateOrderLockAsync requires an active transaction.");
        }

        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT pg_advisory_xact_lock(hashtext({clientRequestKey.Trim()}))",
            cancellationToken);
    }

    /// <summary>
    /// Lấy PayOS order code tiếp theo bằng DB sequence để tránh collision đa instance.
    /// </summary>
    public async Task<long> GetNextPayOsOrderCodeAsync(CancellationToken cancellationToken = default)
    {
        if (_context.Database.ProviderName?.Contains("Npgsql", StringComparison.OrdinalIgnoreCase) == true)
        {
            return await _context.Database
                .SqlQueryRaw<long>($"SELECT nextval('{PayOsOrderCodeSequenceName}'::regclass) AS \"Value\"")
                .SingleAsync(cancellationToken);
        }

        var baseCode = checked(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1000L);
        var candidate = baseCode + Random.Shared.Next(0, 1000);
        while (await _context.DepositOrders.AnyAsync(order => order.PayOsOrderCode == candidate, cancellationToken))
        {
            candidate += 1;
        }

        return candidate;
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
        var normalizedPage = NormalizePage(page);
        var normalizedPageSize = NormalizePageSize(pageSize, 200, 20);

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
    /// Lấy danh sách đơn nạp của một user theo phân trang.
    /// </summary>
    public async Task<(IReadOnlyCollection<DepositOrder> Orders, int TotalCount)> GetPaginatedByUserAsync(
        Guid userId,
        int page,
        int pageSize,
        string? status,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = NormalizePage(page);
        var normalizedPageSize = NormalizePageSize(pageSize, 50, 10);
        var query = _context.DepositOrders.Where(order => order.UserId == userId);

        if (!string.IsNullOrWhiteSpace(status))
        {
            var normalizedStatus = status.Trim();
            query = query.Where(order => order.Status == normalizedStatus);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var orders = await query
            .OrderByDescending(order => order.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ToArrayAsync(cancellationToken);

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

    private static int NormalizePage(int page)
    {
        return page < 1 ? 1 : page;
    }

    private static int NormalizePageSize(int pageSize, int maxPageSize, int defaultPageSize)
    {
        if (pageSize <= 0)
        {
            return defaultPageSize;
        }

        return Math.Min(pageSize, maxPageSize);
    }
}

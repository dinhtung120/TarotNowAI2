

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository quản lý đơn nạp tiền qua cổng thanh toán.
public class DepositOrderRepository : IDepositOrderRepository
{
    // DbContext thao tác bảng deposit_orders.
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Khởi tạo repository đơn nạp.
    /// Luồng xử lý: nhận DbContext từ DI để dùng chung transaction với command handler.
    /// </summary>
    public DepositOrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy đơn nạp theo id.
    /// Luồng xử lý: truy vấn bản ghi đầu tiên khớp id hoặc null nếu không có.
    /// </summary>
    public async Task<DepositOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DepositOrders
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    /// <summary>
    /// Lấy đơn nạp theo id với khóa hàng để cập nhật an toàn.
    /// Luồng xử lý: dùng câu SQL FOR UPDATE để chống race-condition khi webhook và admin xử lý đồng thời.
    /// </summary>
    public async Task<DepositOrder?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var orders = await _context.DepositOrders
            .FromSqlRaw("SELECT * FROM deposit_orders WHERE id = {0} FOR UPDATE", id)
            .ToListAsync(cancellationToken);
        // Dùng raw SQL vì EF chưa biểu diễn trực tiếp lock FOR UPDATE trong LINQ chuẩn.

        return orders.FirstOrDefault();
    }

    /// <summary>
    /// Lấy đơn nạp theo transaction id từ cổng thanh toán.
    /// Luồng xử lý: truy vấn trực tiếp trường transaction_id để hỗ trợ idempotency webhook.
    /// </summary>
    public async Task<DepositOrder?> GetByTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default)
    {
        return await _context.DepositOrders
            .FirstOrDefaultAsync(o => o.TransactionId == transactionId, cancellationToken);
    }

    /// <summary>
    /// Lấy các đơn pending quá thời gian ngưỡng.
    /// Luồng xử lý: tính threshold từ hiện tại rồi lọc trạng thái Pending có created_at cũ hơn ngưỡng.
    /// </summary>
    public async Task<IEnumerable<DepositOrder>> GetPendingOrdersAsync(TimeSpan olderThan, CancellationToken cancellationToken = default)
    {
        var threshold = DateTime.UtcNow.Subtract(olderThan);
        return await _context.DepositOrders
            .Where(o => o.Status == "Pending" && o.CreatedAt <= threshold)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Lấy danh sách đơn nạp phân trang có lọc trạng thái.
    /// Luồng xử lý: chuẩn hóa page/pageSize, áp filter status nếu có, đếm tổng và lấy page theo created_at giảm dần.
    /// </summary>
    public async Task<(IEnumerable<DepositOrder> Orders, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? status, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);
        // Giới hạn pageSize tối đa để tránh truy vấn quá nặng ở API admin.

        var query = _context.DepositOrders.AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(o => o.Status == status);
            // Chỉ áp lọc khi caller gửi status hợp lệ.
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
    /// Luồng xử lý: add entity rồi save ngay để đảm bảo đơn tồn tại trước khi gọi cổng thanh toán.
    /// </summary>
    public async Task AddAsync(DepositOrder order, CancellationToken cancellationToken = default)
    {
        await _context.DepositOrders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Cập nhật đơn nạp hiện có.
    /// Luồng xử lý: mark entity modified và persist để đồng bộ trạng thái xử lý.
    /// </summary>
    public async Task UpdateAsync(DepositOrder order, CancellationToken cancellationToken = default)
    {
        _context.DepositOrders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

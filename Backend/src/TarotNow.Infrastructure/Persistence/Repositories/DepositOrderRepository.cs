/*
 * FILE: DepositOrderRepository.cs
 * MỤC ĐÍCH: Repository quản lý bảng deposit_orders (PostgreSQL).
 *   Đơn nạp tiền: User nạp VNĐ → nhận Diamond (kim cương).
 *
 *   CÁC CHỨC NĂNG CHÍNH:
 *   → GetByIdAsync: tìm đơn nạp theo ID
 *   → GetByIdForUpdateAsync: tìm đơn + KHÓA HÀNG (FOR UPDATE) tránh race condition
 *   → GetByTransactionIdAsync: tìm đơn theo mã giao dịch ngân hàng (chặn webhook trùng)
 *   → GetPendingOrdersAsync: lấy đơn Pending quá lâu (cleanup job)
 *   → GetPaginatedAsync: phân trang cho Admin xem danh sách đơn nạp
 *   → Add/Update: thêm mới và cập nhật đơn
 */

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IDepositOrderRepository — truy cập bảng deposit_orders (PostgreSQL).
/// </summary>
public class DepositOrderRepository : IDepositOrderRepository
{
    private readonly ApplicationDbContext _context;

    public DepositOrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>Tìm đơn nạp theo ID (đọc bình thường, không khóa hàng).</summary>
    public async Task<DepositOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DepositOrders
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    /// <summary>
    /// Tìm đơn nạp theo ID VÀ KHÓA HÀNG trong transaction (FOR UPDATE).
    /// 
    /// TẠI SAO CẦN "FOR UPDATE"?
    /// → Khi xử lý webhook từ MoMo/VNPay, có thể 2 webhook đến CÙNG LÚC cho 1 đơn.
    /// → "FOR UPDATE" khóa hàng dữ liệu → webhook thứ 2 phải ĐỢI webhook thứ 1 xong.
    /// → Tránh tình trạng: cộng Diamond 2 lần cho cùng 1 đơn nạp (double-spend).
    /// 
    /// Lưu ý: PHẢI gọi trong SaveChangesAsync() transaction, nếu không sẽ không có tác dụng.
    /// </summary>
    public async Task<DepositOrder?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Dùng FromSqlRaw vì EF Core không hỗ trợ "FOR UPDATE" qua LINQ
        var orders = await _context.DepositOrders
            .FromSqlRaw("SELECT * FROM deposit_orders WHERE id = {0} FOR UPDATE", id)
            .ToListAsync(cancellationToken);

        return orders.FirstOrDefault();
    }

    /// <summary>
    /// Tìm đơn nạp theo TransactionId (mã giao dịch từ payment gateway).
    /// Dùng để kiểm tra: webhook này đã xử lý chưa? (idempotency check)
    /// TransactionId có Unique Index → chỉ có 1 đơn duy nhất.
    /// </summary>
    public async Task<DepositOrder?> GetByTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default)
    {
        return await _context.DepositOrders
            .FirstOrDefaultAsync(o => o.TransactionId == transactionId, cancellationToken);
    }

    /// <summary>
    /// Lấy danh sách đơn nạp Pending QUÁ LÂU (lâu hơn olderThan).
    /// Cleanup job: đơn nạp Pending quá 30 phút mà không nhận webhook → đánh dấu Expired.
    /// Ví dụ: olderThan = 30 phút → lấy đơn Pending tạo trước 30 phút trở lên.
    /// </summary>
    public async Task<IEnumerable<DepositOrder>> GetPendingOrdersAsync(TimeSpan olderThan, CancellationToken cancellationToken = default)
    {
        var threshold = DateTime.UtcNow.Subtract(olderThan);
        return await _context.DepositOrders
            .Where(o => o.Status == "Pending" && o.CreatedAt <= threshold)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Phân trang danh sách đơn nạp — dùng cho Admin dashboard.
    /// Hỗ trợ filter theo status (Pending/Completed/Failed).
    /// Trả về: (danh sách đơn, tổng số đơn) — UI dùng totalCount để tính số trang.
    /// Giới hạn pageSize tối đa 200 để tránh query quá nặng.
    /// </summary>
    public async Task<(IEnumerable<DepositOrder> Orders, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? status, CancellationToken cancellationToken = default)
    {
        // Normalize: trang < 1 → trang 1, pageSize <= 0 → 20, pageSize > 200 → 200
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        var query = _context.DepositOrders.AsQueryable();

        // Filter theo status nếu có
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(o => o.Status == status);
        }

        // Đếm tổng số đơn khớp filter (cho pagination UI)
        var totalCount = await query.CountAsync(cancellationToken);

        // Lấy dữ liệu trang hiện tại, sắp xếp mới nhất trước
        var orders = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ToListAsync(cancellationToken);

        return (orders, totalCount);
    }

    /// <summary>Thêm đơn nạp mới vào DB.</summary>
    public async Task AddAsync(DepositOrder order, CancellationToken cancellationToken = default)
    {
        await _context.DepositOrders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Cập nhật đơn nạp (ví dụ: đổi status từ Pending → Completed).</summary>
    public async Task UpdateAsync(DepositOrder order, CancellationToken cancellationToken = default)
    {
        _context.DepositOrders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

/*
 * FILE: WithdrawalRepository.cs
 * MỤC ĐÍCH: Repository quản lý yêu cầu rút tiền (PostgreSQL).
 *   User (Reader) yêu cầu rút Diamond → Admin duyệt → chuyển tiền thật.
 *
 *   CÁC CHỨC NĂNG:
 *   → GetByIdAsync: lấy yêu cầu theo ID
 *   → HasPendingRequestTodayAsync: kiểm tra đã gửi yêu cầu hôm nay chưa (giới hạn 1/ngày)
 *   → ListByUserAsync: lịch sử rút tiền của User (phân trang)
 *   → ListPendingAsync: Admin queue — hàng đợi duyệt (FIFO: cũ nhất trước)
 *   → AddAsync / UpdateAsync: CRUD cơ bản
 *
 *   QUY TẮC KINH DOANH:
 *   → Mỗi User chỉ được gửi 1 yêu cầu rút/ngày (business_date_utc)
 *   → Admin duyệt FIFO (first in, first out — cũ nhất trước)
 *   → SaveChanges tách riêng — để TransactionCoordinator quản lý khi cần
 */

using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.Repositories;

/// <summary>
/// Implement IWithdrawalRepository — yêu cầu rút tiền (PostgreSQL).
/// </summary>
public class WithdrawalRepository : IWithdrawalRepository
{
    private readonly ApplicationDbContext _db;

    public WithdrawalRepository(ApplicationDbContext db) => _db = db;

    /// <summary>Lấy yêu cầu theo Primary Key.</summary>
    public async Task<WithdrawalRequest?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.WithdrawalRequests.FindAsync(new object[] { id }, ct);

    /// <summary>
    /// Kiểm tra User đã gửi yêu cầu rút tiền TRONG NGÀY chưa.
    /// Quy tắc: mỗi User chỉ được 1 yêu cầu/ngày (business_date_utc).
    /// Chỉ tính yêu cầu chưa bị rejected hoặc paid (loại trừ kết thúc).
    /// Database có unique index idx_withdrawal_one_per_day hỗ trợ thêm.
    /// </summary>
    public async Task<bool> HasPendingRequestTodayAsync(Guid userId, DateOnly businessDate, CancellationToken ct = default)
        => await _db.WithdrawalRequests
            .AnyAsync(r => r.UserId == userId
                        && r.BusinessDateUtc == businessDate
                        && r.Status != "rejected"
                        && r.Status != "paid", ct);

    /// <summary>
    /// Lịch sử rút tiền của User — phân trang, mới nhất trước.
    /// User dùng để theo dõi trạng thái các yêu cầu đã gửi.
    /// </summary>
    public async Task<List<WithdrawalRequest>> ListByUserAsync(Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        return await _db.WithdrawalRequests
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt) // Mới nhất trước
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Admin approval queue — hàng đợi duyệt yêu cầu rút tiền.
    /// FIFO: OrderBy CreatedAt (CŨ NHẤT trước) — xử lý công bằng theo thứ tự gửi.
    /// Khác với các danh sách khác (DESC) vì Admin cần ưu tiên yêu cầu đợi lâu nhất.
    /// </summary>
    public async Task<List<WithdrawalRequest>> ListPendingAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        return await _db.WithdrawalRequests
            .Where(r => r.Status == "pending")
            .OrderBy(r => r.CreatedAt) // FIFO — cũ nhất trước (công bằng)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ToListAsync(ct);
    }

    /// <summary>Tạo yêu cầu rút tiền mới (không SaveChanges — TransactionCoordinator quản lý).</summary>
    public async Task AddAsync(WithdrawalRequest request, CancellationToken ct = default)
        => await _db.WithdrawalRequests.AddAsync(request, ct);

    /// <summary>Cập nhật yêu cầu (Admin approve/reject — không SaveChanges).</summary>
    public Task UpdateAsync(WithdrawalRequest request, CancellationToken ct = default)
    {
        _db.WithdrawalRequests.Update(request);
        return Task.CompletedTask;
    }

    /// <summary>Lưu thay đổi — gọi khi KHÔNG dùng TransactionCoordinator.</summary>
    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _db.SaveChangesAsync(ct);
}

using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.Repositories;

// Repository xử lý truy cập dữ liệu yêu cầu rút tiền.
public class WithdrawalRepository : IWithdrawalRepository
{
    // DbContext dùng để đồng bộ trạng thái withdrawal trong cùng unit-of-work.
    private readonly ApplicationDbContext _db;

    /// <summary>
    /// Khởi tạo repository với DbContext hiện tại.
    /// Luồng này đảm bảo mọi thao tác withdrawal dùng chung transaction scope của request.
    /// </summary>
    public WithdrawalRepository(ApplicationDbContext db) => _db = db;

    /// <summary>
    /// Lấy yêu cầu rút tiền theo định danh.
    /// Luồng tra cứu trực tiếp theo khóa chính để tối ưu truy vấn detail.
    /// </summary>
    public async Task<WithdrawalRequest?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.WithdrawalRequests.FindAsync(new object[] { id }, ct);

    /// <summary>
    /// Lấy yêu cầu rút tiền theo định danh với lock FOR UPDATE.
    /// Luồng này dùng cho nhánh process để tránh race-condition khi nhiều admin thao tác đồng thời.
    /// </summary>
    public async Task<WithdrawalRequest?> GetByIdForUpdateAsync(Guid id, CancellationToken ct = default)
    {
        var records = await _db.WithdrawalRequests
            .FromSqlRaw("SELECT * FROM withdrawal_requests WHERE id = {0} FOR UPDATE", id)
            .ToListAsync(ct);

        return records.FirstOrDefault();
    }

    /// <summary>
    /// Kiểm tra user đã tạo yêu cầu rút trong tuần nghiệp vụ UTC hay chưa.
    /// Luồng này chặn user gửi nhiều hơn 1 request trong cùng tuần.
    /// </summary>
    public async Task<bool> HasAnyRequestInWeekAsync(Guid userId, DateOnly businessWeekStartUtc, CancellationToken ct = default)
        => await _db.WithdrawalRequests
            .AnyAsync(r => r.UserId == userId && r.BusinessWeekStartUtc == businessWeekStartUtc, ct);

    /// <summary>
    /// Lấy request theo process idempotency key.
    /// Luồng này dùng để hỗ trợ idempotent retry cho thao tác approve/reject.
    /// </summary>
    public Task<WithdrawalRequest?> GetByProcessIdempotencyKeyAsync(string processIdempotencyKey, CancellationToken ct = default)
        => _db.WithdrawalRequests.FirstOrDefaultAsync(
            r => r.ProcessIdempotencyKey == processIdempotencyKey,
            ct);

    /// <summary>
    /// Lấy lịch sử yêu cầu rút tiền của một người dùng theo phân trang.
    /// Luồng chuẩn hóa page/pageSize để tránh truy vấn ngoài biên và trả dữ liệu ổn định.
    /// </summary>
    public async Task<List<WithdrawalRequest>> ListByUserAsync(Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        // Chuẩn hóa đầu vào để bảo vệ truy vấn khỏi page âm hoặc pageSize quá lớn.
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        return await _db.WithdrawalRequests
            .Where(r => r.UserId == userId)
            // Trả mới nhất trước để màn hình lịch sử phản ánh đúng kỳ vọng người dùng.
            .OrderByDescending(r => r.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Lấy danh sách yêu cầu đang chờ xử lý cho màn hình vận hành.
    /// Luồng sắp xếp theo thời điểm tạo tăng dần để đội vận hành xử lý theo thứ tự hàng đợi.
    /// </summary>
    public async Task<List<WithdrawalRequest>> ListPendingAsync(int page, int pageSize, CancellationToken ct = default)
    {
        // Chuẩn hóa phân trang để tránh truy vấn quá tải khi tham số không hợp lệ.
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        return await _db.WithdrawalRequests
            .Where(r => r.Status == WithdrawalRequestStatus.Pending)
            .OrderBy(r => r.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Thêm yêu cầu rút tiền mới vào DbContext.
    /// Luồng tách add và commit để phối hợp với các bước nghiệp vụ liên quan ví.
    /// </summary>
    public async Task AddAsync(WithdrawalRequest request, CancellationToken ct = default)
        => await _db.WithdrawalRequests.AddAsync(request, ct);

    /// <summary>
    /// Đánh dấu yêu cầu rút tiền đã thay đổi để cập nhật.
    /// Luồng này phục vụ chuyển trạng thái pending, paid hoặc rejected.
    /// </summary>
    public Task UpdateAsync(WithdrawalRequest request, CancellationToken ct = default)
    {
        // Cập nhật state entity để SaveChanges lưu thay đổi vào database.
        _db.WithdrawalRequests.Update(request);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Commit toàn bộ thay đổi withdrawal trong phiên làm việc hiện tại.
    /// Luồng commit tập trung giúp trạng thái yêu cầu rút tiền nhất quán.
    /// </summary>
    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _db.SaveChangesAsync(ct);
}

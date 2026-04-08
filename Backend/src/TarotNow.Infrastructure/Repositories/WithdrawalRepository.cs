using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
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
    /// Kiểm tra người dùng đã có yêu cầu rút chưa hoàn tất trong ngày nghiệp vụ hay chưa.
    /// Luồng này chặn spam yêu cầu mới khi request cũ vẫn đang chờ xử lý.
    /// </summary>
    public async Task<bool> HasPendingRequestTodayAsync(Guid userId, DateOnly businessDate, CancellationToken ct = default)
        => await _db.WithdrawalRequests
            .AnyAsync(r => r.UserId == userId
                        && r.BusinessDateUtc == businessDate
                        // Chỉ xem là pending khi chưa bị từ chối và chưa chi trả.
                        && r.Status != "rejected"
                        && r.Status != "paid", ct);

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
            .Where(r => r.Status == "pending")
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



using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository truy vấn lịch sử biến động ví (ledger) của người dùng.
public class LedgerRepository : ILedgerRepository
{
    // DbContext truy cập bảng wallet_transactions.
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Khởi tạo repository ledger.
    /// Luồng xử lý: nhận DbContext từ DI để tái sử dụng context hiện tại.
    /// </summary>
    public LedgerRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Đếm tổng số giao dịch ví của một user.
    /// Luồng xử lý: lọc theo user_id và trả count để tính metadata phân trang.
    /// </summary>
    public async Task<int> GetTotalCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.WalletTransactions
            .Where(x => x.UserId == userId)
            .CountAsync(cancellationToken);
    }

    /// <summary>
    /// Lấy danh sách giao dịch ví theo trang.
    /// Luồng xử lý: chuẩn hóa page/limit, lọc theo user, sort created_at desc rồi áp skip/take.
    /// </summary>
    public async Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(Guid userId, int page, int limit, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedLimit = limit <= 0 ? 20 : Math.Min(limit, 200);
        // Chặn limit quá lớn để tránh truy vấn nặng khi client gửi sai tham số.

        return await _dbContext.WalletTransactions
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedLimit)
            .Take(normalizedLimit)
            .ToListAsync(cancellationToken);
    }
}

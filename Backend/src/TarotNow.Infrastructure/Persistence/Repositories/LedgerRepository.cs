/*
 * FILE: LedgerRepository.cs
 * MỤC ĐÍCH: Repository quản lý bảng wallet_transactions (PostgreSQL) — Sổ cái giao dịch.
 *   Cho phép User xem lịch sử giao dịch ví (nạp tiền, trừ tiền, hoàn tiền, v.v.).
 *
 *   CÁC CHỨC NĂNG:
 *   → GetTotalCountAsync: đếm tổng số giao dịch của User (cho pagination)
 *   → GetTransactionsAsync: lấy danh sách giao dịch có phân trang (mới nhất trước)
 *
 *   LƯU Ý: Đây chỉ là READ-ONLY repository.
 *   Việc THÊM transaction mới được xử lý trong các Command handler (ví dụ: DepositConfirm, AiRequest),
 *   vì mỗi giao dịch phải đi kèm với logic nghiệp vụ cụ thể (ACID, idempotency, v.v.).
 */

using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement ILedgerRepository — đọc dữ liệu sổ cái giao dịch ví.
/// </summary>
public class LedgerRepository : ILedgerRepository
{
    private readonly ApplicationDbContext _dbContext;

    public LedgerRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Đếm tổng số giao dịch của 1 User — dùng cho pagination trên UI.
    /// UI cần biết "có bao nhiêu trang" để hiển thị nút chuyển trang.
    /// </summary>
    public async Task<int> GetTotalCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.WalletTransactions
            .Where(x => x.UserId == userId)
            .CountAsync(cancellationToken);
    }

    /// <summary>
    /// Lấy danh sách giao dịch có phân trang, sắp xếp mới nhất trước.
    /// Normalize page/limit để tránh giá trị bất hợp lệ:
    ///   - page < 1 → page = 1
    ///   - limit ≤ 0 → limit = 20 (mặc định)
    ///   - limit > 200 → limit = 200 (giới hạn bảo vệ DB)
    /// </summary>
    public async Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(Guid userId, int page, int limit, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedLimit = limit <= 0 ? 20 : Math.Min(limit, 200);

        return await _dbContext.WalletTransactions
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedLimit)
            .Take(normalizedLimit)
            .ToListAsync(cancellationToken);
    }
}

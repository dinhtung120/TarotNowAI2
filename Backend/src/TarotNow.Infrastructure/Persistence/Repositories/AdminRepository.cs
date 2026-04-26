

using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository phục vụ các truy vấn quản trị liên quan đối soát dữ liệu.
public class AdminRepository : IAdminRepository
{
    // DbContext quan hệ dùng cho truy vấn SQL/EF trong module admin.
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Khởi tạo repository admin.
    /// Luồng xử lý: nhận ApplicationDbContext từ DI để tái sử dụng transaction và connection hiện có.
    /// </summary>
    public AdminRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Lấy danh sách chênh lệch giữa số dư user và số dư tổng hợp từ ledger.
    /// Luồng xử lý: chạy SQL đối soát trực tiếp trên view ledger, chỉ trả các bản ghi có mismatch thực sự.
    /// </summary>
    public async Task<List<MismatchRecord>> GetLedgerMismatchesAsync(CancellationToken cancellationToken = default)
    {
        var sql = @"
            SELECT u.id AS UserId,
                   u.gold_balance AS UserGoldBalance,
                   COALESCE(g.ledger_balance, 0) AS LedgerGoldBalance,
                   u.diamond_balance AS UserDiamondBalance,
                   COALESCE(d.ledger_balance, 0) AS LedgerDiamondBalance
            FROM users u
            LEFT JOIN v_user_ledger_balance g
                ON u.id = g.user_id
               AND g.currency = 'gold'
            LEFT JOIN v_user_ledger_balance d
                ON u.id = d.user_id
               AND d.currency = 'diamond'
            WHERE u.gold_balance IS DISTINCT FROM COALESCE(g.ledger_balance, 0)
               OR u.diamond_balance IS DISTINCT FROM COALESCE(d.ledger_balance, 0);
        ";
        // IS DISTINCT FROM xử lý đúng cả trường hợp null, tránh bỏ sót mismatch do giá trị thiếu.

        return await _dbContext.Database.SqlQueryRaw<MismatchRecord>(sql)
            .ToListAsync(cancellationToken);
    }
}

/*
 * FILE: AdminRepository.cs
 * MỤC ĐÍCH: Repository cho chức năng Admin — kiểm tra sai lệch sổ cái (ledger).
 *   
 *   Hàm chính: GetLedgerMismatchesAsync()
 *   → So sánh số dư của User (bảng users) với tổng giao dịch thực tế (view v_user_ledger_balance).
 *   → Nếu khác nhau = có bất thường (bug, hack, hoặc race condition).
 *   → Admin dùng để audit và phát hiện sai lệch tài chính.
 *
 *   Tại sao dùng Raw SQL thay vì LINQ?
 *   → Query này JOIN với VIEW (v_user_ledger_balance) — EF Core không map VIEW mặc định.
 *   → SQL thủ công cho phép dùng "IS DISTINCT FROM" (PostgreSQL syntax) để so sánh NULL-safe.
 */

using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository dành cho Admin — kiểm tra tính toàn vẹn dữ liệu tài chính.
/// Implement interface IAdminRepository (Application layer).
/// </summary>
public class AdminRepository : IAdminRepository
{
    // DbContext để truy cập PostgreSQL
    private readonly ApplicationDbContext _dbContext;

    public AdminRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Tìm tất cả User có số dư BỊ LỆCH so với sổ cái giao dịch.
    /// 
    /// CÁCH HOẠT ĐỘNG:
    /// 1. Bảng "users" chứa số dư hiện tại (gold_balance, diamond_balance) — cập nhật real-time.
    /// 2. View "v_user_ledger_balance" tính tổng từ wallet_transactions (ledger) — source of truth.
    /// 3. Nếu 2 số khác nhau → có sai lệch → trả về MismatchRecord.
    ///
    /// "IS DISTINCT FROM" (PostgreSQL):
    /// → Khác với "!=" thông thường vì xử lý NULL đúng cách.
    /// → NULL != 0 = NULL (không rõ ràng), nhưng NULL IS DISTINCT FROM 0 = true (rõ ràng).
    /// </summary>
    public async Task<List<MismatchRecord>> GetLedgerMismatchesAsync(CancellationToken cancellationToken = default)
    {
        var sql = @"
            SELECT u.id AS UserId, u.gold_balance AS UserGoldBalance, v.ledger_gold AS LedgerGoldBalance, 
                   u.diamond_balance AS UserDiamondBalance, v.ledger_diamond AS LedgerDiamondBalance 
            FROM users u
            LEFT JOIN v_user_ledger_balance v ON u.id = v.user_id
            WHERE u.gold_balance IS DISTINCT FROM COALESCE(v.ledger_gold, 0)
               OR u.diamond_balance IS DISTINCT FROM COALESCE(v.ledger_diamond, 0);
        ";

        // SqlQueryRaw: chạy SQL thô và map kết quả vào DTO MismatchRecord
        return await _dbContext.Database.SqlQueryRaw<MismatchRecord>(sql)
                               .ToListAsync(cancellationToken);
    }
}

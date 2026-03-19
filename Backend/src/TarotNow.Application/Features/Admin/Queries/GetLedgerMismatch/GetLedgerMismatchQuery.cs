/*
 * ===================================================================
 * FILE: GetLedgerMismatchQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch
 * ===================================================================
 * MỤC ĐÍCH:
 *   Query + DTO cho tính năng ĐỐI SOÁT SỔ CÁI (Ledger Reconciliation).
 *
 * ĐỐI SOÁT LÀ GÌ?
 *   Kiểm tra số dư ví (users.gold_balance, users.diamond_balance) 
 *   có KHỚP với tổng sổ cái (SUM(ledger_entries.amount)) không.
 *   
 *   Nếu KHỚP → hệ thống hoạt động đúng ✅
 *   Nếu LỆCH → có bug hoặc gian lận ❌ → cần điều tra
 *
 * VÍ DỤ LỆCH:
 *   User A: gold_balance = 100, nhưng SUM(ledger) = 95
 *   → Lệch 5 gold → có thể do: bug double-credit, lỗi rollback, hack
 *
 * TẠI SAO CẦN?
 *   Hệ thống tài chính PHẢI có đối soát định kỳ (daily/weekly).
 *   Đây là nguyên tắc cơ bản của kế toán: sổ cái luôn phải cân.
 * ===================================================================
 */

using MediatR;

namespace TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch;

/// <summary>
/// DTO đại diện cho 1 BẢN GHI LỆCH — 1 user mà số dư không khớp sổ cái.
/// </summary>
public class MismatchRecordDto
{
    /// <summary>UUID user bị lệch.</summary>
    public Guid UserId { get; set; }

    /// <summary>Số dư Gold hiện tại trong users table.</summary>
    public long UserGoldBalance { get; set; }

    /// <summary>Tổng Gold tính từ sổ cái (SUM ledger entries).</summary>
    public long LedgerGoldBalance { get; set; }

    /// <summary>Số dư Diamond hiện tại trong users table.</summary>
    public long UserDiamondBalance { get; set; }

    /// <summary>Tổng Diamond tính từ sổ cái.</summary>
    public long LedgerDiamondBalance { get; set; }

    /*
     * Cách đọc: nếu UserGoldBalance ≠ LedgerGoldBalance → LỖI!
     * Admin cần điều tra:
     * 1. Xem ledger entries của user này
     * 2. Tìm giao dịch bất thường
     * 3. Sửa lại balance hoặc tạo ledger entry điều chỉnh
     */
}

/// <summary>
/// Query lấy danh sách TẤT CẢ user bị lệch số dư.
/// Không cần tham số — quét toàn bộ hệ thống.
/// 
/// IRequest<List<MismatchRecordDto>>: trả về danh sách các bản ghi lệch.
/// Nếu danh sách rỗng → hệ thống cân → mọi thứ OK.
/// </summary>
public class GetLedgerMismatchQuery : IRequest<List<MismatchRecordDto>>
{
}

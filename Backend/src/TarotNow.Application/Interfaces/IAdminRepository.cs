/*
 * ===================================================================
 * FILE: IAdminRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Định nghĩa Interface giao tiếp với Database dành riêng cho các 
 *   truy vấn quản trị (Admin/Backoffice).
 * ===================================================================
 */

using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Interface cho Admin Repository.
/// Cung cấp các hàm kiểm tra đối soát, phát hiện gian lận.
/// </summary>
public interface IAdminRepository
{
    /// <summary>
    /// Chạy truy vấn đối soát v_user_ledger_balance vs users.
    /// Đối chiếu số dư hiện tại trong ví (Wallet) với tổng lịch sử giao dịch (Ledger).
    /// Nếu có độ lệch (mismatch) -> Báo động đỏ hệ thống bị hack/lỗi logic.
    /// </summary>
    /// <param name="cancellationToken">Token huỷ tác vụ.</param>
    /// <returns>Danh sách các user bị sai lệch số dư kèm thông báo chi tiết.</returns>
    Task<List<MismatchRecord>> GetLedgerMismatchesAsync(CancellationToken cancellationToken = default);
}

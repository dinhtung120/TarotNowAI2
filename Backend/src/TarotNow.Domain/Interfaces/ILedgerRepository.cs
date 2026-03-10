using TarotNow.Domain.Entities;

namespace TarotNow.Domain.Interfaces;

/// <summary>
/// Domain Interface chuyên dùng để truy vấn lịch sử giao dịch ví (Ledger) và đối soát.
/// </summary>
public interface ILedgerRepository
{
    /// <summary>
    /// Lấy danh sách biến động số dư có phân trang.
    /// </summary>
    Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(Guid userId, int page, int limit, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Đếm tổng số lượng giao dịch của user để phân trang.
    /// </summary>
    Task<int> GetTotalCountAsync(Guid userId, CancellationToken cancellationToken = default);
}



using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract truy vấn sổ cái ví để cung cấp lịch sử giao dịch và tổng số bản ghi.
public interface ILedgerRepository
{
    /// <summary>
    /// Lấy danh sách giao dịch ví theo phân trang để hiển thị lịch sử tài chính của người dùng.
    /// Luồng xử lý: lọc theo userId, áp page/limit và trả tập WalletTransaction tương ứng.
    /// </summary>
    Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(Guid userId, int page, int limit, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy tổng số giao dịch của người dùng để tính toán số trang hiển thị.
    /// Luồng xử lý: đếm toàn bộ transaction theo userId và trả số lượng bản ghi.
    /// </summary>
    Task<int> GetTotalCountAsync(Guid userId, CancellationToken cancellationToken = default);
}

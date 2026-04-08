

using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

// Contract cho nghiệp vụ quản trị cần truy xuất dữ liệu đối soát hệ thống.
public interface IAdminRepository
{
    /// <summary>
    /// Lấy danh sách lệch sổ cái để đội vận hành xử lý sai khác giao dịch.
    /// Luồng xử lý: truy vấn các mismatch hiện có và trả về tập record phục vụ điều tra.
    /// </summary>
    Task<List<MismatchRecord>> GetLedgerMismatchesAsync(CancellationToken cancellationToken = default);
}

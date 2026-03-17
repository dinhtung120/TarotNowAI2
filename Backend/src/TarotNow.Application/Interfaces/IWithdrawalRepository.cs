using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Repository cho withdrawal_requests (PostgreSQL).
/// Quản lý tạo/cập nhật yêu cầu rút tiền.
/// Wallet debit PHẢI qua IWalletRepository.
/// </summary>
public interface IWithdrawalRepository
{
    Task<WithdrawalRequest?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Kiểm tra đã có request pending/approved cùng ngày chưa.</summary>
    Task<bool> HasPendingRequestTodayAsync(Guid userId, DateOnly businessDate, CancellationToken ct = default);

    /// <summary>Danh sách requests của user (reader), sắp xếp mới nhất.</summary>
    Task<List<WithdrawalRequest>> ListByUserAsync(Guid userId, int page, int pageSize, CancellationToken ct = default);

    /// <summary>Admin queue: danh sách pending requests.</summary>
    Task<List<WithdrawalRequest>> ListPendingAsync(int page, int pageSize, CancellationToken ct = default);

    Task AddAsync(WithdrawalRequest request, CancellationToken ct = default);
    Task UpdateAsync(WithdrawalRequest request, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}

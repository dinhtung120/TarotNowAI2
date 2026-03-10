using TarotNow.Domain.Entities;

namespace TarotNow.Domain.Interfaces;

public interface IAdminRepository
{
    // Chạy truy vấn đối soát v_user_ledger_balance vs users
    Task<List<MismatchRecord>> GetLedgerMismatchesAsync(CancellationToken cancellationToken = default);
}

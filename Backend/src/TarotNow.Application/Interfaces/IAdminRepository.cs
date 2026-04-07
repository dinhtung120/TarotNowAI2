

using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

public interface IAdminRepository
{
        Task<List<MismatchRecord>> GetLedgerMismatchesAsync(CancellationToken cancellationToken = default);
}

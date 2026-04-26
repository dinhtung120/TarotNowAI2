using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Truy cập persisted state của reading reveal saga để hỗ trợ retry/repair an toàn.
/// </summary>
public interface IReadingRevealSagaStateRepository
{
    Task<ReadingRevealSagaState?> GetBySessionIdAsync(
        string sessionId,
        CancellationToken cancellationToken = default);

    Task<ReadingRevealSagaState> GetOrCreateAsync(
        string sessionId,
        Guid userId,
        string language,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        ReadingRevealSagaState state,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReadingRevealSagaState>> ListDueRepairAsync(
        DateTime nowUtc,
        int batchSize,
        CancellationToken cancellationToken = default);
}

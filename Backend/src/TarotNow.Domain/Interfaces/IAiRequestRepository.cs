using TarotNow.Domain.Entities;

namespace TarotNow.Domain.Interfaces;

/// <summary>
/// Domain Interface cho thao tác với AI Requests.
/// Đảm bảo tính nhất quán (Idempotency) khi hoàn tiền hoặc cập nhật trạng thái Stream.
/// </summary>
public interface IAiRequestRepository
{
    Task<AiRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(AiRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(AiRequest request, CancellationToken cancellationToken = default);
    
    Task<int> GetDailyAiRequestCountAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<int> GetActiveAiRequestCountAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<int> GetFollowupCountBySessionAsync(string sessionId, CancellationToken cancellationToken = default);
}

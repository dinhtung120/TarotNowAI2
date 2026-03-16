using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

public interface IReadingSessionRepository
{
    Task<ReadingSession> CreateAsync(ReadingSession session, CancellationToken cancellationToken = default);
    Task<ReadingSession?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task UpdateAsync(ReadingSession session, CancellationToken cancellationToken = default);
    
    // Check xem user đã rút Daily card hôm nay chưa (theo UTC)
    Task<bool> HasDrawnDailyCardAsync(Guid userId, DateTime utcNow, CancellationToken cancellationToken = default);

    // Xử lý nguyên tử (Atomic Transaction): Trừ tiền & Cấp Session
    Task<(bool Success, string ErrorMessage)> StartPaidSessionAtomicAsync(Guid userId, string spreadType, ReadingSession session, long costGold, long costDiamond, CancellationToken cancellationToken = default);

    // Phase 1.5 - History Paging
    Task<(IEnumerable<ReadingSession> Items, int TotalCount)> GetSessionsByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);

    // Phase 1.5 - History Detail
    Task<(ReadingSession ReadingSession, IEnumerable<TarotNow.Domain.Entities.AiRequest> AiRequests)?> GetSessionWithAiRequestsAsync(
        string sessionId, CancellationToken cancellationToken = default);

    // Admin - Lấy toàn bộ lịch sử (có bộ lọc)
    Task<(IEnumerable<ReadingSession> Items, int TotalCount)> GetAllSessionsAsync(
        int page, 
        int pageSize, 
        List<string>? userIds = null,
        string? spreadType = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);
}

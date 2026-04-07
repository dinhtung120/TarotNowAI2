

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IReadingSessionRepository
{
        Task<ReadingSession> CreateAsync(ReadingSession session, CancellationToken cancellationToken = default);
    
        Task<ReadingSession?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    
        Task UpdateAsync(ReadingSession session, CancellationToken cancellationToken = default);
    
        Task<bool> HasDrawnDailyCardAsync(Guid userId, DateTime utcNow, CancellationToken cancellationToken = default);

        Task<(IEnumerable<ReadingSession> Items, int TotalCount)> GetSessionsByUserIdAsync(
        Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);

        Task<(ReadingSession ReadingSession, IEnumerable<TarotNow.Domain.Entities.AiRequest> AiRequests)?> GetSessionWithAiRequestsAsync(
        string sessionId, CancellationToken cancellationToken = default);

        Task<(IEnumerable<ReadingSession> Items, int TotalCount)> GetAllSessionsAsync(
        int page, 
        int pageSize, 
        List<string>? userIds = null,
        string? spreadType = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);
}

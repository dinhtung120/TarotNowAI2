

using TarotNow.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IAiRequestRepository
{
        Task<AiRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
        Task AddAsync(AiRequest request, CancellationToken cancellationToken = default);
    
        Task UpdateAsync(AiRequest request, CancellationToken cancellationToken = default);
    
        Task<int> GetDailyAiRequestCountAsync(Guid userId, CancellationToken cancellationToken = default);
    
        Task<int> GetActiveAiRequestCountAsync(Guid userId, CancellationToken cancellationToken = default);
    
        Task<int> GetFollowupCountBySessionAsync(string sessionId, CancellationToken cancellationToken = default);
}

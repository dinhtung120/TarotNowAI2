using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Interfaces;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class AiRequestRepository : IAiRequestRepository
{
    private readonly ApplicationDbContext _context;

    public AiRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AiRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AiRequests.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<AiRequest?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default)
    {
        return await _context.AiRequests.FirstOrDefaultAsync(x => x.IdempotencyKey == idempotencyKey, cancellationToken);
    }

    public async Task AddAsync(AiRequest request, CancellationToken cancellationToken = default)
    {
        await _context.AiRequests.AddAsync(request, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(AiRequest request, CancellationToken cancellationToken = default)
    {
        _context.AiRequests.Update(request);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> GetDailyAiRequestCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var todayOffset = new DateTimeOffset(DateTime.UtcNow.Date, TimeSpan.Zero);
        return await _context.AiRequests.CountAsync(
            x => x.UserId == userId && 
                 x.CreatedAt >= todayOffset && 
                 (x.Status == TarotNow.Domain.Enums.AiRequestStatus.Completed || x.Status == TarotNow.Domain.Enums.AiRequestStatus.FirstTokenReceived), 
            cancellationToken);
    }

    public async Task<int> GetActiveAiRequestCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.AiRequests.CountAsync(
            x => x.UserId == userId && 
                 (x.Status == TarotNow.Domain.Enums.AiRequestStatus.Requested || x.Status == TarotNow.Domain.Enums.AiRequestStatus.FirstTokenReceived), 
            cancellationToken);
    }

    public async Task<int> GetFollowupCountBySessionAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        // Follow-up records contain a distinct IdempotencyKey pattern or can be distinguished.
        // Actually, the simpler way is to count ALL requests for this session MINUS 1 (the original 3-card reading).
        // If there's 0 or 1, followup count is 0. If there are 3, followup count is 2.
        
        var totalRequests = await _context.AiRequests.CountAsync(
            x => x.ReadingSessionRef == sessionId && 
                 x.Status == TarotNow.Domain.Enums.AiRequestStatus.Completed, 
            cancellationToken);

        return Math.Max(0, totalRequests - 1);
    }
}

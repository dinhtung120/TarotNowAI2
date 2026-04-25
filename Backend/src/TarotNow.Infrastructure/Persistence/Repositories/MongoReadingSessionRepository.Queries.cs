using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial truy vấn đọc chi tiết reading session.
public partial class MongoReadingSessionRepository
{
    /// <summary>
    /// Lấy reading session theo id.
    /// Luồng xử lý: query Mongo theo BuildIdFilter và map sang domain entity nếu có.
    /// </summary>
    public async Task<ReadingSession?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var doc = await _mongoContext.ReadingSessions
            .Find(BuildIdFilter(id))
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : MapToEntity(doc);
    }

    /// <summary>
    /// Kiểm tra user đã rút daily card trong ngày hay chưa.
    /// Luồng xử lý: xác định khoảng [startOfDay, endOfDay) UTC và đếm sessions spread Daily1Card trong khoảng.
    /// </summary>
    public async Task<bool> HasDrawnDailyCardAsync(Guid userId, DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var startOfDay = utcNow.Date;
        var endOfDay = startOfDay.AddDays(1);
        // Dùng cận trái đóng, cận phải mở để tránh overlap giữa các ngày.

        var count = await _mongoContext.ReadingSessions.CountDocumentsAsync(
            r => r.UserId == userId.ToString()
                && r.SpreadType == SpreadType.Daily1Card
                && r.CreatedAt >= startOfDay
                && r.CreatedAt < endOfDay,
            cancellationToken: cancellationToken);

        return count > 0;
    }

    /// <summary>
    /// Lấy session kèm danh sách AI requests liên quan.
    /// Luồng xử lý: lấy session từ Mongo, nếu tồn tại thì truy vấn AiRequests từ PostgreSQL theo sessionId.
    /// </summary>
    public async Task<(ReadingSession ReadingSession, IEnumerable<AiRequest> AiRequests)?> GetSessionWithAiRequestsAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        var session = await GetByIdAsync(sessionId, cancellationToken);
        if (session == null)
        {
            return null;
            // Edge case: session không tồn tại thì không truy vấn phụ AI requests.
        }

        if (Guid.TryParse(sessionId, out var readingSessionRef) == false || readingSessionRef == Guid.Empty)
        {
            // Session id không phải Guid thì không thể join sang ai_requests kiểu uuid.
            return (session, Enumerable.Empty<AiRequest>());
        }

        var aiRequests = await _pgContext.AiRequests
            .Where(a => a.ReadingSessionRef == readingSessionRef)
            .OrderBy(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
        // Giữ thứ tự thời gian để tái dựng timeline stream/follow-up chính xác.

        return (session, aiRequests);
    }
}

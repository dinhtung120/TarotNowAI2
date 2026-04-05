using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Triển khai ICallSessionRepository bằng MongoDB.
/// Thực hiện CRUD metadata cuộc gọi, sử dụng atomic updates để tránh data race.
/// </summary>
public partial class MongoCallSessionRepository : ICallSessionRepository
{
    private readonly MongoDbContext _context;

    public MongoCallSessionRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CallSessionDto session, CancellationToken ct = default)
    {
        var document = CallSessionDocumentMapper.ToDocument(session);
        await _context.CallSessions.InsertOneAsync(document, cancellationToken: ct);
        session.Id = document.Id;
    }

    public async Task<CallSessionDto?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        var doc = await _context.CallSessions.Find(x => x.Id == id).FirstOrDefaultAsync(ct);
        return doc == null ? null : CallSessionDocumentMapper.ToDto(doc);
    }

    public async Task<CallSessionDto?> GetActiveByConversationAsync(string conversationId, CancellationToken ct = default)
    {
        var filter = Builders<CallSessionDocument>.Filter.Eq(x => x.ConversationId, conversationId) &
                     Builders<CallSessionDocument>.Filter.In(x => x.Status, new[] { "requested", "accepted" });

        var doc = await _context.CallSessions.Find(filter).FirstOrDefaultAsync(ct);
        return doc == null ? null : CallSessionDocumentMapper.ToDto(doc);
    }

    /// <summary>
    /// FIX #12: Khi update status = Ended, tự động tính DurationSeconds
    /// từ (EndedAt - StartedAt) nếu cả hai field đều có giá trị.
    /// Điều này đảm bảo DurationSeconds luôn chính xác và không bao giờ null
    /// sau khi cuộc gọi kết thúc.
    /// </summary>
    public async Task<bool> UpdateStatusAsync(
        string id,
        CallSessionStatus newStatus,
        DateTime? startedAt = null,
        DateTime? endedAt = null,
        string? endReason = null,
        CallSessionStatus? expectedPreviousStatus = null,
        CancellationToken ct = default)
    {
        var filterBuilder = Builders<CallSessionDocument>.Filter;
        var filter = filterBuilder.Eq(x => x.Id, id);

        // FIX #17 (Idempotency): Chỉ update nếu status hiện tại đúng như mong đợi
        // Ví dụ: Chỉ Accept nếu đang là Requested. Tránh Duplicate Accept.
        if (expectedPreviousStatus.HasValue)
        {
            var expectedStatusString = CallSessionDocumentMapper.MapStatus(expectedPreviousStatus.Value);
            filter &= filterBuilder.Eq(x => x.Status, expectedStatusString);
        }
        
        var update = Builders<CallSessionDocument>.Update
            .Set(x => x.Status, CallSessionDocumentMapper.MapStatus(newStatus))
            .Set(x => x.UpdatedAt, DateTime.UtcNow);

        if (startedAt.HasValue)
            update = update.Set(x => x.StartedAt, startedAt.Value);
            
        if (endedAt.HasValue)
            update = update.Set(x => x.EndedAt, endedAt.Value);
            
        if (!string.IsNullOrEmpty(endReason))
            update = update.Set(x => x.EndReason, endReason);

        // FIX #12: Nếu đang kết thúc cuộc gọi, tính durationSeconds
        // Cần lấy document hiện tại để biết StartedAt
        if (newStatus == CallSessionStatus.Ended && endedAt.HasValue)
        {
            var existingDoc = await _context.CallSessions.Find(filter).FirstOrDefaultAsync(ct);
            if (existingDoc?.StartedAt != null)
            {
                var duration = (int)(endedAt.Value - existingDoc.StartedAt.Value).TotalSeconds;
                if (duration < 0) duration = 0;
                update = update.Set(x => x.DurationSeconds, duration);
            }
        }

        var result = await _context.CallSessions.UpdateOneAsync(filter, update, cancellationToken: ct);
        return result.MatchedCount > 0;
    }

    /// <summary>
    /// FIX #10: Tách count và items thành 2 filter riêng biệt.
    /// 
    /// Trước đây dùng chung 1 IFindFluent cho cả CountDocumentsAsync và ToListAsync,
    /// có thể gây undefined behavior do shared cursor nội bộ.
    /// Giờ mỗi query dùng filter riêng → an toàn concurrency.
    /// </summary>
    public async Task<(IEnumerable<CallSessionDto> Items, long TotalCount)> GetByConversationIdPaginatedAsync(
        string conversationId,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var filter = Builders<CallSessionDocument>.Filter.Eq(x => x.ConversationId, conversationId);

        // FIX #10: Tạo 2 query riêng biệt với cùng filter (nhưng khác cursor)
        var totalTask = _context.CallSessions.CountDocumentsAsync(filter, cancellationToken: ct);
        var itemsTask = _context.CallSessions
            .Find(filter)
            .SortByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(ct);

        await Task.WhenAll(totalTask, itemsTask);

        var dtos = itemsTask.Result.Select(CallSessionDocumentMapper.ToDto).ToList();
        return (dtos, totalTask.Result);
    }
}

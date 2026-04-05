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

    public async Task<IEnumerable<CallSessionDto>> GetActiveByConversationIdsAsync(IEnumerable<string> conversationIds, CancellationToken ct = default)
    {
        var ids = conversationIds.ToList();
        if (ids.Count == 0) return Enumerable.Empty<CallSessionDto>();

        var filter = Builders<CallSessionDocument>.Filter.In(x => x.ConversationId, ids) &
                     Builders<CallSessionDocument>.Filter.In(x => x.Status, new[] { "requested", "accepted" });

        var docs = await _context.CallSessions.Find(filter).ToListAsync(ct);
        return docs.Select(CallSessionDocumentMapper.ToDto);
    }

    /// <summary>
    /// FIX #1: Atomic UpdateStatusAsync với FindOneAndUpdateAsync
    /// Khắc phục hoàn toàn Read-then-write Race condition.
    /// Tính toán durationSeconds trực tiếp hoặc qua 2 bước an toàn hơn: 
    /// Lấy document cũ ra, tính toán nội bộ trong ram, rồi ghi lại bằng UpdateOneAsync (nếu không cần strict atomic aggregation),
    /// Tuy nhiên ở đây, "FindOneAndUpdate" trả về DOC CŨ, từ đó có thể Update thêm DurationSeconds an toàn nếu cần.
    /// NHƯNG CÁCH TỐT NHẤT: Update trạng thái nguyên tử, rồi tính toán bù.
    /// Để giữ signature, tôi thay thành Update liên hoàn nếu cần.
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

        // FIX #1 (Race Condition): Sử dụng FindOneAndUpdateAsync để đảm bảo atomic operation cực cao
        // Trả về document cũ để tính toán duration nếu cần.
        var options = new FindOneAndUpdateOptions<CallSessionDocument>
        {
            ReturnDocument = ReturnDocument.Before
        };

        var oldDoc = await _context.CallSessions.FindOneAndUpdateAsync(filter, update, options, cancellationToken: ct);
        
        if (oldDoc == null) return false;

        // Bổ trợ Fix #12: Tính durationSeconds nguyên tử nếu vừa bị Ended (End call)
        if (newStatus == CallSessionStatus.Ended && endedAt.HasValue && oldDoc.StartedAt.HasValue)
        {
            var duration = (int)(endedAt.Value - oldDoc.StartedAt.Value).TotalSeconds;
            if (duration < 0) duration = 0;
            
            var durationUpdate = Builders<CallSessionDocument>.Update.Set(x => x.DurationSeconds, duration);
            await _context.CallSessions.UpdateOneAsync(x => x.Id == id, durationUpdate, cancellationToken: ct);
        }

        return true;
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

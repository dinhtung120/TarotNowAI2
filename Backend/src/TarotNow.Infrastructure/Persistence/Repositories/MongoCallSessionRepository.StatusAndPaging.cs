using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý cập nhật trạng thái và phân trang call sessions.
public partial class MongoCallSessionRepository
{
    /// <summary>
    /// Cập nhật trạng thái cuộc gọi với kiểm soát trạng thái trước đó.
    /// Luồng xử lý: dựng filter/update, find-and-update atomically, sau đó cập nhật duration nếu cuộc gọi ended.
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
        var filter = BuildUpdateStatusFilter(id, expectedPreviousStatus);
        var update = BuildUpdateStatusDefinition(newStatus, startedAt, endedAt, endReason);
        var oldDoc = await FindAndUpdateCallSessionAsync(filter, update, ct);

        if (oldDoc == null)
        {
            return false;
            // Không tìm thấy bản ghi hoặc không khớp expected status thì coi như update thất bại.
        }

        await UpdateDurationWhenEndedAsync(id, newStatus, endedAt, oldDoc.StartedAt, ct);
        return true;
    }

    /// <summary>
    /// Lấy lịch sử call session theo conversation có phân trang.
    /// Luồng xử lý: chạy song song count và query danh sách rồi map sang DTO.
    /// </summary>
    public async Task<(IEnumerable<CallSessionDto> Items, long TotalCount)> GetByConversationIdPaginatedAsync(
        string conversationId,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var filter = Builders<CallSessionDocument>.Filter.Eq(x => x.ConversationId, conversationId);
        var totalTask = _context.CallSessions.CountDocumentsAsync(filter, cancellationToken: ct);
        var itemsTask = _context.CallSessions
            .Find(filter)
            .SortByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(ct);

        await Task.WhenAll(totalTask, itemsTask);
        // Tận dụng parallel IO để giảm thời gian chờ tổng thể của API history.
        return (itemsTask.Result.Select(CallSessionDocumentMapper.ToDto).ToList(), totalTask.Result);
    }

    /// <summary>
    /// Dựng filter cho thao tác cập nhật trạng thái.
    /// Luồng xử lý: luôn khóa theo id, và bổ sung expectedPreviousStatus khi caller yêu cầu optimistic-concurrency.
    /// </summary>
    private static FilterDefinition<CallSessionDocument> BuildUpdateStatusFilter(string id, CallSessionStatus? expectedPreviousStatus)
    {
        var filterBuilder = Builders<CallSessionDocument>.Filter;
        var filter = filterBuilder.Eq(x => x.Id, id);

        if (expectedPreviousStatus.HasValue)
        {
            var expectedStatus = CallSessionDocumentMapper.MapStatus(expectedPreviousStatus.Value);
            filter &= filterBuilder.Eq(x => x.Status, expectedStatus);
            // Chặn race-condition đổi trạng thái sai thứ tự giữa nhiều actor.
        }

        return filter;
    }

    /// <summary>
    /// Dựng update definition cho thay đổi trạng thái cuộc gọi.
    /// Luồng xử lý: set status/updated_at bắt buộc, chỉ set started/ended/end_reason khi dữ liệu đầu vào hợp lệ.
    /// </summary>
    private static UpdateDefinition<CallSessionDocument> BuildUpdateStatusDefinition(
        CallSessionStatus newStatus,
        DateTime? startedAt,
        DateTime? endedAt,
        string? endReason)
    {
        var update = Builders<CallSessionDocument>.Update
            .Set(x => x.Status, CallSessionDocumentMapper.MapStatus(newStatus))
            .Set(x => x.UpdatedAt, DateTime.UtcNow);

        if (startedAt.HasValue)
        {
            update = update.Set(x => x.StartedAt, startedAt.Value);
            // Chỉ ghi StartedAt khi có tín hiệu bắt đầu cuộc gọi.
        }

        if (endedAt.HasValue)
        {
            update = update.Set(x => x.EndedAt, endedAt.Value);
            // EndedAt chỉ ghi tại bước kết thúc để tránh ghi sai trạng thái trung gian.
        }

        if (!string.IsNullOrWhiteSpace(endReason))
        {
            update = update.Set(x => x.EndReason, endReason);
            // Chỉ lưu lý do kết thúc khi caller thực sự cung cấp nội dung.
        }

        return update;
    }

    /// <summary>
    /// Tìm và cập nhật call session theo kiểu atomic.
    /// Luồng xử lý: FindOneAndUpdate trả về document trước cập nhật để phục vụ tính duration.
    /// </summary>
    private async Task<CallSessionDocument?> FindAndUpdateCallSessionAsync(
        FilterDefinition<CallSessionDocument> filter,
        UpdateDefinition<CallSessionDocument> update,
        CancellationToken ct)
    {
        return await _context.CallSessions.FindOneAndUpdateAsync(
            filter,
            update,
            new FindOneAndUpdateOptions<CallSessionDocument> { ReturnDocument = ReturnDocument.Before },
            cancellationToken: ct);
        // ReturnDocument.Before giúp giữ StartedAt cũ ổn định cho bước tính thời lượng.
    }
}

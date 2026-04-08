using MongoDB.Driver;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial truy vấn danh sách reading sessions cho user/admin.
public partial class MongoReadingSessionRepository
{
    /// <summary>
    /// Lấy danh sách reading session theo user có phân trang.
    /// Luồng xử lý: chuẩn hóa page/pageSize, filter user + chưa xóa, đếm tổng rồi query page mới nhất trước.
    /// </summary>
    public async Task<(IEnumerable<ReadingSession> Items, int TotalCount)> GetSessionsByUserIdAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 200);
        // Mặc định 10 bản ghi phù hợp màn history người dùng.

        var filter = Builders<ReadingSessionDocument>.Filter.Eq(r => r.UserId, userId.ToString())
            & Builders<ReadingSessionDocument>.Filter.Eq(r => r.IsDeleted, false);

        var totalCount = await _mongoContext.ReadingSessions.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        var docs = await QueryPagedAsync(filter, normalizedPage, normalizedPageSize, cancellationToken);

        return (docs.Select(MapToEntity).ToList(), (int)totalCount);
    }

    /// <summary>
    /// Lấy danh sách reading session cho màn admin với bộ lọc tổng hợp.
    /// Luồng xử lý: dựng filter theo userIds/spread/date range, đếm tổng và query phân trang.
    /// </summary>
    public async Task<(IEnumerable<ReadingSession> Items, int TotalCount)> GetAllSessionsAsync(
        int page,
        int pageSize,
        List<string>? userIds = null,
        string? spreadType = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 200);
        var filter = BuildAdminFilter(userIds, spreadType, startDate, endDate);

        var totalCount = await _mongoContext.ReadingSessions.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        var docs = await QueryPagedAsync(filter, normalizedPage, normalizedPageSize, cancellationToken);

        return (docs.Select(MapToEntity).ToList(), (int)totalCount);
    }

    /// <summary>
    /// Dựng filter admin cho reading sessions.
    /// Luồng xử lý: luôn lọc is_deleted=false và bổ sung từng điều kiện tùy chọn khi input có giá trị.
    /// </summary>
    private static FilterDefinition<ReadingSessionDocument> BuildAdminFilter(
        List<string>? userIds,
        string? spreadType,
        DateTime? startDate,
        DateTime? endDate)
    {
        var builder = Builders<ReadingSessionDocument>.Filter;
        var filter = builder.Eq(r => r.IsDeleted, false);

        if (userIds != null && userIds.Any())
        {
            filter &= builder.In(r => r.UserId, userIds);
            // Lọc theo tập userIds khi admin muốn khoanh vùng dữ liệu người dùng cụ thể.
        }

        if (!string.IsNullOrWhiteSpace(spreadType))
        {
            filter &= builder.Eq(r => r.SpreadType, spreadType);
        }

        if (startDate.HasValue)
        {
            filter &= builder.Gte(r => r.CreatedAt, startDate.Value);
            // Cận dưới khoảng thời gian.
        }

        if (endDate.HasValue)
        {
            filter &= builder.Lte(r => r.CreatedAt, endDate.Value);
            // Cận trên khoảng thời gian.
        }

        return filter;
    }

    /// <summary>
    /// Chạy truy vấn phân trang dùng chung.
    /// Luồng xử lý: sort created_at desc rồi skip/take theo trang.
    /// </summary>
    private Task<List<ReadingSessionDocument>> QueryPagedAsync(
        FilterDefinition<ReadingSessionDocument> filter,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        return _mongoContext.ReadingSessions
            .Find(filter)
            .SortByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }
}

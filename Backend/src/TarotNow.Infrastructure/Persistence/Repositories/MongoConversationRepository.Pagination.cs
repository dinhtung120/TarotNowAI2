using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý phân trang conversation theo participant.
public partial class MongoConversationRepository
{
    /// <summary>
    /// Lấy conversation theo user id có phân trang.
    /// Luồng xử lý: dựng filter participant phía user và chuyển sang hàm phân trang dùng chung.
    /// </summary>
    public Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByUserIdPaginatedAsync(
        string userId,
        int page,
        int pageSize,
        IReadOnlyCollection<string>? statuses = null,
        CancellationToken cancellationToken = default)
    {
        var filter = BuildParticipantFilter(
            Builders<ConversationDocument>.Filter.Eq(c => c.UserId, userId),
            statuses);

        return GetPaginatedInternal(filter, page, pageSize, cancellationToken);
    }

    /// <summary>
    /// Lấy conversation theo reader id có phân trang.
    /// Luồng xử lý: dựng filter participant phía reader và dùng chung luồng phân trang.
    /// </summary>
    public Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByReaderIdPaginatedAsync(
        string readerId,
        int page,
        int pageSize,
        IReadOnlyCollection<string>? statuses = null,
        CancellationToken cancellationToken = default)
    {
        var filter = BuildParticipantFilter(
            Builders<ConversationDocument>.Filter.Eq(c => c.ReaderId, readerId),
            statuses);

        return GetPaginatedInternal(filter, page, pageSize, cancellationToken);
    }

    /// <summary>
    /// Lấy conversation theo participant bất kỳ (user hoặc reader).
    /// Luồng xử lý: filter OR hai vai trò participant rồi gọi hàm phân trang nội bộ.
    /// </summary>
    public Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByParticipantIdPaginatedAsync(
        string participantId,
        int page,
        int pageSize,
        IReadOnlyCollection<string>? statuses = null,
        CancellationToken cancellationToken = default)
    {
        var filter = BuildParticipantFilter(
            Builders<ConversationDocument>.Filter.Or(
                Builders<ConversationDocument>.Filter.Eq(c => c.UserId, participantId),
                Builders<ConversationDocument>.Filter.Eq(c => c.ReaderId, participantId)),
            statuses);

        return GetPaginatedInternal(filter, page, pageSize, cancellationToken);
    }

    /// <summary>
    /// Dựng filter participant + trạng thái cho conversation.
    /// Luồng xử lý: luôn lọc is_deleted=false và thêm điều kiện status khi caller truyền danh sách trạng thái.
    /// </summary>
    private static FilterDefinition<ConversationDocument> BuildParticipantFilter(
        FilterDefinition<ConversationDocument> participantFilter,
        IReadOnlyCollection<string>? statuses)
    {
        var filters = new List<FilterDefinition<ConversationDocument>>
        {
            participantFilter,
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false)
        };

        if (statuses != null && statuses.Count > 0)
        {
            filters.Add(Builders<ConversationDocument>.Filter.In(c => c.Status, statuses));
            // Chỉ áp filter status khi có dữ liệu, tránh ràng buộc rỗng gây loại hết bản ghi.
        }

        return Builders<ConversationDocument>.Filter.And(filters);
    }

    /// <summary>
    /// Thực hiện truy vấn conversation phân trang dùng chung.
    /// Luồng xử lý: chuẩn hóa tham số page/pageSize, đếm tổng và lấy danh sách theo last_message_at desc.
    /// </summary>
    private async Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetPaginatedInternal(
        FilterDefinition<ConversationDocument> filter,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);
        // Page size có trần để đảm bảo API inbox không bị truy vấn quá nặng.

        var totalCount = await _context.Conversations.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _context.Conversations.Find(filter)
            .SortByDescending(c => c.LastMessageAt)
            .ThenByDescending(c => c.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        return (docs.Select(ToDto), totalCount);
    }
}

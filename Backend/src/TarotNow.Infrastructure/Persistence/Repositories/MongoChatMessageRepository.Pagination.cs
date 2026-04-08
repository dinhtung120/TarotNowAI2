using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý phân trang message theo page/cursor.
public partial class MongoChatMessageRepository
{
    /// <summary>
    /// Lấy message theo conversation với phân trang kiểu page-number.
    /// Luồng xử lý: chuẩn hóa page/pageSize, lọc message chưa xóa, đếm tổng và trả danh sách mới nhất trước.
    /// </summary>
    public async Task<(IEnumerable<ChatMessageDto> Items, long TotalCount)> GetByConversationIdPaginatedAsync(
        string conversationId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 50 : Math.Min(pageSize, 200);
        // Giới hạn page size để tránh truy vấn quá nặng trong hội thoại lớn.

        var filter = Builders<ChatMessageDocument>.Filter.And(
            Builders<ChatMessageDocument>.Filter.Eq(m => m.ConversationId, conversationId),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false));
        // Luôn loại message soft-delete để giữ nhất quán với UI chat.

        var totalCount = await _context.ChatMessages.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        var docs = await _context.ChatMessages.Find(filter)
            .SortByDescending(m => m.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        return (docs.Select(ToDto), totalCount);
    }

    /// <summary>
    /// Lấy message theo conversation với cursor-based pagination.
    /// Luồng xử lý: validate cursor ObjectId, lọc bản ghi cũ hơn cursor, lấy limit+1 để xác định hasMore và sinh nextCursor.
    /// </summary>
    public async Task<(IReadOnlyList<ChatMessageDto> Items, string? NextCursor)> GetByConversationIdCursorAsync(
        string conversationId,
        string? cursor,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var normalizedLimit = limit <= 0 ? 50 : Math.Min(limit, 200);
        // Limit được khóa cứng để tránh client kéo quá nhiều bản ghi một lần.

        var filterBuilder = Builders<ChatMessageDocument>.Filter;
        var filter = filterBuilder.And(
            filterBuilder.Eq(m => m.ConversationId, conversationId),
            filterBuilder.Eq(m => m.IsDeleted, false));

        if (string.IsNullOrWhiteSpace(cursor) == false)
        {
            if (ObjectId.TryParse(cursor, out var cursorObjectId) == false)
            {
                return (Array.Empty<ChatMessageDto>(), null);
                // Edge case: cursor sai định dạng thì trả trang rỗng thay vì throw lỗi.
            }

            filter = filterBuilder.And(filter, filterBuilder.Lt(m => m.Id, cursorObjectId.ToString()));
            // Cursor pagination: chỉ lấy message có id nhỏ hơn cursor hiện tại.
        }

        var docs = await _context.ChatMessages
            .Find(filter)
            .SortByDescending(m => m.Id)
            .Limit(normalizedLimit + 1)
            .ToListAsync(cancellationToken);
        // Lấy dư 1 phần tử để biết còn trang sau hay không.

        var hasMore = docs.Count > normalizedLimit;
        var pageDocs = hasMore ? docs.Take(normalizedLimit).ToList() : docs;
        var nextCursor = hasMore && pageDocs.Count > 0 ? pageDocs[^1].Id : null;
        return (pageDocs.Select(ToDto).ToList(), nextCursor);
    }
}

using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoChatMessageRepository
{
    public async Task<(IEnumerable<ChatMessageDto> Items, long TotalCount)> GetByConversationIdPaginatedAsync(
        string conversationId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 50 : Math.Min(pageSize, 200);

        var filter = Builders<ChatMessageDocument>.Filter.And(
            Builders<ChatMessageDocument>.Filter.Eq(m => m.ConversationId, conversationId),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false));

        var totalCount = await _context.ChatMessages.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        var docs = await _context.ChatMessages.Find(filter)
            .SortByDescending(m => m.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        return (docs.Select(ToDto), totalCount);
    }

    public async Task<(IReadOnlyList<ChatMessageDto> Items, string? NextCursor)> GetByConversationIdCursorAsync(
        string conversationId,
        string? cursor,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var normalizedLimit = limit <= 0 ? 50 : Math.Min(limit, 200);
        var filterBuilder = Builders<ChatMessageDocument>.Filter;
        var filter = filterBuilder.And(
            filterBuilder.Eq(m => m.ConversationId, conversationId),
            filterBuilder.Eq(m => m.IsDeleted, false));

        if (string.IsNullOrWhiteSpace(cursor) == false)
        {
            if (ObjectId.TryParse(cursor, out var cursorObjectId) == false)
            {
                return (Array.Empty<ChatMessageDto>(), null);
            }

            filter = filterBuilder.And(filter, filterBuilder.Lt(m => m.Id, cursorObjectId.ToString()));
        }

        var docs = await _context.ChatMessages
            .Find(filter)
            .SortByDescending(m => m.Id)
            .Limit(normalizedLimit + 1)
            .ToListAsync(cancellationToken);

        var hasMore = docs.Count > normalizedLimit;
        var pageDocs = hasMore ? docs.Take(normalizedLimit).ToList() : docs;
        var nextCursor = hasMore && pageDocs.Count > 0 ? pageDocs[^1].Id : null;
        return (pageDocs.Select(ToDto).ToList(), nextCursor);
    }
}

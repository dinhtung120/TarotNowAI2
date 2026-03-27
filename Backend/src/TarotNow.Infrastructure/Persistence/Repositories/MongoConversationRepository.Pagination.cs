using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoConversationRepository
{
    public Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByUserIdPaginatedAsync(
        string userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.UserId, userId),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        return GetPaginatedInternal(filter, page, pageSize, cancellationToken);
    }

    public Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByReaderIdPaginatedAsync(
        string readerId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.ReaderId, readerId),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        return GetPaginatedInternal(filter, page, pageSize, cancellationToken);
    }

    public Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByParticipantIdPaginatedAsync(
        string participantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Or(
                Builders<ConversationDocument>.Filter.Eq(c => c.UserId, participantId),
                Builders<ConversationDocument>.Filter.Eq(c => c.ReaderId, participantId)),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        return GetPaginatedInternal(filter, page, pageSize, cancellationToken);
    }

    private async Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetPaginatedInternal(
        FilterDefinition<ConversationDocument> filter,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

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

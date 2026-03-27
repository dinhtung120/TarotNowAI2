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
        IReadOnlyCollection<string>? statuses = null,
        CancellationToken cancellationToken = default)
    {
        var filter = BuildParticipantFilter(
            Builders<ConversationDocument>.Filter.Eq(c => c.UserId, userId),
            statuses);

        return GetPaginatedInternal(filter, page, pageSize, cancellationToken);
    }

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
        }

        return Builders<ConversationDocument>.Filter.And(filters);
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

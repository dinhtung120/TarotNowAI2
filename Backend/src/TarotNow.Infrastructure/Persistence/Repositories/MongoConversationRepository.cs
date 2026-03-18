using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository cho conversations collection (MongoDB).
/// Map giữa ConversationDto (Application) ↔ ConversationDocument (Infrastructure).
/// </summary>
public class MongoConversationRepository : IConversationRepository
{
    private readonly MongoDbContext _context;

    public MongoConversationRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ConversationDto conversation, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(conversation);
        await _context.Conversations.InsertOneAsync(doc, cancellationToken: cancellationToken);
        conversation.Id = doc.Id;
    }

    public async Task<ConversationDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.Id, id),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        var doc = await _context.Conversations.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : ToDto(doc);
    }

    /// <summary>
    /// Tìm conversation active giữa 2 user — tránh tạo duplicate.
    /// Chỉ xét status: pending hoặc active.
    /// </summary>
    public async Task<ConversationDto?> GetActiveByParticipantsAsync(
        string userId, string readerId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.UserId, userId),
            Builders<ConversationDocument>.Filter.Eq(c => c.ReaderId, readerId),
            Builders<ConversationDocument>.Filter.In(c => c.Status, new[] { "pending", "active" }),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        var doc = await _context.Conversations.Find(filter)
            .SortByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : ToDto(doc);
    }

    /// <summary>Inbox user — sort by last_message_at DESC.</summary>
    public async Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByUserIdPaginatedAsync(
        string userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.UserId, userId),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        return await GetPaginatedInternal(filter, page, pageSize, cancellationToken);
    }

    /// <summary>Inbox reader.</summary>
    public async Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByReaderIdPaginatedAsync(
        string readerId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.ReaderId, readerId),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        return await GetPaginatedInternal(filter, page, pageSize, cancellationToken);
    }

    public async Task UpdateAsync(ConversationDto conversation, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(conversation);
        doc.UpdatedAt = DateTime.UtcNow;
        var filter = Builders<ConversationDocument>.Filter.Eq(c => c.Id, doc.Id);
        await _context.Conversations.ReplaceOneAsync(filter, doc, cancellationToken: cancellationToken);
    }

    // --- Helpers ---

    private async Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetPaginatedInternal(
        FilterDefinition<ConversationDocument> filter, int page, int pageSize,
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

    // --- Mapping ---

    private static ConversationDocument ToDocument(ConversationDto dto)
    {
        return new ConversationDocument
        {
            Id = string.IsNullOrEmpty(dto.Id) ? MongoDB.Bson.ObjectId.GenerateNewId().ToString() : dto.Id,
            UserId = dto.UserId,
            ReaderId = dto.ReaderId,
            Status = dto.Status,
            LastMessageAt = dto.LastMessageAt,
            OfferExpiresAt = dto.OfferExpiresAt,
            UnreadCount = new UnreadCount { User = dto.UnreadCountUser, Reader = dto.UnreadCountReader },
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    private static ConversationDto ToDto(ConversationDocument doc)
    {
        return new ConversationDto
        {
            Id = doc.Id,
            UserId = doc.UserId,
            ReaderId = doc.ReaderId,
            Status = doc.Status,
            LastMessageAt = doc.LastMessageAt,
            OfferExpiresAt = doc.OfferExpiresAt,
            UnreadCountUser = doc.UnreadCount.User,
            UnreadCountReader = doc.UnreadCount.Reader,
            CreatedAt = doc.CreatedAt,
            UpdatedAt = doc.UpdatedAt
        };
    }
}

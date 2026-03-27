using MongoDB.Driver;
using MongoDB.Bson;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoConversationRepository : IConversationRepository
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

    public async Task<ConversationDto?> GetActiveByParticipantsAsync(
        string userId,
        string readerId,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.UserId, userId),
            Builders<ConversationDocument>.Filter.Eq(c => c.ReaderId, readerId),
            Builders<ConversationDocument>.Filter.In(c => c.Status, new[]
            {
                ConversationStatus.Pending,
                ConversationStatus.AwaitingAcceptance,
                ConversationStatus.Ongoing
            }),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        var doc = await _context.Conversations
            .Find(filter)
            .SortByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : ToDto(doc);
    }

    public Task<long> CountActiveByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.UserId, userId),
            Builders<ConversationDocument>.Filter.In(c => c.Status, new[]
            {
                ConversationStatus.Pending,
                ConversationStatus.AwaitingAcceptance,
                ConversationStatus.Ongoing
            }),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        return _context.Conversations.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyList<ConversationDto>> GetConversationsAwaitingCompletionResolutionAsync(
        DateTime dueAtUtc,
        int limit = 200,
        CancellationToken cancellationToken = default)
    {
        var normalizedLimit = limit <= 0 ? 200 : Math.Min(limit, 1000);
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.Status, ConversationStatus.Ongoing),
            Builders<ConversationDocument>.Filter.Ne(c => c.Confirm, null),
            Builders<ConversationDocument>.Filter.Ne("confirm.auto_resolve_at", BsonNull.Value),
            Builders<ConversationDocument>.Filter.Lte("confirm.auto_resolve_at", dueAtUtc),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        var docs = await _context.Conversations
            .Find(filter)
            .SortBy(c => c.Confirm!.AutoResolveAt)
            .Limit(normalizedLimit)
            .ToListAsync(cancellationToken);

        return docs.Select(ToDto).ToList();
    }

    public async Task UpdateAsync(ConversationDto conversation, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(conversation);
        doc.UpdatedAt = DateTime.UtcNow;
        var filter = Builders<ConversationDocument>.Filter.Eq(c => c.Id, doc.Id);
        await _context.Conversations.ReplaceOneAsync(filter, doc, cancellationToken: cancellationToken);
    }
}

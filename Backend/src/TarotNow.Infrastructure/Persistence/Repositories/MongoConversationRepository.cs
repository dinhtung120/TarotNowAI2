using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
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
            Builders<ConversationDocument>.Filter.In(c => c.Status, new[] { "pending", "active" }),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        var doc = await _context.Conversations
            .Find(filter)
            .SortByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : ToDto(doc);
    }

    public async Task UpdateAsync(ConversationDto conversation, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(conversation);
        doc.UpdatedAt = DateTime.UtcNow;
        var filter = Builders<ConversationDocument>.Filter.Eq(c => c.Id, doc.Id);
        await _context.Conversations.ReplaceOneAsync(filter, doc, cancellationToken: cancellationToken);
    }
}

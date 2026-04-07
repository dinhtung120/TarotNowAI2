

using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoChatMessageRepository : IChatMessageRepository
{
    private readonly MongoDbContext _context;

    public MongoChatMessageRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<ChatMessageDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ChatMessageDocument>.Filter.And(
            Builders<ChatMessageDocument>.Filter.Eq(m => m.Id, id),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false));

        var doc = await _context.ChatMessages.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : ToDto(doc);
    }

        public async Task AddAsync(ChatMessageDto message, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(message);
        await _context.ChatMessages.InsertOneAsync(doc, cancellationToken: cancellationToken);
        message.Id = doc.Id; 
    }

        public async Task<long> MarkAsReadAsync(string conversationId, string readerId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ChatMessageDocument>.Filter.And(
            Builders<ChatMessageDocument>.Filter.Eq(m => m.ConversationId, conversationId),
            Builders<ChatMessageDocument>.Filter.Ne(m => m.SenderId, readerId), 
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsRead, false),      
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false));   

        var update = Builders<ChatMessageDocument>.Update
            .Set(m => m.IsRead, true)
            .Set(m => m.UpdatedAt, DateTime.UtcNow);

        var result = await _context.ChatMessages.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }

        public async Task UpdateFlagAsync(string messageId, bool isFlagged, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ChatMessageDocument>.Filter.Eq(m => m.Id, messageId);
        var update = Builders<ChatMessageDocument>.Update
            .Set(m => m.IsFlagged, isFlagged)
            .Set(m => m.UpdatedAt, DateTime.UtcNow);

        await _context.ChatMessages.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<ChatMessageDto>> GetLatestMessagesAsync(IEnumerable<string> conversationIds, CancellationToken cancellationToken = default)
    {
        var ids = conversationIds.ToList();
        if (ids.Count == 0) return Enumerable.Empty<ChatMessageDto>();

        var pipeline = new EmptyPipelineDefinition<ChatMessageDocument>()
            .Match(Builders<ChatMessageDocument>.Filter.And(
                Builders<ChatMessageDocument>.Filter.In(m => m.ConversationId, ids),
                Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false)))
            .Sort(Builders<ChatMessageDocument>.Sort.Combine(
                Builders<ChatMessageDocument>.Sort.Descending(m => m.ConversationId),
                Builders<ChatMessageDocument>.Sort.Descending(m => m.CreatedAt)))
            .Group(m => m.ConversationId, g => g.First())
            .Project(m => m); 

        var docs = await _context.ChatMessages.Aggregate(pipeline, cancellationToken: cancellationToken).ToListAsync(cancellationToken);
        return docs.Select(ToDto);
    }
}

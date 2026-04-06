using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoCallSessionRepository : ICallSessionRepository
{
    private readonly MongoDbContext _context;

    public MongoCallSessionRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CallSessionDto session, CancellationToken ct = default)
    {
        var document = CallSessionDocumentMapper.ToDocument(session);
        try
        {
            await _context.CallSessions.InsertOneAsync(document, cancellationToken: ct);
        }
        catch (MongoWriteException ex) when (ex.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            throw new InvalidOperationException("Đang có một cuộc gọi active khác trong cuộc trò chuyện này.");
        }

        session.Id = document.Id;
    }

    public async Task<CallSessionDto?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        var doc = await _context.CallSessions.Find(x => x.Id == id).FirstOrDefaultAsync(ct);
        return doc == null ? null : CallSessionDocumentMapper.ToDto(doc);
    }

    public async Task<CallSessionDto?> GetActiveByConversationAsync(string conversationId, CancellationToken ct = default)
    {
        var filter = Builders<CallSessionDocument>.Filter.Eq(x => x.ConversationId, conversationId) &
                     Builders<CallSessionDocument>.Filter.In(x => x.Status, new[] { "requested", "accepted" });

        var doc = await _context.CallSessions.Find(filter).FirstOrDefaultAsync(ct);
        return doc == null ? null : CallSessionDocumentMapper.ToDto(doc);
    }

    public async Task<IEnumerable<CallSessionDto>> GetActiveByConversationIdsAsync(IEnumerable<string> conversationIds, CancellationToken ct = default)
    {
        var ids = conversationIds.ToList();
        if (ids.Count == 0) return Enumerable.Empty<CallSessionDto>();

        var filter = Builders<CallSessionDocument>.Filter.In(x => x.ConversationId, ids) &
                     Builders<CallSessionDocument>.Filter.In(x => x.Status, new[] { "requested", "accepted" });

        var docs = await _context.CallSessions.Find(filter).ToListAsync(ct);
        return docs.Select(CallSessionDocumentMapper.ToDto);
    }

}

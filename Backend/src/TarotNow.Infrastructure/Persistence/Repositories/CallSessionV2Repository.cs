using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class CallSessionV2Repository : ICallSessionV2Repository
{
    private readonly MongoDbContext _context;

    public CallSessionV2Repository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CallSessionV2Dto session, CancellationToken ct = default)
    {
        var document = ToDocument(session);
        await _context.CallSessionsV2.InsertOneAsync(document, cancellationToken: ct);
        session.Id = document.Id;
    }

    public async Task<CallSessionV2Dto?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        var document = await _context.CallSessionsV2.Find(x => x.Id == id).FirstOrDefaultAsync(ct);
        return document == null ? null : ToDto(document);
    }

    public async Task<CallSessionV2Dto?> GetByRoomNameAsync(string roomName, CancellationToken ct = default)
    {
        var document = await _context.CallSessionsV2.Find(x => x.RoomName == roomName).FirstOrDefaultAsync(ct);
        return document == null ? null : ToDto(document);
    }

    public async Task<CallSessionV2Dto?> GetActiveByConversationAsync(string conversationId, CancellationToken ct = default)
    {
        var filter = Builders<CallSessionV2Document>.Filter.Eq(x => x.ConversationId, conversationId)
            & Builders<CallSessionV2Document>.Filter.In(x => x.Status, CallSessionV2Statuses.ActiveStates);

        var document = await _context.CallSessionsV2.Find(filter).FirstOrDefaultAsync(ct);
        return document == null ? null : ToDto(document);
    }

    public async Task<IReadOnlyList<CallSessionV2Dto>> ListStaleByStatusAsync(
        IReadOnlyCollection<string> statuses,
        DateTime updatedBeforeUtc,
        int limit,
        CancellationToken ct = default)
    {
        if (statuses.Count == 0 || limit <= 0)
        {
            return Array.Empty<CallSessionV2Dto>();
        }

        var filter = Builders<CallSessionV2Document>.Filter.In(x => x.Status, statuses)
            & Builders<CallSessionV2Document>.Filter.Lt(x => x.UpdatedAt, updatedBeforeUtc);

        var documents = await _context.CallSessionsV2
            .Find(filter)
            .SortBy(x => x.UpdatedAt)
            .Limit(limit)
            .ToListAsync(ct);

        return documents.Select(ToDto).ToArray();
    }
}

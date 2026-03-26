using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoReadingSessionRepository : IReadingSessionRepository
{
    private readonly MongoDbContext _mongoContext;
    private readonly ApplicationDbContext _pgContext;

    public MongoReadingSessionRepository(MongoDbContext mongoContext, ApplicationDbContext pgContext)
    {
        _mongoContext = mongoContext;
        _pgContext = pgContext;
    }

    private static FilterDefinition<ReadingSessionDocument> BuildIdFilter(string id)
    {
        return ObjectId.TryParse(id, out var oid)
            ? Builders<ReadingSessionDocument>.Filter.Eq("_id", oid)
            : Builders<ReadingSessionDocument>.Filter.Eq("_id", id);
    }

    private static List<DrawnCard> ParseDrawnCards(string? cardsDrawnJson)
    {
        var cardIds = System.Text.Json.JsonSerializer.Deserialize<int[]>(cardsDrawnJson ?? "[]")
            ?? Array.Empty<int>();

        return cardIds
            .Select((cardId, idx) => new DrawnCard
            {
                CardId = cardId,
                Position = idx,
                IsReversed = false
            })
            .ToList();
    }
}

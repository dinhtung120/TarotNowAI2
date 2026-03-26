using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoReadingSessionRepository
{
    public async Task<ReadingSession> CreateAsync(ReadingSession session, CancellationToken cancellationToken = default)
    {
        var doc = new ReadingSessionDocument
        {
            Id = ObjectId.TryParse(session.Id, out var oid) ? oid : session.Id,
            UserId = session.UserId,
            SpreadType = session.SpreadType,
            Question = session.Question,
            AiStatus = session.IsCompleted ? "completed" : "pending",
            CreatedAt = session.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        if (session.IsCompleted)
        {
            doc.DrawnCards = ParseDrawnCards(session.CardsDrawn);
        }

        if (session.CurrencyUsed != null && session.AmountCharged > 0)
        {
            doc.Cost = new SessionCost
            {
                Currency = session.CurrencyUsed,
                Amount = session.AmountCharged
            };
        }

        await _mongoContext.ReadingSessions.InsertOneAsync(doc, cancellationToken: cancellationToken);
        return session;
    }

    public async Task UpdateAsync(ReadingSession session, CancellationToken cancellationToken = default)
    {
        var update = Builders<ReadingSessionDocument>.Update.Set(r => r.UpdatedAt, DateTime.UtcNow);

        if (session.IsCompleted)
        {
            update = update
                .Set(r => r.DrawnCards, ParseDrawnCards(session.CardsDrawn))
                .Set(r => r.AiStatus, "completed");
        }

        if (!string.IsNullOrEmpty(session.AiSummary))
        {
            update = update.Set(r => r.AiResult, new AiResult { Summary = session.AiSummary });
        }

        if (session.Followups != null && session.Followups.Any())
        {
            var mappedFollowups = session.Followups.Select(f => new FollowupEntry
            {
                Question = f.Question,
                Answer = f.Answer
            }).ToList();
            update = update.Set(r => r.Followups, mappedFollowups);
        }

        await _mongoContext.ReadingSessions.UpdateOneAsync(
            BuildIdFilter(session.Id),
            update,
            cancellationToken: cancellationToken);
    }
}

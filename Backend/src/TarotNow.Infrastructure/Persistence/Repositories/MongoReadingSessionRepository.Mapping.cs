using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoReadingSessionRepository
{
    private static ReadingSession MapToEntity(ReadingSessionDocument doc)
    {
        var isCompleted = string.Equals(doc.AiStatus, "completed", StringComparison.OrdinalIgnoreCase);

        var cardsJson = doc.DrawnCards != null && doc.DrawnCards.Count > 0
            ? System.Text.Json.JsonSerializer.Serialize(
                doc.DrawnCards
                    .OrderBy(c => c.Position)
                    .Select(c => c.CardId)
                    .ToArray())
            : null;

        var followups = doc.Followups?.Select(f => new ReadingFollowup
        {
            Question = f.Question,
            Answer = f.Answer
        }).ToList();

        return ReadingSession.Rehydrate(new ReadingSessionSnapshot
        {
            Id = doc.Id?.ToString() ?? string.Empty,
            UserId = doc.UserId,
            SpreadType = doc.SpreadType,
            Question = doc.Question,
            CardsDrawn = cardsJson,
            CurrencyUsed = doc.Cost?.Currency,
            AmountCharged = doc.Cost?.Amount ?? 0,
            IsCompleted = isCompleted,
            CreatedAt = doc.CreatedAt,
            CompletedAt = isCompleted ? doc.UpdatedAt : null,
            AiSummary = doc.AiResult?.Summary,
            Followups = followups
        });
    }
}

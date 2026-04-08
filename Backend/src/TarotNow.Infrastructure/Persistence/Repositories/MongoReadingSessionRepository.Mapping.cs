using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial mapping reading session document về domain entity.
public partial class MongoReadingSessionRepository
{
    /// <summary>
    /// Map ReadingSessionDocument sang aggregate ReadingSession.
    /// Luồng xử lý: xác định trạng thái completed, chuyển drawn cards thành JSON cardsDrawn và rehydrate từ snapshot.
    /// </summary>
    private static ReadingSession MapToEntity(ReadingSessionDocument doc)
    {
        var isCompleted = string.Equals(doc.AiStatus, "completed", StringComparison.OrdinalIgnoreCase);
        // Trạng thái completed dựa trên ai_status để đồng bộ với nguồn dữ liệu Mongo.

        var cardsJson = doc.DrawnCards != null && doc.DrawnCards.Count > 0
            ? System.Text.Json.JsonSerializer.Serialize(
                doc.DrawnCards
                    .OrderBy(c => c.Position)
                    .Select(c => c.CardId)
                    .ToArray())
            : null;
        // Giữ thứ tự theo position trước khi serialize để snapshot domain ổn định.

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

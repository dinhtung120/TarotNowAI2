using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý lệnh tạo/cập nhật reading session.
public partial class MongoReadingSessionRepository
{
    /// <summary>
    /// Tạo mới reading session trên Mongo.
    /// Luồng xử lý: map entity domain sang document, bổ sung drawn cards/cost khi có dữ liệu rồi insert.
    /// </summary>
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
            // Session completed cần lưu bộ bài rút để hỗ trợ lịch sử xem lại.
        }

        if (session.CurrencyUsed != null && session.AmountCharged > 0)
        {
            doc.Cost = new SessionCost
            {
                Currency = session.CurrencyUsed,
                Amount = session.AmountCharged
            };
            // Chỉ lưu cost khi có giao dịch thực tế để tránh noise dữ liệu miễn phí.
        }

        await _mongoContext.ReadingSessions.InsertOneAsync(doc, cancellationToken: cancellationToken);
        return session;
    }

    /// <summary>
    /// Cập nhật reading session đã tồn tại.
    /// Luồng xử lý: dựng update động theo trạng thái hoàn tất, ai summary và followups rồi update theo id.
    /// </summary>
    public async Task UpdateAsync(ReadingSession session, CancellationToken cancellationToken = default)
    {
        var update = Builders<ReadingSessionDocument>.Update.Set(r => r.UpdatedAt, DateTime.UtcNow);

        if (session.IsCompleted)
        {
            update = update
                .Set(r => r.DrawnCards, ParseDrawnCards(session.CardsDrawn))
                .Set(r => r.AiStatus, "completed");
            // Khi complete phải đồng bộ cả dữ liệu bài rút và trạng thái AI.
        }

        if (!string.IsNullOrEmpty(session.AiSummary))
        {
            update = update.Set(r => r.AiResult, new AiResult { Summary = session.AiSummary });
            // Chỉ set AiResult khi summary thực sự có nội dung.
        }

        if (session.Followups != null && session.Followups.Any())
        {
            var mappedFollowups = session.Followups.Select(f => new FollowupEntry
            {
                Question = f.Question,
                Answer = f.Answer
            }).ToList();
            update = update.Set(r => r.Followups, mappedFollowups);
            // Ghi đè danh sách followups bằng snapshot mới nhất để tránh lệch dữ liệu.
        }

        await _mongoContext.ReadingSessions.UpdateOneAsync(
            BuildIdFilter(session.Id),
            update,
            cancellationToken: cancellationToken);
    }
}

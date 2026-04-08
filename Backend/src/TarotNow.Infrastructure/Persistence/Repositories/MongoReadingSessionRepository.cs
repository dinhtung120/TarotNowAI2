using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository chính cho reading sessions, kết hợp Mongo + PostgreSQL theo nhu cầu nghiệp vụ.
public partial class MongoReadingSessionRepository : IReadingSessionRepository
{
    // Mongo context lưu dữ liệu phiên đọc.
    private readonly MongoDbContext _mongoContext;
    // PostgreSQL context truy vấn AiRequests liên quan phiên đọc.
    private readonly ApplicationDbContext _pgContext;

    /// <summary>
    /// Khởi tạo repository reading session.
    /// Luồng xử lý: nhận đồng thời MongoDbContext và ApplicationDbContext để xử lý luồng truy vấn lai.
    /// </summary>
    public MongoReadingSessionRepository(MongoDbContext mongoContext, ApplicationDbContext pgContext)
    {
        _mongoContext = mongoContext;
        _pgContext = pgContext;
    }

    /// <summary>
    /// Dựng filter theo id cho reading session.
    /// Luồng xử lý: thử parse ObjectId, nếu parse được thì lọc theo ObjectId, ngược lại fallback chuỗi để tương thích dữ liệu cũ.
    /// </summary>
    private static FilterDefinition<ReadingSessionDocument> BuildIdFilter(string id)
    {
        return ObjectId.TryParse(id, out var oid)
            ? Builders<ReadingSessionDocument>.Filter.Eq("_id", oid)
            : Builders<ReadingSessionDocument>.Filter.Eq("_id", id);
        // Edge case: một số bản ghi legacy có _id dạng string nên cần fallback để không mất truy cập.
    }

    /// <summary>
    /// Parse JSON cardsDrawn về danh sách DrawnCard.
    /// Luồng xử lý: deserialize mảng cardId và gán position tuần tự, mặc định is_reversed=false.
    /// </summary>
    private static List<DrawnCard> ParseDrawnCards(string? cardsDrawnJson)
    {
        var cardIds = System.Text.Json.JsonSerializer.Deserialize<int[]>(cardsDrawnJson ?? "[]")
            ?? Array.Empty<int>();
        // Khi JSON null/sai định dạng nhẹ, fallback mảng rỗng để giữ luồng xử lý ổn định.

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

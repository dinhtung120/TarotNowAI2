using MongoDB.Driver;
using MongoDB.Bson;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// MongoDB implementation cho IReadingSessionRepository.
///
/// THAY THẾ ReadingSessionRepository (EF Core / PostgreSQL) trước đó.
/// Phiên đọc bài giờ lưu trong MongoDB collection "reading_sessions".
///
/// Lưu ý quan trọng:
/// - StartPaidSessionAtomicAsync vẫn dùng IWalletRepository (PostgreSQL)
///   để trừ tiền, nhưng tạo session trong MongoDB.
/// - GetSessionWithAiRequestsAsync vẫn query AiRequests từ PostgreSQL
///   (cross-DB query) vì ai_requests là bảng PostgreSQL.
/// </summary>
public class MongoReadingSessionRepository : IReadingSessionRepository
{
    private readonly MongoDbContext _mongoContext;
    private readonly IWalletRepository _walletRepository;
    private readonly ApplicationDbContext _pgContext; // Cần cho cross-DB query (AiRequests)

    public MongoReadingSessionRepository(
        MongoDbContext mongoContext,
        IWalletRepository walletRepository,
        ApplicationDbContext pgContext)
    {
        _mongoContext = mongoContext;
        _walletRepository = walletRepository;
        _pgContext = pgContext;
    }

    /// <summary>Tạo phiên đọc bài mới trong MongoDB.</summary>
    public async Task<ReadingSession> CreateAsync(ReadingSession session, CancellationToken cancellationToken = default)
    {
        // Tạo document MongoDB từ Domain Entity
        var doc = new ReadingSessionDocument
        {
            // Thử convert sang ObjectId nếu Id là 24 ký tự hex hợp lệ
            Id = ObjectId.TryParse(session.Id, out var oid) ? (object)oid : session.Id,
            UserId = session.UserId,
            SpreadType = session.SpreadType,
            Question = session.Question,
            AiStatus = session.IsCompleted ? "completed" : "pending",
            CreatedAt = session.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        // Nếu session đã complete → map drawn cards
        if (session.IsCompleted && session.CardsDrawn != null)
        {
            var cardIds = System.Text.Json.JsonSerializer.Deserialize<int[]>(session.CardsDrawn) ?? Array.Empty<int>();
            doc.DrawnCards = cardIds.Select((cardId, idx) => new DrawnCard
            {
                CardId = cardId,
                Position = idx,
                IsReversed = false
            }).ToList();
        }

        // Thêm cost info nếu có
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

    /// <summary>
    /// Lấy phiên theo ID — trả về Domain Entity.
    /// Map từ MongoDB document → ReadingSession entity (reconstruct).
    /// </summary>
    public async Task<ReadingSession?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        // Logic lọc linh hoạt: Thử tìm theo ObjectId trước, sau đó là String
        FilterDefinition<ReadingSessionDocument> filter;
        if (ObjectId.TryParse(id, out var oid))
        {
            filter = Builders<ReadingSessionDocument>.Filter.Eq("_id", oid);
        }
        else 
        {
            filter = Builders<ReadingSessionDocument>.Filter.Eq("_id", id);
        }

        var doc = await _mongoContext.ReadingSessions
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : MapToEntity(doc);
    }

    /// <summary>Update phiên — merge changes vào MongoDB document.</summary>
    public async Task UpdateAsync(ReadingSession session, CancellationToken cancellationToken = default)
    {
        // Logic lọc linh hoạt cho ID
        FilterDefinition<ReadingSessionDocument> filter;
        if (ObjectId.TryParse(session.Id, out var oid))
        {
            filter = Builders<ReadingSessionDocument>.Filter.Eq("_id", oid);
        }
        else 
        {
            filter = Builders<ReadingSessionDocument>.Filter.Eq("_id", session.Id);
        }

        // Build update definition từ Domain Entity
        var update = Builders<ReadingSessionDocument>.Update
            .Set(r => r.UpdatedAt, DateTime.UtcNow);

        // Nếu session đã complete → cập nhật drawn_cards
        if (session.IsCompleted && session.CardsDrawn != null)
        {
            // Parse JSON string → DrawnCard objects
            var cardIds = System.Text.Json.JsonSerializer.Deserialize<int[]>(session.CardsDrawn) ?? Array.Empty<int>();
            var drawnCards = cardIds.Select((cardId, idx) => new DrawnCard
            {
                CardId = cardId,
                Position = idx,
                IsReversed = false // Default
            }).ToList();

            update = update
                .Set(r => r.DrawnCards, drawnCards)
                .Set(r => r.AiStatus, "completed");
        }

        await _mongoContext.ReadingSessions.UpdateOneAsync(
            filter,
            update,
            cancellationToken: cancellationToken);
    }

    /// <summary>Kiểm tra user đã rút daily card hôm nay (UTC) chưa.</summary>
    public async Task<bool> HasDrawnDailyCardAsync(Guid userId, DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();
        var startOfDay = utcNow.Date;
        var endOfDay = startOfDay.AddDays(1);

        var count = await _mongoContext.ReadingSessions.CountDocumentsAsync(
            r => r.UserId == userIdStr
                && r.SpreadType == SpreadType.Daily1Card
                && r.CreatedAt >= startOfDay
                && r.CreatedAt < endOfDay,
            cancellationToken: cancellationToken);

        return count > 0;
    }

    /// <summary>
    /// Atomic: Trừ tiền (PostgreSQL) + Tạo session (MongoDB).
    /// 
    /// Vì cross-DB (PostgreSQL wallet + MongoDB session), không thể dùng
    /// single ACID transaction. Thay vào đó dùng pattern:
    /// 1. Trừ tiền trước (PostgreSQL transaction)
    /// 2. Tạo session sau (MongoDB)
    /// 3. Nếu MongoDB fail → cần compensating transaction (refund)
    /// 
    /// Rủi ro thấp vì MongoDB insert rất hiếm khi fail.
    /// </summary>
    public async Task<(bool Success, string ErrorMessage)> StartPaidSessionAtomicAsync(
        Guid userId,
        string spreadType,
        ReadingSession session,
        long costGold,
        long costDiamond,
        CancellationToken cancellationToken = default)
    {
        var idempotencyKey = $"read_{session.Id}";

        try
        {
            // 1. Trừ tiền trong PostgreSQL (giữ nguyên logic cũ)
            if (costGold > 0)
            {
                await _walletRepository.DebitAsync(userId, CurrencyType.Gold, TransactionType.ReadingCostGold,
                    costGold, "Reading", $"Tarot_{spreadType}", $"Phiên rút Tarot {spreadType}",
                    null, idempotencyKey, cancellationToken);
            }
            if (costDiamond > 0)
            {
                await _walletRepository.DebitAsync(userId, CurrencyType.Diamond, TransactionType.ReadingCostDiamond,
                    costDiamond, "Reading", $"Tarot_{spreadType}", $"Phiên rút Tarot {spreadType}",
                    null, idempotencyKey, cancellationToken);
            }

            // 2. Tạo session trong MongoDB
            await CreateAsync(session, cancellationToken);

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            // COMPENSATING TRANSACTION:
            // Nếu Postgres đã debit thành công nhưng MongoDB insert fail -> Cần Refund ngay lập tức
            // để đảm bảo tính nguyên tử (Atomicity) liên database.
            try
            {
                if (costGold > 0)
                {
                    await _walletRepository.CreditAsync(userId, CurrencyType.Gold, TransactionType.ReadingRefund, costGold,
                        "System_Rollback", session.Id.ToString(), $"Hoàn trả Gold do lỗi hệ thống lưu Session",
                        null, $"refund_rollback_{session.Id}", CancellationToken.None);
                }
                if (costDiamond > 0)
                {
                    await _walletRepository.CreditAsync(userId, CurrencyType.Diamond, TransactionType.ReadingRefund, costDiamond,
                        "System_Rollback", session.Id.ToString(), $"Hoàn trả Diamond do lỗi hệ thống lưu Session",
                        null, $"refund_rollback_{session.Id}", CancellationToken.None);
                }
            }
            catch (Exception refundEx)
            {
                // CRITICAL: Nếu cả refund cũng fail, cần log lỗi cực kỳ nghiêm trọng để admin can thiệp thủ công.
                // Ở đây ta re-throw exception gốc nhưng đính kèm thông tin lỗi refund.
                throw new InvalidOperationException($"Lỗi nghiêm trọng: Trừ tiền thành công nhưng lưu Session thất bại và không thể Refund. Error: {ex.Message}. RefundError: {refundEx.Message}", ex);
            }

            return (false, ex.Message);
        }
    }

    /// <summary>Lấy lịch sử phiên theo user — phân trang.</summary>
    public async Task<(IEnumerable<ReadingSession> Items, int TotalCount)> GetSessionsByUserIdAsync(
        Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();
        var filter = Builders<ReadingSessionDocument>.Filter.Eq(r => r.UserId, userIdStr)
            & Builders<ReadingSessionDocument>.Filter.Eq(r => r.IsDeleted, false);

        var totalCount = await _mongoContext.ReadingSessions.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _mongoContext.ReadingSessions
            .Find(filter)
            .SortByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        var entities = docs.Select(MapToEntity).ToList();
        return (entities, (int)totalCount);
    }

    /// <summary>
    /// Lấy session + AI requests — cross-DB query.
    /// Session từ MongoDB, AiRequests từ PostgreSQL.
    /// </summary>
    public async Task<(ReadingSession ReadingSession, IEnumerable<AiRequest> AiRequests)?> GetSessionWithAiRequestsAsync(
        string sessionId, CancellationToken cancellationToken = default)
    {
        // 1. Lấy session từ MongoDB
        var session = await GetByIdAsync(sessionId, cancellationToken);
        if (session == null) return null;

        // 2. Lấy AiRequests từ PostgreSQL (cross-DB)
        var aiRequests = _pgContext.AiRequests
            .Where(a => a.ReadingSessionRef == sessionId)
            .OrderBy(a => a.CreatedAt)
            .AsEnumerable();

        return (session, aiRequests);
    }

    /// <summary>Admin: Lấy toàn bộ lịch sử phiên từ MongoDB — có hỗ trợ bộ lọc và phân trang.</summary>
    public async Task<(IEnumerable<ReadingSession> Items, int TotalCount)> GetAllSessionsAsync(
        int page, 
        int pageSize, 
        List<string>? userIds = null,
        string? spreadType = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var builder = Builders<ReadingSessionDocument>.Filter;
        var filter = builder.Eq(r => r.IsDeleted, false);

        if (userIds != null && userIds.Any())
        {
            filter &= builder.In(r => r.UserId, userIds);
        }

        if (!string.IsNullOrWhiteSpace(spreadType))
        {
            filter &= builder.Eq(r => r.SpreadType, spreadType);
        }

        if (startDate.HasValue)
        {
            filter &= builder.Gte(r => r.CreatedAt, startDate.Value);
        }

        if (endDate.HasValue)
        {
            filter &= builder.Lte(r => r.CreatedAt, endDate.Value);
        }

        var totalCount = await _mongoContext.ReadingSessions.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _mongoContext.ReadingSessions
            .Find(filter)
            .SortByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        var (items, totalCountResult) = (docs.Select(MapToEntity).ToList(), (int)totalCount);
        return (items, totalCountResult);
    }

    // ======================================================================
    // MAPPING: MongoDB Document → Domain Entity
    // ======================================================================

    /// <summary>
    /// Reconstruct Domain Entity từ MongoDB document.
    /// Giữ nguyên timestamp + trạng thái từ dữ liệu đã lưu.
    /// </summary>
    private static ReadingSession MapToEntity(ReadingSessionDocument doc)
    {
        var idStr = doc.Id?.ToString() ?? string.Empty;
        var isCompleted = string.Equals(doc.AiStatus, "completed", StringComparison.OrdinalIgnoreCase);
        var cardsJson = doc.DrawnCards != null && doc.DrawnCards.Count > 0
            ? System.Text.Json.JsonSerializer.Serialize(doc.DrawnCards
                .OrderBy(c => c.Position)
                .Select(c => c.CardId)
                .ToArray())
            : null;

        return ReadingSession.Rehydrate(
            id: idStr,
            userId: doc.UserId,
            spreadType: doc.SpreadType,
            question: doc.Question,
            cardsDrawn: cardsJson,
            currencyUsed: doc.Cost?.Currency,
            amountCharged: doc.Cost?.Amount ?? 0,
            isCompleted: isCompleted,
            createdAt: doc.CreatedAt,
            completedAt: isCompleted ? doc.UpdatedAt : null);
    }
}

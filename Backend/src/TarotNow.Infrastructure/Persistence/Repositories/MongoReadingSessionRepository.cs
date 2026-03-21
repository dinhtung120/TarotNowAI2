/*
 * FILE: MongoReadingSessionRepository.cs
 * MỤC ĐÍCH: Repository quản lý phiên đọc bài Tarot từ MongoDB (collection "reading_sessions").
 *   ĐÂY LÀ REPOSITORY PHỨC TẠP NHẤT — vì xử lý cross-database (MongoDB + PostgreSQL).
 *
 *   CÁC CHỨC NĂNG:
 *   → CreateAsync: tạo phiên mới trong MongoDB
 *   → GetByIdAsync: lấy phiên theo ID (hỗ trợ cả ObjectId lẫn string ID)
 *   → UpdateAsync: cập nhật phiên (drawn_cards, AI status)
 *   → HasDrawnDailyCardAsync: kiểm tra User đã rút daily card hôm nay chưa
 *   → StartPaidSessionAtomicAsync: PATTERN PHỨC TẠP — trừ tiền (PostgreSQL) + tạo session (MongoDB)
 *   → GetSessionsByUserIdAsync: lịch sử phiên của User (phân trang)
 *   → GetSessionWithAiRequestsAsync: cross-DB query (MongoDB session + PostgreSQL ai_requests)
 *   → GetAllSessionsAsync: Admin dashboard với bộ lọc nâng cao
 *
 *   CROSS-DB PATTERN:
 *   Vì dữ liệu nằm ở 2 database khác nhau (MongoDB + PostgreSQL), không thể dùng
 *   single ACID transaction. Thay vào đó dùng compensating transaction pattern:
 *   → Bước 1: Trừ tiền trong PostgreSQL (ACID transaction)
 *   → Bước 2: Tạo session trong MongoDB
 *   → Nếu bước 2 fail: hoàn tiền (refund) trong PostgreSQL
 */

using MongoDB.Driver;
using MongoDB.Bson;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IReadingSessionRepository — phiên đọc bài trong MongoDB.
/// Inject thêm IWalletRepository (PostgreSQL) và ApplicationDbContext (PostgreSQL)
/// vì cần xử lý cross-database.
/// </summary>
public class MongoReadingSessionRepository : IReadingSessionRepository
{
    private readonly MongoDbContext _mongoContext;
    // WalletRepository (PG) để trừ/hoàn tiền trong StartPaidSessionAtomicAsync
    private readonly IWalletRepository _walletRepository;
    // ApplicationDbContext (PG) để query AiRequests trong GetSessionWithAiRequestsAsync
    private readonly ApplicationDbContext _pgContext;

    public MongoReadingSessionRepository(
        MongoDbContext mongoContext,
        IWalletRepository walletRepository,
        ApplicationDbContext pgContext)
    {
        _mongoContext = mongoContext;
        _walletRepository = walletRepository;
        _pgContext = pgContext;
    }

    /// <summary>
    /// Tạo phiên đọc bài mới trong MongoDB.
    /// Map Domain Entity → MongoDB Document, bao gồm:
    ///   - Thử parse ID thành ObjectId (nếu là 24-char hex string)
    ///   - Map drawn_cards từ JSON string → List&lt;DrawnCard&gt;
    ///   - Map cost info (currency + amount) nếu phiên có phí
    /// </summary>
    public async Task<ReadingSession> CreateAsync(ReadingSession session, CancellationToken cancellationToken = default)
    {
        var doc = new ReadingSessionDocument
        {
            // Thử convert ID sang ObjectId, nếu không được thì giữ nguyên string
            Id = ObjectId.TryParse(session.Id, out var oid) ? (object)oid : session.Id,
            UserId = session.UserId,
            SpreadType = session.SpreadType,
            Question = session.Question,
            AiStatus = session.IsCompleted ? "completed" : "pending",
            CreatedAt = session.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        // Nếu phiên đã hoàn tất → parse JSON string lá bài → tạo DrawnCard objects
        if (session.IsCompleted && session.CardsDrawn != null)
        {
            var cardIds = System.Text.Json.JsonSerializer.Deserialize<int[]>(session.CardsDrawn) ?? Array.Empty<int>();
            doc.DrawnCards = cardIds.Select((cardId, idx) => new DrawnCard
            {
                CardId = cardId,
                Position = idx,
                IsReversed = false // Mặc định — sẽ cập nhật sau
            }).ToList();
        }

        // Thêm thông tin chi phí nếu có (phiên trả phí)
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
    /// Lấy phiên theo ID — hỗ trợ cả ObjectId và string ID.
    /// Tại sao cần flexible? → ID có thể là ObjectId (từ MongoDB insert) hoặc
    /// custom string (từ logic cũ). TryParse xử lý cả 2 trường hợp.
    /// </summary>
    public async Task<ReadingSession?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        // Linh hoạt: thử tìm bằng ObjectId trước, nếu không parse được thì tìm bằng string
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

    /// <summary>
    /// Cập nhật phiên — chỉ set các trường cần thay đổi (partial update).
    /// Không replace toàn bộ document → hiệu quả hơn khi document lớn.
    /// </summary>
    public async Task UpdateAsync(ReadingSession session, CancellationToken cancellationToken = default)
    {
        // Flexible ID filter (giống GetByIdAsync)
        FilterDefinition<ReadingSessionDocument> filter;
        if (ObjectId.TryParse(session.Id, out var oid))
        {
            filter = Builders<ReadingSessionDocument>.Filter.Eq("_id", oid);
        }
        else 
        {
            filter = Builders<ReadingSessionDocument>.Filter.Eq("_id", session.Id);
        }

        // Luôn cập nhật UpdatedAt
        var update = Builders<ReadingSessionDocument>.Update
            .Set(r => r.UpdatedAt, DateTime.UtcNow);

        // Nếu phiên đã hoàn tất → cập nhật drawn_cards + AI status
        if (session.IsCompleted && session.CardsDrawn != null)
        {
            var cardIds = System.Text.Json.JsonSerializer.Deserialize<int[]>(session.CardsDrawn) ?? Array.Empty<int>();
            var drawnCards = cardIds.Select((cardId, idx) => new DrawnCard
            {
                CardId = cardId,
                Position = idx,
                IsReversed = false
            }).ToList();

            update = update
                .Set(r => r.DrawnCards, drawnCards)
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
            filter,
            update,
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Kiểm tra User đã rút daily card HÔM NAY (UTC) chưa.
    /// Mỗi User chỉ được rút 1 lá miễn phí/ngày (spread_type = "daily_1").
    /// So sánh CreatedAt trong khoảng [startOfDay, endOfDay) UTC.
    /// </summary>
    public async Task<bool> HasDrawnDailyCardAsync(Guid userId, DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();
        var startOfDay = utcNow.Date;        // 00:00:00 UTC hôm nay
        var endOfDay = startOfDay.AddDays(1); // 00:00:00 UTC ngày mai

        var count = await _mongoContext.ReadingSessions.CountDocumentsAsync(
            r => r.UserId == userIdStr
                && r.SpreadType == SpreadType.Daily1Card
                && r.CreatedAt >= startOfDay
                && r.CreatedAt < endOfDay,
            cancellationToken: cancellationToken);

        return count > 0; // Đã rút ít nhất 1 lần → true
    }

    /// <summary>
    /// PATTERN PHỨC TẠP: Trừ tiền (PostgreSQL) + Tạo session (MongoDB) — cross-database atomic.
    ///
    /// Vì 2 database khác nhau, KHÔNG THỂ dùng single ACID transaction.
    /// Dùng COMPENSATING TRANSACTION pattern:
    ///   1. Trừ Gold/Diamond trong PostgreSQL (ACID transaction riêng)
    ///   2. Tạo reading session trong MongoDB
    ///   3. Nếu MongoDB insert fail → HOÀN TIỀN (refund) trong PostgreSQL
    ///   4. Nếu refund cũng fail → CRITICAL ERROR → log + throw → Admin can thiệp thủ công
    ///
    /// Rủi ro: MongoDB insert rất hiếm khi fail (chỉ khi database down hoàn toàn).
    /// Idempotency key: "read_{sessionId}" → chặn double-charge nếu retry.
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
        var goldDebited = false;
        var diamondDebited = false;

        try
        {
            // ===== BƯỚC 1: Trừ tiền trong PostgreSQL =====
            if (costGold > 0)
            {
                await _walletRepository.DebitAsync(userId, CurrencyType.Gold, TransactionType.ReadingCostGold,
                    costGold, "Reading", $"Tarot_{spreadType}", $"Phiên rút Tarot {spreadType}",
                    null, idempotencyKey, cancellationToken);
                goldDebited = true; // Đánh dấu đã trừ Gold thành công
            }
            if (costDiamond > 0)
            {
                await _walletRepository.DebitAsync(userId, CurrencyType.Diamond, TransactionType.ReadingCostDiamond,
                    costDiamond, "Reading", $"Tarot_{spreadType}", $"Phiên rút Tarot {spreadType}",
                    null, idempotencyKey, cancellationToken);
                diamondDebited = true; // Đánh dấu đã trừ Diamond thành công
            }

            // ===== BƯỚC 2: Tạo session trong MongoDB =====
            await CreateAsync(session, cancellationToken);

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            // ===== COMPENSATING TRANSACTION: Hoàn tiền nếu MongoDB fail =====
            // PostgreSQL đã trừ tiền nhưng MongoDB chưa tạo session → cần refund
            try
            {
                if (goldDebited)
                {
                    await _walletRepository.CreditAsync(userId, CurrencyType.Gold, TransactionType.ReadingRefund, costGold,
                        "System_Rollback", session.Id.ToString(), $"Hoàn trả Gold do lỗi hệ thống lưu Session",
                        null, $"refund_rollback_{session.Id}", CancellationToken.None);
                }
                if (diamondDebited)
                {
                    await _walletRepository.CreditAsync(userId, CurrencyType.Diamond, TransactionType.ReadingRefund, costDiamond,
                        "System_Rollback", session.Id.ToString(), $"Hoàn trả Diamond do lỗi hệ thống lưu Session",
                        null, $"refund_rollback_{session.Id}", CancellationToken.None);
                }
            }
            catch (Exception refundEx)
            {
                // ===== CRITICAL: Cả refund cũng fail =====
                // Trạng thái KHÔNG NHẤT QUÁN: tiền đã trừ, session chưa tạo, refund cũng fail.
                // Throw exception kèm cả 2 lỗi → Admin phải xử lý thủ công.
                throw new InvalidOperationException(
                    "Lỗi nghiêm trọng: giao dịch không nhất quán khi khởi tạo phiên đọc bài. Vui lòng liên hệ hỗ trợ.",
                    new AggregateException(ex, refundEx));
            }

            return (false, "start_paid_session_failed");
        }
    }

    /// <summary>
    /// Lịch sử phiên đọc bài của User — phân trang, mới nhất trước.
    /// Chỉ lấy phiên chưa xóa (IsDeleted = false).
    /// </summary>
    public async Task<(IEnumerable<ReadingSession> Items, int TotalCount)> GetSessionsByUserIdAsync(
        Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 200);

        var userIdStr = userId.ToString();
        var filter = Builders<ReadingSessionDocument>.Filter.Eq(r => r.UserId, userIdStr)
            & Builders<ReadingSessionDocument>.Filter.Eq(r => r.IsDeleted, false);

        var totalCount = await _mongoContext.ReadingSessions.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _mongoContext.ReadingSessions
            .Find(filter)
            .SortByDescending(r => r.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        var entities = docs.Select(MapToEntity).ToList();
        return (entities, (int)totalCount);
    }

    /// <summary>
    /// CROSS-DB QUERY: Lấy phiên (MongoDB) + AI requests liên quan (PostgreSQL).
    /// Vì ai_requests nằm trong PostgreSQL, cần query 2 database riêng biệt rồi ghép kết quả.
    /// </summary>
    public async Task<(ReadingSession ReadingSession, IEnumerable<AiRequest> AiRequests)?> GetSessionWithAiRequestsAsync(
        string sessionId, CancellationToken cancellationToken = default)
    {
        // 1. Lấy session từ MongoDB
        var session = await GetByIdAsync(sessionId, cancellationToken);
        if (session == null) return null;

        // 2. Lấy AiRequests từ PostgreSQL (cross-DB) — filter bằng ReadingSessionRef
        var aiRequests = _pgContext.AiRequests
            .Where(a => a.ReadingSessionRef == sessionId)
            .OrderBy(a => a.CreatedAt)
            .AsEnumerable();

        return (session, aiRequests);
    }

    /// <summary>
    /// Admin dashboard: lấy toàn bộ phiên với bộ lọc nâng cao.
    /// Filter: userIds (lọc theo user), spreadType, startDate/endDate (khoảng thời gian).
    /// </summary>
    public async Task<(IEnumerable<ReadingSession> Items, int TotalCount)> GetAllSessionsAsync(
        int page, 
        int pageSize, 
        List<string>? userIds = null,
        string? spreadType = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 200);

        var builder = Builders<ReadingSessionDocument>.Filter;
        var filter = builder.Eq(r => r.IsDeleted, false);

        // Filter theo danh sách User IDs (nếu có)
        if (userIds != null && userIds.Any())
        {
            filter &= builder.In(r => r.UserId, userIds);
        }

        // Filter theo loại trải bài
        if (!string.IsNullOrWhiteSpace(spreadType))
        {
            filter &= builder.Eq(r => r.SpreadType, spreadType);
        }

        // Filter theo khoảng thời gian
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
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        var (items, totalCountResult) = (docs.Select(MapToEntity).ToList(), (int)totalCount);
        return (items, totalCountResult);
    }

    // ==================== MAPPING ====================

    /// <summary>
    /// Reconstruct Domain Entity từ MongoDB Document.
    /// Dùng Entity.Rehydrate() factory method — giữ nguyên timestamp + trạng thái gốc.
    /// Parse DrawnCards → JSON string (vì Domain Entity lưu cards dạng JSON string).
    /// </summary>
    private static ReadingSession MapToEntity(ReadingSessionDocument doc)
    {
        var idStr = doc.Id?.ToString() ?? string.Empty;
        var isCompleted = string.Equals(doc.AiStatus, "completed", StringComparison.OrdinalIgnoreCase);
        // Chuyển DrawnCards list → JSON array string [1, 5, 22] (định dạng Domain Entity)
        var cardsJson = doc.DrawnCards != null && doc.DrawnCards.Count > 0
            ? System.Text.Json.JsonSerializer.Serialize(doc.DrawnCards
                .OrderBy(c => c.Position)
                .Select(c => c.CardId)
                .ToArray())
            : null;

        var followups = doc.Followups?.Select(f => new ReadingFollowup
        {
            Question = f.Question,
            Answer = f.Answer
        }).ToList();

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
            completedAt: isCompleted ? doc.UpdatedAt : null,
            aiSummary: doc.AiResult?.Summary,
            followups: followups);
    }
}

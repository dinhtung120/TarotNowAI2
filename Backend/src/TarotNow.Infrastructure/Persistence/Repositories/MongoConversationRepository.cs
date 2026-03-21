/*
 * FILE: MongoConversationRepository.cs
 * MỤC ĐÍCH: Repository quản lý cuộc hội thoại chat từ MongoDB (collection "conversations").
 *
 *   CÁC CHỨC NĂNG:
 *   → AddAsync: tạo cuộc hội thoại mới
 *   → GetByIdAsync: lấy theo ID
 *   → GetActiveByParticipantsAsync: tìm conversation đang active giữa 2 người (chặn trùng)
 *   → GetByUserIdPaginatedAsync: inbox của User (phân trang)
 *   → GetByReaderIdPaginatedAsync: inbox của Reader (phân trang)
 *   → UpdateAsync: cập nhật (đổi status, last_message_at, v.v.)
 */

using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IConversationRepository — đọc/ghi cuộc hội thoại từ MongoDB.
/// </summary>
public class MongoConversationRepository : IConversationRepository
{
    private readonly MongoDbContext _context;

    public MongoConversationRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>Tạo cuộc hội thoại mới, gán ObjectId vừa sinh về DTO.</summary>
    public async Task AddAsync(ConversationDto conversation, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(conversation);
        await _context.Conversations.InsertOneAsync(doc, cancellationToken: cancellationToken);
        conversation.Id = doc.Id;
    }

    /// <summary>Lấy cuộc hội thoại theo ID, chỉ lấy chưa xóa (IsDeleted = false).</summary>
    public async Task<ConversationDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.Id, id),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        var doc = await _context.Conversations.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : ToDto(doc);
    }

    /// <summary>
    /// Tìm conversation ĐANG HOẠT ĐỘNG giữa 2 người (User + Reader).
    /// Chỉ xét status = "pending" hoặc "active" (đang mở).
    /// Mục đích: chặn tạo duplicate — nếu đã có conversation active thì dùng lại.
    /// Lấy conversation mới nhất (SortByDescending CreatedAt) phòng trường hợp có nhiều.
    /// </summary>
    public async Task<ConversationDto?> GetActiveByParticipantsAsync(
        string userId, string readerId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.UserId, userId),
            Builders<ConversationDocument>.Filter.Eq(c => c.ReaderId, readerId),
            Builders<ConversationDocument>.Filter.In(c => c.Status, new[] { "pending", "active" }),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        var doc = await _context.Conversations.Find(filter)
            .SortByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : ToDto(doc);
    }

    /// <summary>
    /// Inbox User — lấy danh sách conversation của User, phân trang.
    /// Sắp xếp: conversation có tin mới nhất hiện trước (LastMessageAt DESC).
    /// </summary>
    public async Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByUserIdPaginatedAsync(
        string userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.UserId, userId),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        return await GetPaginatedInternal(filter, page, pageSize, cancellationToken);
    }

    /// <summary>Inbox Reader — tương tự inbox User nhưng lọc theo ReaderId.</summary>
    public async Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByReaderIdPaginatedAsync(
        string readerId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.ReaderId, readerId),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        return await GetPaginatedInternal(filter, page, pageSize, cancellationToken);
    }

    /// <summary>
    /// Truy vấn danh sách Hộp Thư Mới Gộp Chung cả Vai User Lẫn Reader.
    /// </summary>
    public async Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByParticipantIdPaginatedAsync(
        string participantId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Or(
                Builders<ConversationDocument>.Filter.Eq(c => c.UserId, participantId),
                Builders<ConversationDocument>.Filter.Eq(c => c.ReaderId, participantId)
            ),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false)
        );

        return await GetPaginatedInternal(filter, page, pageSize, cancellationToken);
    }

    /// <summary>
    /// Cập nhật conversation: map DTO → Document, set UpdatedAt, replace toàn bộ document.
    /// ReplaceOneAsync thay thế TOÀN BỘ document (không merge) → đảm bảo consistency.
    /// </summary>
    public async Task UpdateAsync(ConversationDto conversation, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(conversation);
        doc.UpdatedAt = DateTime.UtcNow;
        var filter = Builders<ConversationDocument>.Filter.Eq(c => c.Id, doc.Id);
        await _context.Conversations.ReplaceOneAsync(filter, doc, cancellationToken: cancellationToken);
    }

    // ==================== HELPER ====================

    /// <summary>
    /// Logic phân trang chung cho cả inbox User và inbox Reader.
    /// Sắp xếp: LastMessageAt DESC → CreatedAt DESC (conversation mới chat hiện trước).
    /// </summary>
    private async Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetPaginatedInternal(
        FilterDefinition<ConversationDocument> filter, int page, int pageSize,
        CancellationToken cancellationToken)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        var totalCount = await _context.Conversations.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        var docs = await _context.Conversations.Find(filter)
            .SortByDescending(c => c.LastMessageAt)   // Conversation có tin mới nhất lên trước
            .ThenByDescending(c => c.CreatedAt)        // Nếu chưa có tin → dùng CreatedAt
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        return (docs.Select(ToDto), totalCount);
    }

    // ==================== MAPPING ====================

    /// <summary>Map Application DTO → MongoDB Document.</summary>
    private static ConversationDocument ToDocument(ConversationDto dto)
    {
        return new ConversationDocument
        {
            Id = string.IsNullOrEmpty(dto.Id) ? MongoDB.Bson.ObjectId.GenerateNewId().ToString() : dto.Id,
            UserId = dto.UserId,
            ReaderId = dto.ReaderId,
            Status = dto.Status,
            LastMessageAt = dto.LastMessageAt,
            OfferExpiresAt = dto.OfferExpiresAt,
            UnreadCount = new UnreadCount { User = dto.UnreadCountUser, Reader = dto.UnreadCountReader },
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    /// <summary>Map MongoDB Document → Application DTO.</summary>
    private static ConversationDto ToDto(ConversationDocument doc)
    {
        return new ConversationDto
        {
            Id = doc.Id,
            UserId = doc.UserId,
            ReaderId = doc.ReaderId,
            Status = doc.Status,
            LastMessageAt = doc.LastMessageAt,
            OfferExpiresAt = doc.OfferExpiresAt,
            UnreadCountUser = doc.UnreadCount.User,
            UnreadCountReader = doc.UnreadCount.Reader,
            CreatedAt = doc.CreatedAt,
            UpdatedAt = doc.UpdatedAt
        };
    }
}

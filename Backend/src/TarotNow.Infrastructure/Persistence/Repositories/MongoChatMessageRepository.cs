/*
 * FILE: MongoChatMessageRepository.cs
 * MỤC ĐÍCH: Repository quản lý tin nhắn chat từ MongoDB (collection "chat_messages").
 *
 *   CÁC CHỨC NĂNG:
 *   → AddAsync: gửi tin nhắn mới (map DTO → Document → InsertOne)
 *   → GetByConversationIdPaginatedAsync: lấy tin nhắn theo cuộc hội thoại (phân trang)
 *   → MarkAsReadAsync: đánh dấu đã đọc tất cả tin của người khác gửi
 *
 *   MAPPING: DTO (Application) ↔ Document (Infrastructure) — 2 chiều.
 *   Application layer chỉ biết ChatMessageDto, không biết MongoDB Document.
 */

using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IChatMessageRepository — đọc/ghi tin nhắn chat từ MongoDB.
/// </summary>
public class MongoChatMessageRepository : IChatMessageRepository
{
    private readonly MongoDbContext _context;

    public MongoChatMessageRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gửi tin nhắn mới: map DTO → Document, insert vào MongoDB, gán ID mới về DTO.
    /// Sau khi insert, MongoDB tự sinh ObjectId → gán ngược lại vào message.Id
    /// để caller (Command handler) biết ID tin nhắn vừa tạo.
    /// </summary>
    public async Task AddAsync(ChatMessageDto message, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(message);
        await _context.ChatMessages.InsertOneAsync(doc, cancellationToken: cancellationToken);
        message.Id = doc.Id; // Gán ObjectId vừa sinh ngược về DTO
    }

    /// <summary>
    /// Lấy tin nhắn theo cuộc hội thoại, phân trang, mới nhất trước.
    /// Frontend sẽ reverse lại danh sách để hiển thị đúng timeline (cũ → mới).
    /// Chỉ lấy tin chưa bị xóa (IsDeleted = false).
    /// Trả về: (danh sách DTO, tổng số tin) — UI dùng totalCount cho pagination.
    /// </summary>
    public async Task<(IEnumerable<ChatMessageDto> Items, long TotalCount)> GetByConversationIdPaginatedAsync(
        string conversationId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 50 : Math.Min(pageSize, 200);

        // Filter: đúng conversation + chưa xóa
        var filter = Builders<ChatMessageDocument>.Filter.And(
            Builders<ChatMessageDocument>.Filter.Eq(m => m.ConversationId, conversationId),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false));

        var totalCount = await _context.ChatMessages.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _context.ChatMessages.Find(filter)
            .SortByDescending(m => m.CreatedAt) // Mới nhất trước (frontend reverse lại)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        return (docs.Select(ToDto), totalCount);
    }

    /// <summary>
    /// Đánh dấu ĐÃ ĐỌC tất cả tin nhắn của NGƯỜI KHÁC gửi trong cuộc hội thoại.
    /// readerId = ID người đang đọc → cập nhật tin có sender_id ≠ readerId (tin của đối phương).
    /// Chỉ cập nhật tin chưa đọc (IsRead = false) và chưa xóa (IsDeleted = false).
    /// UpdateManyAsync: cập nhật hàng loạt trong 1 lần gọi (hiệu quả hơn loop từng tin).
    /// Trả về: số tin đã được mark (để cập nhật unread_count trên conversation).
    /// </summary>
    public async Task<long> MarkAsReadAsync(string conversationId, string readerId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ChatMessageDocument>.Filter.And(
            Builders<ChatMessageDocument>.Filter.Eq(m => m.ConversationId, conversationId),
            Builders<ChatMessageDocument>.Filter.Ne(m => m.SenderId, readerId), // Tin của đối phương
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsRead, false),      // Chưa đọc
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false));   // Chưa xóa

        var update = Builders<ChatMessageDocument>.Update
            .Set(m => m.IsRead, true)
            .Set(m => m.UpdatedAt, DateTime.UtcNow);

        var result = await _context.ChatMessages.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }

    // ==================== MAPPING ====================

    /// <summary>Map Application DTO → MongoDB Document (khi ghi).</summary>
    private static ChatMessageDocument ToDocument(ChatMessageDto dto)
    {
        return new ChatMessageDocument
        {
            // Nếu chưa có ID → tự sinh ObjectId mới
            Id = string.IsNullOrEmpty(dto.Id) ? MongoDB.Bson.ObjectId.GenerateNewId().ToString() : dto.Id,
            ConversationId = dto.ConversationId,
            SenderId = dto.SenderId,
            Type = dto.Type,
            Content = dto.Content,
            // PaymentPayload chỉ có khi type = "payment_offer"
            PaymentPayload = dto.PaymentPayload != null ? new ChatPaymentPayload
            {
                AmountDiamond = dto.PaymentPayload.AmountDiamond,
                ProposalId = dto.PaymentPayload.ProposalId,
                ExpiresAt = dto.PaymentPayload.ExpiresAt
            } : null,
            IsRead = dto.IsRead,
            CreatedAt = dto.CreatedAt
        };
    }

    /// <summary>Map MongoDB Document → Application DTO (khi đọc).</summary>
    private static ChatMessageDto ToDto(ChatMessageDocument doc)
    {
        return new ChatMessageDto
        {
            Id = doc.Id,
            ConversationId = doc.ConversationId,
            SenderId = doc.SenderId,
            Type = doc.Type,
            Content = doc.Content,
            PaymentPayload = doc.PaymentPayload != null ? new PaymentPayloadDto
            {
                AmountDiamond = doc.PaymentPayload.AmountDiamond,
                ProposalId = doc.PaymentPayload.ProposalId,
                ExpiresAt = doc.PaymentPayload.ExpiresAt
            } : null,
            IsRead = doc.IsRead,
            CreatedAt = doc.CreatedAt
        };
    }
}

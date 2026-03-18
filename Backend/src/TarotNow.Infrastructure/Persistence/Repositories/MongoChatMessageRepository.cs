using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository cho chat_messages collection (MongoDB).
/// Map giữa ChatMessageDto (Application) ↔ ChatMessageDocument (Infrastructure).
/// </summary>
public class MongoChatMessageRepository : IChatMessageRepository
{
    private readonly MongoDbContext _context;

    public MongoChatMessageRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ChatMessageDto message, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(message);
        await _context.ChatMessages.InsertOneAsync(doc, cancellationToken: cancellationToken);
        message.Id = doc.Id;
    }

    /// <summary>
    /// Lấy tin nhắn theo conversation — sort by created_at DESC.
    /// Frontend reverse lại để hiển thị đúng timeline.
    /// </summary>
    public async Task<(IEnumerable<ChatMessageDto> Items, long TotalCount)> GetByConversationIdPaginatedAsync(
        string conversationId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 50 : Math.Min(pageSize, 200);

        var filter = Builders<ChatMessageDocument>.Filter.And(
            Builders<ChatMessageDocument>.Filter.Eq(m => m.ConversationId, conversationId),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false));

        var totalCount = await _context.ChatMessages.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _context.ChatMessages.Find(filter)
            .SortByDescending(m => m.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        return (docs.Select(ToDto), totalCount);
    }

    /// <summary>
    /// Đánh dấu tin nhắn đã đọc — tất cả tin gửi bởi người KHÁC trong conversation.
    /// readerId = người đang đọc → update tin nhắn có sender_id != readerId.
    /// </summary>
    public async Task<long> MarkAsReadAsync(string conversationId, string readerId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ChatMessageDocument>.Filter.And(
            Builders<ChatMessageDocument>.Filter.Eq(m => m.ConversationId, conversationId),
            Builders<ChatMessageDocument>.Filter.Ne(m => m.SenderId, readerId),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsRead, false),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false));

        var update = Builders<ChatMessageDocument>.Update
            .Set(m => m.IsRead, true)
            .Set(m => m.UpdatedAt, DateTime.UtcNow);

        var result = await _context.ChatMessages.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }

    // --- Mapping ---

    private static ChatMessageDocument ToDocument(ChatMessageDto dto)
    {
        return new ChatMessageDocument
        {
            Id = string.IsNullOrEmpty(dto.Id) ? MongoDB.Bson.ObjectId.GenerateNewId().ToString() : dto.Id,
            ConversationId = dto.ConversationId,
            SenderId = dto.SenderId,
            Type = dto.Type,
            Content = dto.Content,
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

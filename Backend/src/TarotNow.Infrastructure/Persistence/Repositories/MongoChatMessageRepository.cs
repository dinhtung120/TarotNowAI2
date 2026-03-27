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
public partial class MongoChatMessageRepository : IChatMessageRepository
{
    private readonly MongoDbContext _context;

    public MongoChatMessageRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<ChatMessageDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ChatMessageDocument>.Filter.And(
            Builders<ChatMessageDocument>.Filter.Eq(m => m.Id, id),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false));

        var doc = await _context.ChatMessages.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : ToDto(doc);
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

    /// <summary>
    /// Cập nhật cờ IsFlagged cho một Message khi vi phạm (Auto Moderation).
    /// Gắn nhãn để phân loại tin nhắn đen.
    /// </summary>
    public async Task UpdateFlagAsync(string messageId, bool isFlagged, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ChatMessageDocument>.Filter.Eq(m => m.Id, messageId);
        var update = Builders<ChatMessageDocument>.Update
            .Set(m => m.IsFlagged, isFlagged)
            .Set(m => m.UpdatedAt, DateTime.UtcNow);

        await _context.ChatMessages.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<ChatMessageDto>> GetLatestMessagesAsync(IEnumerable<string> conversationIds, CancellationToken cancellationToken = default)
    {
        var ids = conversationIds.ToList();
        if (ids.Count == 0) return Enumerable.Empty<ChatMessageDto>();

        var pipeline = new EmptyPipelineDefinition<ChatMessageDocument>()
            .Match(Builders<ChatMessageDocument>.Filter.And(
                Builders<ChatMessageDocument>.Filter.In(m => m.ConversationId, ids),
                Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false)))
            .Sort(Builders<ChatMessageDocument>.Sort.Combine(
                Builders<ChatMessageDocument>.Sort.Descending(m => m.ConversationId),
                Builders<ChatMessageDocument>.Sort.Descending(m => m.CreatedAt)))
            .Group(m => m.ConversationId, g => g.First())
            .Project(m => m); // First() returns the Document

        var docs = await _context.ChatMessages.Aggregate(pipeline, cancellationToken: cancellationToken).ToListAsync(cancellationToken);
        return docs.Select(ToDto);
    }
}

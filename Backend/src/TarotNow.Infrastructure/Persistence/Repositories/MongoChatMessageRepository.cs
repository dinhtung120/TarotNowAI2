

using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository chính xử lý message chat trên MongoDB.
public partial class MongoChatMessageRepository : IChatMessageRepository
{
    // Mongo context truy cập collection chat_messages.
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository chat message.
    /// Luồng xử lý: nhận MongoDbContext từ DI để thao tác dữ liệu hội thoại.
    /// </summary>
    public MongoChatMessageRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy message theo id nếu chưa bị xóa mềm.
    /// Luồng xử lý: filter id + is_deleted=false, map document sang DTO khi tồn tại.
    /// </summary>
    public async Task<ChatMessageDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ChatMessageDocument>.Filter.And(
            Builders<ChatMessageDocument>.Filter.Eq(m => m.Id, id),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false));

        var doc = await _context.ChatMessages.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : ToDto(doc);
    }

    /// <summary>
    /// Thêm mới message vào hội thoại.
    /// Luồng xử lý: map DTO sang document, insert Mongo và cập nhật lại id cho DTO đầu vào.
    /// </summary>
    public async Task AddAsync(ChatMessageDto message, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(message);
        try
        {
            await _context.ChatMessages.InsertOneAsync(doc, cancellationToken: cancellationToken);
            message.Id = doc.Id;
            // Đồng bộ id phát sinh để các bước push realtime dùng cùng message id.
        }
        catch (MongoWriteException exception) when (CanRecoverSystemEventDuplicate(exception, doc))
        {
            message.Id = await ResolveExistingSystemEventMessageIdAsync(doc, cancellationToken) ?? doc.Id;
            // Trường hợp retry outbox đã chèn trước đó: trả về id cũ để caller tiếp tục idempotent.
        }
    }

    /// <summary>
    /// Đánh dấu đã đọc cho toàn bộ message phù hợp trong hội thoại.
    /// Luồng xử lý: chỉ update message không phải do reader hiện tại gửi, chưa đọc và chưa xóa.
    /// </summary>
    public async Task<long> MarkAsReadAsync(string conversationId, string readerId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ChatMessageDocument>.Filter.And(
            Builders<ChatMessageDocument>.Filter.Eq(m => m.ConversationId, conversationId),
            Builders<ChatMessageDocument>.Filter.Ne(m => m.SenderId, readerId),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsRead, false),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false));
        // Business rule: không đánh dấu đã đọc các message do chính actor gửi.

        var update = Builders<ChatMessageDocument>.Update
            .Set(m => m.IsRead, true)
            .Set(m => m.UpdatedAt, DateTime.UtcNow);

        var result = await _context.ChatMessages.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }

    /// <summary>
    /// Cập nhật cờ moderation cho message.
    /// Luồng xử lý: set IsFlagged và updated_at theo messageId.
    /// </summary>
    public async Task UpdateFlagAsync(string messageId, bool isFlagged, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ChatMessageDocument>.Filter.Eq(m => m.Id, messageId);
        var update = Builders<ChatMessageDocument>.Update
            .Set(m => m.IsFlagged, isFlagged)
            .Set(m => m.UpdatedAt, DateTime.UtcNow);

        await _context.ChatMessages.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Lấy message mới nhất của từng conversation trong một danh sách conversationIds.
    /// Luồng xử lý: dùng aggregate match-sort-group-first để lấy latest message mỗi hội thoại.
    /// </summary>
    public async Task<IEnumerable<ChatMessageDto>> GetLatestMessagesAsync(IEnumerable<string> conversationIds, CancellationToken cancellationToken = default)
    {
        var ids = conversationIds.ToList();
        if (ids.Count == 0) return Enumerable.Empty<ChatMessageDto>();
        // Edge case input rỗng thì trả rỗng ngay để tránh chạy aggregate không cần thiết.

        var pipeline = new EmptyPipelineDefinition<ChatMessageDocument>()
            .Match(Builders<ChatMessageDocument>.Filter.And(
                Builders<ChatMessageDocument>.Filter.In(m => m.ConversationId, ids),
                Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false)))
            .Sort(Builders<ChatMessageDocument>.Sort.Combine(
                Builders<ChatMessageDocument>.Sort.Descending(m => m.ConversationId),
                Builders<ChatMessageDocument>.Sort.Descending(m => m.CreatedAt)))
            .Group(m => m.ConversationId, g => g.First())
            .Project(m => m);
        // Group + First sau sort giúp lấy chính xác message mới nhất từng conversation.

        var docs = await _context.ChatMessages.Aggregate(pipeline, cancellationToken: cancellationToken).ToListAsync(cancellationToken);
        return docs.Select(ToDto);
    }

    private static bool CanRecoverSystemEventDuplicate(MongoWriteException exception, ChatMessageDocument doc)
    {
        return exception.WriteError?.Category == ServerErrorCategory.DuplicateKey
               && !string.IsNullOrWhiteSpace(doc.SystemEventKey);
    }

    private async Task<string?> ResolveExistingSystemEventMessageIdAsync(
        ChatMessageDocument doc,
        CancellationToken cancellationToken)
    {
        var filter = Builders<ChatMessageDocument>.Filter.And(
            Builders<ChatMessageDocument>.Filter.Eq(m => m.ConversationId, doc.ConversationId),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.SystemEventKey, doc.SystemEventKey));
        var existing = await _context.ChatMessages
            .Find(filter)
            .Project(m => m.Id)
            .FirstOrDefaultAsync(cancellationToken);
        return string.IsNullOrWhiteSpace(existing) ? null : existing;
    }
}

using MongoDB.Driver;
using MongoDB.Bson;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository chính quản lý conversation trên MongoDB.
public partial class MongoConversationRepository : IConversationRepository
{
    // Mongo context truy cập collection conversations.
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository conversation.
    /// Luồng xử lý: nhận MongoDbContext qua DI để thao tác dữ liệu conversation.
    /// </summary>
    public MongoConversationRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Thêm mới conversation.
    /// Luồng xử lý: map DTO sang document, insert Mongo và gán id phát sinh ngược về DTO.
    /// </summary>
    public async Task AddAsync(ConversationDto conversation, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(conversation);
        await _context.Conversations.InsertOneAsync(doc, cancellationToken: cancellationToken);
        conversation.Id = doc.Id;
        // Đồng bộ id để caller dùng cho các thao tác gửi message ngay sau khi tạo.
    }

    /// <summary>
    /// Lấy conversation theo id nếu chưa bị xóa mềm.
    /// Luồng xử lý: filter id + is_deleted=false rồi map DTO.
    /// </summary>
    public async Task<ConversationDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.Id, id),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        var doc = await _context.Conversations.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : ToDto(doc);
    }

    /// <summary>
    /// Lấy conversation active theo cặp user-reader.
    /// Luồng xử lý: lọc participant + trạng thái active và lấy bản ghi mới nhất.
    /// </summary>
    public async Task<ConversationDto?> GetActiveByParticipantsAsync(
        string userId,
        string readerId,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.UserId, userId),
            Builders<ConversationDocument>.Filter.Eq(c => c.ReaderId, readerId),
            Builders<ConversationDocument>.Filter.In(c => c.Status, new[]
            {
                ConversationStatus.Pending,
                ConversationStatus.AwaitingAcceptance,
                ConversationStatus.Ongoing
            }),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));
        // Chỉ ba trạng thái này được xem là còn hiệu lực để tiếp tục tương tác.

        var doc = await _context.Conversations
            .Find(filter)
            .SortByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : ToDto(doc);
    }

    /// <summary>
    /// Đếm số conversation active của một user.
    /// Luồng xử lý: dùng cùng tiêu chí active như GetActiveByParticipantsAsync để đảm bảo nhất quán.
    /// </summary>
    public Task<long> CountActiveByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.UserId, userId),
            Builders<ConversationDocument>.Filter.In(c => c.Status, new[]
            {
                ConversationStatus.Pending,
                ConversationStatus.AwaitingAcceptance,
                ConversationStatus.Ongoing
            }),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

        return _context.Conversations.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Lấy các conversation đến hạn auto resolve completion.
    /// Luồng xử lý: lọc conversation ongoing có confirm.auto_resolve_at <= dueAtUtc và chưa xóa, trả danh sách theo hạn sớm trước.
    /// </summary>
    public async Task<IReadOnlyList<ConversationDto>> GetConversationsAwaitingCompletionResolutionAsync(
        DateTime dueAtUtc,
        int limit = 200,
        CancellationToken cancellationToken = default)
    {
        var normalizedLimit = limit <= 0 ? 200 : Math.Min(limit, 1000);
        // Batch job cho phép limit cao hơn API thường nhưng vẫn có trần an toàn.

        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Eq(c => c.Status, ConversationStatus.Ongoing),
            Builders<ConversationDocument>.Filter.Ne(c => c.Confirm, null),
            Builders<ConversationDocument>.Filter.Ne("confirm.auto_resolve_at", BsonNull.Value),
            Builders<ConversationDocument>.Filter.Lte("confirm.auto_resolve_at", dueAtUtc),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));
        // Kiểm tra BsonNull để tránh lỗi khi trường nested chưa tồn tại trên dữ liệu cũ.

        var docs = await _context.Conversations
            .Find(filter)
            .SortBy(c => c.Confirm!.AutoResolveAt)
            .Limit(normalizedLimit)
            .ToListAsync(cancellationToken);

        return docs.Select(ToDto).ToList();
    }

    /// <summary>
    /// Cập nhật toàn bộ conversation.
    /// Luồng xử lý: map DTO sang document, ghi UpdatedAt hiện tại rồi replace theo id.
    /// </summary>
    public async Task UpdateAsync(ConversationDto conversation, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(conversation);
        doc.UpdatedAt = DateTime.UtcNow;
        // Luôn cập nhật mốc thời gian để theo dõi thay đổi trạng thái conversation.

        var filter = Builders<ConversationDocument>.Filter.Eq(c => c.Id, doc.Id);
        await _context.Conversations.ReplaceOneAsync(filter, doc, cancellationToken: cancellationToken);
    }
}

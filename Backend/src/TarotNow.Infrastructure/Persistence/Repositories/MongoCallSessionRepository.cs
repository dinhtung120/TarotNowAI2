using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository chính cho call session trên MongoDB.
public partial class MongoCallSessionRepository : ICallSessionRepository
{
    // Mongo context truy cập collection call_sessions.
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository call session.
    /// Luồng xử lý: nhận MongoDbContext từ DI để dùng chung cấu hình index/collection.
    /// </summary>
    public MongoCallSessionRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Thêm mới call session.
    /// Luồng xử lý: map DTO sang document, insert Mongo và ánh xạ id phát sinh ngược về DTO.
    /// </summary>
    public async Task AddAsync(CallSessionDto session, CancellationToken ct = default)
    {
        var document = CallSessionDocumentMapper.ToDocument(session);
        try
        {
            await _context.CallSessions.InsertOneAsync(document, cancellationToken: ct);
        }
        catch (MongoWriteException ex) when (ex.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            throw new InvalidOperationException("Đang có một cuộc gọi active khác trong cuộc trò chuyện này.");
            // Unique partial index bảo vệ business rule một cuộc gọi active cho mỗi conversation.
        }

        session.Id = document.Id;
        // Đồng bộ id về caller để các bước signaling tiếp theo dùng cùng session id.
    }

    /// <summary>
    /// Lấy call session theo id.
    /// Luồng xử lý: query document và map sang DTO nếu tìm thấy.
    /// </summary>
    public async Task<CallSessionDto?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        var doc = await _context.CallSessions.Find(x => x.Id == id).FirstOrDefaultAsync(ct);
        return doc == null ? null : CallSessionDocumentMapper.ToDto(doc);
    }

    /// <summary>
    /// Lấy call session đang active của một conversation.
    /// Luồng xử lý: lọc theo conversationId và trạng thái requested/accepted.
    /// </summary>
    public async Task<CallSessionDto?> GetActiveByConversationAsync(string conversationId, CancellationToken ct = default)
    {
        var filter = Builders<CallSessionDocument>.Filter.Eq(x => x.ConversationId, conversationId) &
                     Builders<CallSessionDocument>.Filter.In(x => x.Status, new[] { "requested", "accepted" });
        // Hai trạng thái này được xem là active theo chính sách cuộc gọi hiện tại.

        var doc = await _context.CallSessions.Find(filter).FirstOrDefaultAsync(ct);
        return doc == null ? null : CallSessionDocumentMapper.ToDto(doc);
    }

    /// <summary>
    /// Lấy các call session active cho nhiều conversation.
    /// Luồng xử lý: chuẩn hóa danh sách id, trả rỗng nếu không có phần tử, rồi query theo In(conversationId)+active statuses.
    /// </summary>
    public async Task<IEnumerable<CallSessionDto>> GetActiveByConversationIdsAsync(IEnumerable<string> conversationIds, CancellationToken ct = default)
    {
        var ids = conversationIds.ToList();
        if (ids.Count == 0) return Enumerable.Empty<CallSessionDto>();
        // Edge case: input rỗng thì tránh phát sinh truy vấn Mongo không cần thiết.

        var filter = Builders<CallSessionDocument>.Filter.In(x => x.ConversationId, ids) &
                     Builders<CallSessionDocument>.Filter.In(x => x.Status, new[] { "requested", "accepted" });

        var docs = await _context.CallSessions.Find(filter).ToListAsync(ct);
        return docs.Select(CallSessionDocumentMapper.ToDto);
    }
}

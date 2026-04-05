using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    /// <summary>
    /// Tạo index cho các collection liên quan đến Chat (conversations, messages, reports).
    /// Sử dụng SafeCreateIndex thay vì gọi trực tiếp để tránh lỗi
    /// khi index cũ đã tồn tại với tên khác.
    /// </summary>
    private void EnsureChatCollectionIndexes()
    {
        // --- Conversations ---
        // Index hỗ trợ truy vấn: "danh sách hội thoại của user, chưa xoá, theo trạng thái, mới nhất"
        SafeCreateIndex(Conversations, new CreateIndexModel<ConversationDocument>(
            Builders<ConversationDocument>.IndexKeys
                .Ascending(c => c.IsDeleted)
                .Ascending(c => c.UserId)
                .Ascending(c => c.Status)
                .Descending(c => c.UpdatedAt),
            new CreateIndexOptions { Name = "idx_isdeleted_userid_status_updatedat" }));

        // Index cho reader xem danh sách hội thoại mà họ tham gia
        SafeCreateIndex(Conversations, new CreateIndexModel<ConversationDocument>(
            Builders<ConversationDocument>.IndexKeys
                .Ascending(c => c.IsDeleted)
                .Ascending(c => c.ReaderId)
                .Ascending(c => c.Status)
                .Descending(c => c.UpdatedAt),
            new CreateIndexOptions { Name = "idx_isdeleted_readerid_status_updatedat" }));

        // --- ChatMessages ---
        // Index lấy tin nhắn theo conversation, bỏ qua đã xoá, sắp theo thời gian
        SafeCreateIndex(ChatMessages, new CreateIndexModel<ChatMessageDocument>(
            Builders<ChatMessageDocument>.IndexKeys
                .Ascending(m => m.ConversationId)
                .Ascending(m => m.IsDeleted)
                .Descending(m => m.CreatedAt),
            new CreateIndexOptions { Name = "idx_conversationid_isdeleted_createdat_desc" }));

        // Index tra cứu tin nhắn theo người gửi (dành cho moderation)
        SafeCreateIndex(ChatMessages, new CreateIndexModel<ChatMessageDocument>(
            Builders<ChatMessageDocument>.IndexKeys
                .Ascending(m => m.SenderId)
                .Descending(m => m.CreatedAt),
            new CreateIndexOptions { Name = "idx_senderid_createdat_desc" }));

        // --- Reports ---
        // Index lọc báo cáo theo trạng thái (pending/resolved), mới nhất trước
        SafeCreateIndex(Reports, new CreateIndexModel<ReportDocument>(
            Builders<ReportDocument>.IndexKeys
                .Ascending(r => r.Status)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_status_createdat_desc" }));

        // Index tìm báo cáo theo đối tượng bị báo cáo (target type + target id)
        SafeCreateIndex(Reports, new CreateIndexModel<ReportDocument>(
            Builders<ReportDocument>.IndexKeys
                .Ascending(r => r.Target.Type)
                .Ascending(r => r.Target.Id)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_targettype_targetid_createdat_desc" }));
    }
}

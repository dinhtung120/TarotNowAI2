using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

// Khối cấu hình index cho collection call_sessions.
public partial class MongoDbContext
{
    /// <summary>
    /// Bảo đảm toàn bộ index cho call_sessions được tạo khi ứng dụng khởi động.
    /// Luồng xử lý: tạo batch index trong try-catch và chỉ ghi log lỗi để không chặn startup toàn hệ thống.
    /// </summary>
    private void EnsureCallIndexes()
    {
        try
        {
            CallSessions.Indexes.CreateMany(CreateCallSessionIndexes());
            // Tạo batch để giảm số round-trip xuống Mongo khi khởi tạo nhiều index.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MongoDB] Không thể tạo indexes cho collection call_sessions.");
            // Ưu tiên tính sẵn sàng dịch vụ, lỗi index sẽ được theo dõi qua log thay vì crash app.
        }
    }

    /// <summary>
    /// Tập hợp danh sách index cần có cho call session.
    /// Luồng xử lý: trả về theo thứ tự timeline, trạng thái, uniqueness active call và TTL dọn dữ liệu cũ.
    /// </summary>
    private static IEnumerable<CreateIndexModel<CallSessionDocument>> CreateCallSessionIndexes()
    {
        return
        [
            CreateConversationTimelineIndex(),
            CreateStatusConversationIndex(),
            CreateInitiatorTimelineIndex(),
            CreateActiveConversationUniqueIndex(),
            CreateCallSessionTtlIndex()
        ];
    }

    /// <summary>
    /// Tạo index phục vụ lấy lịch sử cuộc gọi theo conversation mới nhất trước.
    /// Luồng xử lý: sắp theo conversation_id + created_at desc để tối ưu truy vấn timeline hội thoại.
    /// </summary>
    private static CreateIndexModel<CallSessionDocument> CreateConversationTimelineIndex()
    {
        return new CreateIndexModel<CallSessionDocument>(
            Builders<CallSessionDocument>.IndexKeys
                .Ascending(x => x.ConversationId)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Background = true });
    }

    /// <summary>
    /// Tạo index cho bộ lọc theo trạng thái trong từng conversation.
    /// Luồng xử lý: đặt status trước conversation_id để phù hợp các truy vấn trạng thái cuộc gọi.
    /// </summary>
    private static CreateIndexModel<CallSessionDocument> CreateStatusConversationIndex()
    {
        return new CreateIndexModel<CallSessionDocument>(
            Builders<CallSessionDocument>.IndexKeys
                .Ascending(x => x.Status)
                .Ascending(x => x.ConversationId),
            new CreateIndexOptions { Background = true });
    }

    /// <summary>
    /// Tạo index lịch sử cuộc gọi theo người khởi tạo.
    /// Luồng xử lý: dùng initiator_id + created_at desc để lấy nhanh danh sách gần nhất của một user.
    /// </summary>
    private static CreateIndexModel<CallSessionDocument> CreateInitiatorTimelineIndex()
    {
        return new CreateIndexModel<CallSessionDocument>(
            Builders<CallSessionDocument>.IndexKeys
                .Ascending(x => x.InitiatorId)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Background = true });
    }

    /// <summary>
    /// Tạo unique partial index để mỗi conversation chỉ có tối đa một cuộc gọi đang hoạt động.
    /// Luồng xử lý: lọc các trạng thái active rồi áp unique trên conversation_id để chặn tạo cuộc gọi trùng.
    /// </summary>
    private static CreateIndexModel<CallSessionDocument> CreateActiveConversationUniqueIndex()
    {
        var activeCallFilter = Builders<CallSessionDocument>.Filter.In(x => x.Status, ["requested", "accepted"]);
        // Chỉ coi requested/accepted là active vì đây là các trạng thái đang chiếm slot hội thoại.

        return new CreateIndexModel<CallSessionDocument>(
            Builders<CallSessionDocument>.IndexKeys.Ascending(x => x.ConversationId),
            new CreateIndexOptions<CallSessionDocument>
            {
                Background = true,
                Unique = true,
                Name = "uq_call_active_per_conversation",
                PartialFilterExpression = activeCallFilter
            });
    }

    /// <summary>
    /// Tạo TTL index tự dọn call session cũ.
    /// Luồng xử lý: hết 90 ngày từ created_at thì Mongo tự xóa để giảm chi phí lưu trữ dài hạn.
    /// </summary>
    private static CreateIndexModel<CallSessionDocument> CreateCallSessionTtlIndex()
    {
        return new CreateIndexModel<CallSessionDocument>(
            Builders<CallSessionDocument>.IndexKeys.Ascending(x => x.CreatedAt),
            new CreateIndexOptions
            {
                Background = true,
                ExpireAfter = TimeSpan.FromDays(90)
            });
    }
}

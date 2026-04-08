

using TarotNow.Application.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract quản lý tin nhắn hội thoại để đảm bảo luồng chat và trạng thái đọc nhất quán.
public interface IChatMessageRepository
{
    /// <summary>
    /// Lấy tin nhắn theo id để phục vụ truy vết hoặc xử lý nghiệp vụ liên quan.
    /// Luồng xử lý: truy vấn theo khóa tin nhắn và trả null nếu không tồn tại.
    /// </summary>
    Task<ChatMessageDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tạo tin nhắn mới vào cuộc hội thoại khi có nội dung gửi lên hệ thống.
    /// Luồng xử lý: nhận DTO tin nhắn đã hợp lệ và persist bản ghi mới.
    /// </summary>
    Task AddAsync(ChatMessageDto message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách tin nhắn theo conversation có phân trang để hiển thị lịch sử.
    /// Luồng xử lý: lọc theo conversationId, áp page/pageSize và trả items kèm tổng bản ghi.
    /// </summary>
    Task<(IEnumerable<ChatMessageDto> Items, long TotalCount)> GetByConversationIdPaginatedAsync(
        string conversationId, int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách tin nhắn theo cursor để hỗ trợ phân trang cuộn vô hạn.
    /// Luồng xử lý: dùng cursor hiện tại làm mốc, lấy tối đa limit bản ghi kế tiếp và trả NextCursor.
    /// </summary>
    Task<(IReadOnlyList<ChatMessageDto> Items, string? NextCursor)> GetByConversationIdCursorAsync(
        string conversationId,
        string? cursor,
        int limit,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Kiểm tra đã có phản hồi cho payment offer hay chưa để tránh xử lý trùng.
    /// Luồng xử lý: đối chiếu conversationId và offerMessageId, trả true nếu đã phát sinh phản hồi.
    /// </summary>
    Task<bool> HasPaymentOfferResponseAsync(
        string conversationId,
        string offerMessageId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Tìm payment offer pending mới nhất để tiếp tục xử lý timeout hoặc follow-up.
    /// Luồng xử lý: lọc tin nhắn pending theo conversationId và chọn bản ghi gần nhất.
    /// </summary>
    Task<ChatMessageDto?> FindLatestPendingPaymentOfferAsync(
        string conversationId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách payment offer pending đã hết hạn để chạy job xử lý nền.
    /// Luồng xử lý: so sánh nowUtc với hạn xử lý, trả tối đa limit bản ghi cần xử lý.
    /// </summary>
    Task<IReadOnlyList<ChatMessageDto>> GetExpiredPendingPaymentOffersAsync(
        DateTime nowUtc,
        int limit = 200,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Đánh dấu đã đọc toàn bộ tin nhắn trong conversation cho một người tham gia.
    /// Luồng xử lý: cập nhật cờ đọc theo conversationId/readerId và trả số bản ghi bị ảnh hưởng.
    /// </summary>
    Task<long> MarkAsReadAsync(string conversationId, string readerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy tin nhắn mới nhất của nhiều conversation để dựng danh sách hội thoại tổng hợp.
    /// Luồng xử lý: batch query theo conversationIds và trả mỗi cuộc hội thoại một mốc tin gần nhất.
    /// </summary>
    Task<IEnumerable<ChatMessageDto>> GetLatestMessagesAsync(IEnumerable<string> conversationIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật cờ đánh dấu của tin nhắn cho nhu cầu moderation hoặc cảnh báo.
    /// Luồng xử lý: xác định messageId, ghi trạng thái isFlagged mới và lưu thay đổi.
    /// </summary>
    Task UpdateFlagAsync(string messageId, bool isFlagged, CancellationToken cancellationToken = default);
}

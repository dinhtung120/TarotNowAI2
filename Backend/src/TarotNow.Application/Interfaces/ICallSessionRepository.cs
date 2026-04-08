using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Interfaces;

// Contract quản lý phiên gọi để đồng bộ trạng thái cuộc gọi trong phiên trò chuyện.
public interface ICallSessionRepository
{
    /// <summary>
    /// Tạo phiên gọi mới ngay khi cuộc gọi được khởi tạo ở tầng nghiệp vụ.
    /// Luồng xử lý: nhận DTO phiên gọi đã chuẩn hóa và persist thành bản ghi mới.
    /// </summary>
    Task AddAsync(CallSessionDto session, CancellationToken ct = default);

    /// <summary>
    /// Lấy chi tiết phiên gọi theo id để theo dõi trạng thái xử lý hiện tại.
    /// Luồng xử lý: truy vấn chính xác theo định danh và trả về null nếu không tồn tại.
    /// </summary>
    Task<CallSessionDto?> GetByIdAsync(string id, CancellationToken ct = default);

    /// <summary>
    /// Lấy phiên gọi đang hoạt động của một cuộc hội thoại để tránh mở trùng phiên.
    /// Luồng xử lý: lọc theo conversationId và trạng thái active, trả về tối đa một phiên.
    /// </summary>
    Task<CallSessionDto?> GetActiveByConversationAsync(string conversationId, CancellationToken ct = default);

    /// <summary>
    /// Lấy các phiên gọi active theo nhiều cuộc hội thoại để tối ưu truy vấn danh sách.
    /// Luồng xử lý: batch filter theo conversationIds và trả tập phiên còn hiệu lực.
    /// </summary>
    Task<IEnumerable<CallSessionDto>> GetActiveByConversationIdsAsync(IEnumerable<string> conversationIds, CancellationToken ct = default);

    /// <summary>
    /// Cập nhật trạng thái phiên gọi kèm mốc thời gian để đảm bảo vòng đời phiên nhất quán.
    /// Luồng xử lý: kiểm tra trạng thái kỳ vọng (nếu có), cập nhật dữ liệu mới và trả kết quả thành công/thất bại.
    /// </summary>
    Task<bool> UpdateStatusAsync(
        string id,
        CallSessionStatus newStatus,
        DateTime? startedAt = null,
        DateTime? endedAt = null,
        string? endReason = null,
        CallSessionStatus? expectedPreviousStatus = null,
        CancellationToken ct = default);

    /// <summary>
    /// Lấy lịch sử phiên gọi theo cuộc hội thoại có phân trang để hiển thị theo từng trang.
    /// Luồng xử lý: lọc theo conversationId, áp page/pageSize, rồi trả danh sách cùng tổng bản ghi.
    /// </summary>
    Task<(IEnumerable<CallSessionDto> Items, long TotalCount)> GetByConversationIdPaginatedAsync(
        string conversationId,
        int page,
        int pageSize,
        CancellationToken ct = default);
}

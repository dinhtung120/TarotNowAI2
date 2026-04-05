using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Interface cho repository quản lý Call Session.
/// Chịu trách nhiệm tương tác với bảng (collection) lưu trữ siêu dữ liệu cuộc gọi.
/// </summary>
public interface ICallSessionRepository
{
    /// <summary>
    /// Tạo mới một call session. Ban đầu cuộc gọi luôn ở trạng thái Requested.
    /// </summary>
    Task AddAsync(CallSessionDto session, CancellationToken ct = default);

    /// <summary>
    /// Lấy thông tin một call session theo ID.
    /// </summary>
    Task<CallSessionDto?> GetByIdAsync(string id, CancellationToken ct = default);

    /// <summary>
    /// Kiểm tra xem trong conversation đang có cuộc gọi nào ĐANG DIỄN RA (Requested hoặc Accepted) hay không.
    /// Dùng để giới hạn 1 cuộc trò chuyện chỉ có tối đa 1 cuộc gọi cùng lúc.
    /// </summary>
    Task<CallSessionDto?> GetActiveByConversationAsync(string conversationId, CancellationToken ct = default);

    /// <summary>
    /// Lấy danh sách các cuộc gọi ĐANG DIỄN RA dựa theo danh sách conversation Ids (tránh N+1 query)
    /// </summary>
    Task<IEnumerable<CallSessionDto>> GetActiveByConversationIdsAsync(IEnumerable<string> conversationIds, CancellationToken ct = default);

    /// <summary>
    /// Cập nhật nguyên trạng thái của cuộc gọi.
    /// Database lưu theo dạng atomic update để đảm bảo tính nhất quán (không thay đổi field khác).
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
    /// Lấy danh sách lịch sử các cuộc gọi trong một conversation có phân trang.
    /// Danh sách được sắp xếp mới nhất ở đầu (CreatedAt DESC).
    /// </summary>
    Task<(IEnumerable<CallSessionDto> Items, long TotalCount)> GetByConversationIdPaginatedAsync(
        string conversationId,
        int page,
        int pageSize,
        CancellationToken ct = default);
}

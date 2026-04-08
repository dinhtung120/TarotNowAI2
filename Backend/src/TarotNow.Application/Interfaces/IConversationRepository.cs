

using TarotNow.Application.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract quản lý conversation để đồng bộ trạng thái phiên chat giữa người dùng và reader.
public interface IConversationRepository
{
    /// <summary>
    /// Tạo conversation mới khi hai bên bắt đầu phiên trao đổi.
    /// Luồng xử lý: persist DTO conversation đã hợp lệ vào kho dữ liệu.
    /// </summary>
    Task AddAsync(ConversationDto conversation, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy conversation theo id để xử lý nghiệp vụ theo phiên cụ thể.
    /// Luồng xử lý: truy vấn theo khóa conversation và trả null nếu không tồn tại.
    /// </summary>
    Task<ConversationDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy conversation active giữa user và reader để tái sử dụng phiên đang mở.
    /// Luồng xử lý: lọc theo cặp participant và trạng thái active, trả null nếu chưa có phiên hợp lệ.
    /// </summary>
    Task<ConversationDto?> GetActiveByParticipantsAsync(
        string userId, string readerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách conversation theo user có phân trang để hiển thị hộp thư.
    /// Luồng xử lý: lọc theo userId và statuses tùy chọn, trả items cùng tổng bản ghi.
    /// </summary>
    Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByUserIdPaginatedAsync(
        string userId, int page, int pageSize, IReadOnlyCollection<string>? statuses = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách conversation theo reader có phân trang để phục vụ dashboard reader.
    /// Luồng xử lý: lọc theo readerId và statuses, phân trang rồi trả kết quả tổng hợp.
    /// </summary>
    Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByReaderIdPaginatedAsync(
        string readerId, int page, int pageSize, IReadOnlyCollection<string>? statuses = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách conversation theo một participant bất kỳ để tái sử dụng chung cho nhiều role.
    /// Luồng xử lý: lọc participantId kết hợp statuses, áp page/pageSize và trả items + total.
    /// </summary>
    Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByParticipantIdPaginatedAsync(
        string participantId, int page, int pageSize, IReadOnlyCollection<string>? statuses = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đếm conversation active theo user để kiểm soát hạn mức phiên đồng thời.
    /// Luồng xử lý: lọc conversation trạng thái active của userId và trả tổng số.
    /// </summary>
    Task<long> CountActiveByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy các conversation chờ xử lý completion resolution để chạy job hậu kiểm.
    /// Luồng xử lý: lọc conversation tới hạn dueAtUtc, giới hạn theo limit và trả danh sách cần xử lý.
    /// </summary>
    Task<IReadOnlyList<ConversationDto>> GetConversationsAwaitingCompletionResolutionAsync(
        DateTime dueAtUtc,
        int limit = 200,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Tính tổng số tin chưa đọc của participant để hiển thị badge thông báo.
    /// Luồng xử lý: cộng dồn unread theo participantId trên toàn bộ conversation liên quan.
    /// </summary>
    Task<int> GetTotalUnreadCountAsync(string participantId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật conversation khi có thay đổi trạng thái hoặc metadata phiên chat.
    /// Luồng xử lý: ghi đè dữ liệu conversation tương ứng theo id hiện có.
    /// </summary>
    Task UpdateAsync(ConversationDto conversation, CancellationToken cancellationToken = default);
}

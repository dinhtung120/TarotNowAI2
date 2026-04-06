/*
 * ===================================================================
 * FILE: IConversationRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bản Vẽ Giao Tiếp Giám Sát Tổ Chức Các Hộp Hội Thoại (Conversations).
 *   Mỗi Cuộc Gọi Là 1 Khối Hội Tụ Thể Hiện Khách Chat Cùng Với Thầy Mới Khai Sinh Được.
 * ===================================================================
 */

using TarotNow.Application.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Trung Tâm Cuộc Trực Tổng Đài Hỗ Trợ Lưu Trữ Mã Hồ Sơ Của Bạn Mạng Khách.
/// 1 Phòng Này Gắn Liền Mật Thiết Với Vụ Tức Khí Đóng Giữ Tiền PostgreSQL Kìa.
/// </summary>
public interface IConversationRepository
{
    /// <summary>Tức Tốc Đăng Kỳ Kế Hoạch Có Thằng Phòng Thăm Bóng Khách Vào Nhà Đặt Phòng.</summary>
    Task AddAsync(ConversationDto conversation, CancellationToken cancellationToken = default);

    /// <summary>Kiểm Phẩm ID Chuỗi Bằng MongoDB Tra Sổ Tay.</summary>
    Task<ConversationDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kẻo Đụng Đầu Đóng Gây Mâu Thuẫn Bạc Mạng: Hai Người Này Đang Chửi Nhau Phòng Này Chênh Lệch Thì Không Cho Khóa Cửa Lập Box Khác Đang Oang Oang Cãi.
    /// Check Phòng Active (Làm 1 App 1 Người Box Riêng Giống Zalo Tránh Duplicate).
    /// </summary>
    Task<ConversationDto?> GetActiveByParticipantsAsync(
        string userId, string readerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tổng Đài Điều Hành Giao Diện Message Sidebar của Cậu Bé Lạ Này Tới Các Nhà Tiên Tri. (Inbox Khách Hỏi).
    /// </summary>
    Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByUserIdPaginatedAsync(
        string userId, int page, int pageSize, IReadOnlyCollection<string>? statuses = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tổng Đài Điều Hành Hộp Thư Phía Sau Hậu Cung Dành Cho Thầy (Thầy có 50 hộp thư đập cửa hằng hằng đòi hỏi bốc lịch xin bài). => Inbox cho Reader.
    /// </summary>
    Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByReaderIdPaginatedAsync(
        string readerId, int page, int pageSize, IReadOnlyCollection<string>? statuses = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yêu cầu mới: Hộp thư Gộp Chung Cho Cả Hai Vai Trò Mở Cửa Sẵn: Lấy Các Cuộc Trò Chuyện Trong Đó Bạn Là `UserId` Hoặc `ReaderId`!
    /// </summary>
    Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByParticipantIdPaginatedAsync(
        string participantId, int page, int pageSize, IReadOnlyCollection<string>? statuses = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đếm số cuộc trò chuyện chưa kết thúc của một user theo vai trò user-side.
    /// Dùng để enforce giới hạn active room trên mỗi user.
    /// </summary>
    Task<long> CountActiveByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ConversationDto>> GetConversationsAwaitingCompletionResolutionAsync(
        DateTime dueAtUtc,
        int limit = 200,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Tính tổng số tin nhắn chưa đọc của một người tham gia qua tất cả các phòng chat.
    /// </summary>
    Task<int> GetTotalUnreadCountAsync(string participantId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thay Đổi Điểm Báo Mốc Thời Gian Tin Rán Lên Top Kéo Box Mới Bay Đầu Mục Gần Đây! (Bumpy Tăng Dễ Thấy Gửi Lần Nữa Trồi Lên).
    /// </summary>
    Task UpdateAsync(ConversationDto conversation, CancellationToken cancellationToken = default);
}
